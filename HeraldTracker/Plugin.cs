using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using Wildskies.UI.Hud;

namespace HeraldTracker
{
    [BepInPlugin("org.kruft.plugins.heraldtracker", "Herald Tracker", "0.1.1")]
    public class HeraldTrackerPlugin : BasePlugin
    {
        public override void Load()
        {
            new Harmony("kruft.HeraldTracker").PatchAll();
        }
    }

    [HarmonyPatch(typeof(HeraldMovementController), "Start")]
    class HeraldTrackerPatch
    {
        static void Postfix(HeraldMovementController __instance)
        {
            UnityEngine.Object.FindFirstObjectByType<CompassHud>().OnPingPlaced("HeraldTracker", __instance.gameObject.transform);
        }
    }
}