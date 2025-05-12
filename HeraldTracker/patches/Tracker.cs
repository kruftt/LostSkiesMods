using UnityEngine;
using UnityEngine.Localization;
using Wildskies.UI.Hud;
namespace HeraldTracker.Patches;

public class Tracker
{
    private static CompassHud _compassHud;

    public static void Notify(string message, Sprite icon = null)
    {
        NotificationHud notificationHud = UnityEngine.Object.FindFirstObjectByType<NotificationHud>();

        if (notificationHud == null)
        {
            HeraldTrackerPlugin.log.LogWarning("Notify: NotificationHud not found.");
            return;
        }

        if (icon == null)
        {
            if (_compassHud == null)
            {
                HeraldTrackerPlugin.log.LogWarning("Notify: CompassHud not found.");
                return;
            }
            icon = _compassHud._compassIconPrefab._pingIcon;
        }

        notificationHud.Show(new NotificationHudPayload(icon, new LocalizedString("General", "ok.label")).Cast<IPayload>());
        notificationHud._currentNotification._message.text = message;
    }

    public static void Track(string name, Transform target)
    {
        if (_compassHud == null)
        {
            HeraldTrackerPlugin.log.LogWarning("Track: CompassHud not found.");
            return;
        }
        _compassHud.OnPingPlaced(name, target);
    }

    public static void Untrack(Transform target)
    {
        if (_compassHud == null)
        {
            HeraldTrackerPlugin.log.LogWarning("Untrack: CompassHud not found.");
            return;
        }
        _compassHud.OnPingCleared(target);
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(CompassHud), nameof(CompassHud.Start))]
    private static void CacheCompassHud(CompassHud __instance)
    {
        _compassHud = __instance;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(CompassHud), nameof(CompassHud.OnDestroy))]
    private static void ClearCompassHud(CompassHud __instance)
    {
        _compassHud = null;
    }
}