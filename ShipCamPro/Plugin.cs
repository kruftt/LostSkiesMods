using BepInEx.Unity.IL2CPP;
using BepInEx;
using HarmonyLib;

namespace ShipCamPro;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class ShipCamProPlugin : BasePlugin
{
    public override void Load()
    {
        Harmony harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
        harmony.PatchAll(typeof(ShipCamProPatch));
    }
}