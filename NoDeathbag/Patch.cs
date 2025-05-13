using Bossa.Cinematika.Modules;
using WildSkies.Player;
namespace NoDeathbag;

class NoDeathbagPatch
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(LocalPlayer), nameof(LocalPlayer.OnEntityDeath))]
    private static bool OnEntityDeath_SkipDeathbag(LocalPlayer __instance)
    {
        var _settings = __instance._cameraManager.GetModule<CameraVisualSettings>();
        _settings.AddVisualAction(new BloomFlash(1f, 4f));
        _settings.AddVisualAction(__instance._deathCameraEffect);
        return false;
    }
}