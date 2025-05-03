using HarmonyLib;
using Wildskies.UI.Hud;

namespace HeraldTracker
{
    class HeraldTrackerPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(HeraldMovementController), "Start")]
        static void AddHeraldToCompass(HeraldMovementController __instance)
        {
            UnityEngine.Object.FindFirstObjectByType<CompassHud>().OnPingPlaced("HeraldTracker", __instance.gameObject.transform);
        }
    }
}
