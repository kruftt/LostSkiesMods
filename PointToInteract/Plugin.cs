using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;

namespace PointToInteract;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class PointToInteractPlugin : BasePlugin
{
    public override void Load()
    {
        Harmony harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
        harmony.PatchAll(typeof(PointToInteractPatch));
    }
}