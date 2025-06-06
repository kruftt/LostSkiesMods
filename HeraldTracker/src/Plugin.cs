﻿namespace HeraldTracker;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class HeraldTrackerPlugin : BasePlugin
{
    public static ManualLogSource log = Logger.CreateLogSource(MyPluginInfo.PLUGIN_NAME);

    public override void Load()
    {
        Harmony harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
        harmony.PatchAll(typeof(Tracker));

        if (Config.Bind<bool>("General", "Herald_Tracking", true, "Whether to track the herald.").Value)
            harmony.PatchAll(typeof(HeraldTracker));

        if (Config.Bind<bool>("General", "Whale_Tracking", true, "Whether to track whales.").Value)
            harmony.PatchAll(typeof(WhaleTracker));
    }
}