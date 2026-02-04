using BepInEx.Configuration;

namespace ShipCamPro;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class ShipCamProPlugin : BasePlugin
{
    public static ConfigEntry<bool> cameraLock;
    public static ConfigEntry<bool> cameraWrap;
    public static ConfigEntry<float> maxZoom;

    public override void Load()
    {
        Harmony harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
        harmony.PatchAll(typeof(ShipCamProPatch));

        cameraLock = Config.Bind<bool>("General", "Lock_Camera", true, "Lock the free-look camera in place.");
        cameraWrap = Config.Bind<bool>("General", "Wrap_Camera", true, "Allow the camera to wrap around.");
        maxZoom = Config.Bind<float>("General", "Max_Zoom", 120.0f, "Maximum zoom distance.");
    }
}