using HarmonyLib;
using WildSkies.AI;
namespace HeraldTracker.Patches;

class WhaleTracker
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(SkyWhaleCritter), nameof(SkyWhaleCritter.Init))]
    private static void AddWhaleToCompass(SkyWhaleCritter __instance)
    {
        Tracker.Track(__instance.name, __instance.transform);
        Tracker.Notify("A whale has been spotted!");
        HeraldTrackerPlugin.log.LogInfo("Added whale to compass.");
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(SkyWhaleCritter), nameof(SkyWhaleCritter.OnDisable))]
    private static void RemoveWhaleFromCompass(SkyWhaleCritter __instance)
    {
        Tracker.Untrack(__instance.transform);
        Tracker.Notify("The whale slips into the clouds...");
        HeraldTrackerPlugin.log.LogInfo("Removed whale from compass.");
    }
}