using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;

namespace HeraldTracker
{
    [BepInPlugin("org.kruft.plugins.heraldtracker", "Herald Tracker", "0.1.1")]
    public class HeraldTrackerPlugin : BasePlugin
    {
        public override void Load()
        {
            Harmony harmony = new Harmony("kruft.HeraldTracker");
            harmony.PatchAll(typeof(HeraldTrackerPatch));
        }
    }
}