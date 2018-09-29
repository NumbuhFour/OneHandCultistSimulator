using Assets.Core.Entities;
using Assets.CS.TabletopUI;
using Assets.TabletopUi;

using Harmony;
using System.Reflection;
using UnityEngine;

namespace OneHandCultistSimulator
{

    internal class OneHandModuleInitializer : Partiality.Modloader.PartialityMod
    {
        public override void Init()
        {
            //FileLog.Log("Patching with " + typeof(ModuleInitializer).FullName);

            try
            {
                var harmony = HarmonyInstance.Create("cultistsimulatorinputmod.onehand");
                harmony.PatchAll(Assembly.GetExecutingAssembly());

                //FileLog.Log("Working! " + typeof(ModuleInitializer).FullName);
            }
            catch (System.Exception e)
            {
                //FileLog.Log(e.ToString());
            }
        }
    }

    [HarmonyPatch(typeof(TabletopManager))]
    [HarmonyPatch("Update")]
    class KeyPatch
    {
        static void Postfix()
        {
            if (Input.GetMouseButtonDown(1)) // Right-Click
            {
                var table = Registry.Retrieve<TabletopManager>();
                table.SetPausedState(!table.GetPausedState());
                //__instance._speedController.TogglePause();
            }

            if (Input.GetMouseButtonDown(2)) // Middle Mouse
            {
                System.Collections.Generic.List<SituationController> registeredSituations = Registry.Retrieve<SituationsCatalogue>().GetRegisteredSituations();
                foreach (SituationController item in registeredSituations)
                {
                    if (item.IsOpen)
                    {
                        item.AttemptActivateRecipe(); // Attempt start recipe
                        item.DumpAllResults(); // Attempt collect all
                        break;
                    }
                }
            }
        }
    }
}
