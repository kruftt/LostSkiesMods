using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using BepInEx.Logging;

namespace HeraldTracker
{
    [BepInPlugin("kruft.heraldtracker", "Herald Tracker", "0.2.0")]
    public class HeraldTrackerPlugin : BasePlugin
    {
        public static ManualLogSource log = Logger.CreateLogSource("Herald Tracker");
        
        public override void Load()
        {
            Harmony harmony = new Harmony("kruft.HeraldTracker");
            harmony.PatchAll(typeof(Patches.Tracker));
            
            if (Config.Bind<bool>("General", "Herald_Tracking", true, "Whether to track the herald.").Value)
                harmony.PatchAll(typeof(Patches.HeraldTracker));

            if (Config.Bind<bool>("General", "Whale_Tracking", true, "Whether to track whales.").Value)
                harmony.PatchAll(typeof(Patches.WhaleTracker));
        }
    }
}