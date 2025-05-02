using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;

namespace HideTaskUI
{
    [BepInPlugin("org.kruft.plugins.HideTaskUI", "Hide Task UI", "0.1.0")]
    public class HideTaskUIPlugin : BasePlugin
    {
        public override void Load()
        {
            new Harmony("kruft.HideTaskUI").PatchAll();
        }
    }

    [HarmonyPatch(typeof(Wildskies.UI.Hud.ActiveTaskHud), "UpdateActiveTask")]
    class TaskUIPatch
    {
        static bool Prefix(Wildskies.UI.Hud.ActiveTaskHud __instance)
        {
            __instance.ClearActiveTask();
            return false;
        }

        //static void Postfix(Wildskies.UI.Hud.ActiveTaskHud __instance)
        //{
        //    __instance.Hide();
        //}
    }
}