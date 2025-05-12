using Wildskies.UI.Hud;
namespace HideTaskUI;

class HideTaskUIPatch
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(ActiveTaskHud), nameof(ActiveTaskHud.UpdateActiveTask))]
    private static bool ClearTasksAndDontUpdate(ActiveTaskHud __instance)
    {
        __instance.ClearActiveTask();
        // __instance.Hide();
        return false;
    }
}