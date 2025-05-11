using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
namespace NoDeathbag;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class NoDeathbagPlugin : BasePlugin
{
    public override void Load()
    {
        Harmony harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
        harmony.PatchAll(typeof(NoDeathbagPatch));
    }
}