
using HarmonyLib;

namespace HideTaskUI
{
    class HideTaskUIPatch
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(Wildskies.UI.Hud.ActiveTaskHud), "UpdateActiveTask")]
        static bool ClearTasksAndDontUpdate(Wildskies.UI.Hud.ActiveTaskHud __instance)
        {
            __instance.ClearActiveTask();
            // __instance.Hide();
            return false;
        }
    }
}
