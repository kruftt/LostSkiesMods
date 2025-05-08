using HarmonyLib;

namespace HeraldTracker.Patches
{
    class HeraldTracker
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(HeraldMovementController), "Start")]
        static void AddHeraldToCompass(HeraldMovementController __instance)
        {
            Tracker.Track("HeraldTracker", __instance.gameObject.transform);
            HeraldTrackerPlugin.log.LogInfo("Added Herald to compass.");
        }
    }
}