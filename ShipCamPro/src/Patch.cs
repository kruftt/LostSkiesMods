using Bossa.Cinematika;
using Bossa.Cinematika.Controllers;
using UnityEngine;
using UnityEngine.InputSystem;
using WildSkies.Input;
using WildSkies.Player;
using WildSkies.ShipParts;
namespace ShipCamPro;

class ShipCamProPatch
{
    private const float SCROLL_SENSITIVITY = 0.1f;
    private const float MIN_ZOOM_DISTANCE = 2.0f;
    private static bool _isOnTurret = false;
    private static bool _isLooking = false;
    private static bool _hasTurned = false;
    private static bool _releaseCamera = false;
    private static bool _thirdPerson = false;
    private static float _zoom = 5.0f;
    private static float _angleX = 0f;
    private static float _angleY = 0f;
    private static Vector3 OFFSET_BASE = new Vector3(0, 0, -1.0f);
    private static Vector3 _thirdPersonOffset
    {
        get => OFFSET_BASE * _zoom;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(LocalPlayer), nameof(LocalPlayer.DropInteraction))]
    private static void ResetRemoteAiming(LocalPlayer __instance)
    {
        _isOnTurret = false;
        UserControlShip userControlShip = __instance._userControlShip;
        userControlShip._isRemoteAiming = false;
        ShipWeapon weapon = userControlShip._currentRemoteWeapon;
        if (weapon != null) weapon.SetPilot(null);
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(LocalPlayer), nameof(LocalPlayer.PilotTurret))]
    private static void IsOnTurret(LocalPlayer __instance)
    {
        _isOnTurret = true;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(LocalPlayer), nameof(LocalPlayer.TakeControlOfShip))]
    private static void ApplyThirdPersonState(LocalPlayer __instance, Helm helm)
    {
        _isOnTurret = false;
        var _cinematikaController = __instance._cameraManager.GetController<PilotCinematikaController>();
        
        if (_thirdPerson && !_cinematikaController.ThirdPerson)
        {
            _cinematikaController.ThirdPersonToggle();
        }

        var settings = _cinematikaController.CurrentSettings;
        var offset = _thirdPersonOffset;
        if (ShipCamProPlugin.cameraWrap.Value) 
            settings._xWrap = true;
        settings._thirdPersonOffset = offset;
        settings._thirdPersonOffsetLowY = offset;
        settings._thirdPersonOffsetHighY = offset;
        settings._maxAngles.y = 89f;
        settings._minAngles.y = -89f;
        _cinematikaController.CurrentSettings = settings;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(PilotCinematikaController), nameof(PilotCinematikaController.ThirdPersonToggle))]
    private static void ThirdPersonToggle(PilotCinematikaController __instance)
    {
        _thirdPerson = __instance.ThirdPerson;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(PilotCinematikaController), nameof(PilotCinematikaController.UpdateInput))]
    private static void StoreTargetAngles(PilotCinematikaController __instance, CameraInputState iState)
    {
        _angleX = __instance._targetAngles.x;
        _angleY = __instance._targetAngles.y;

        var scroll = Mouse.current.scroll.ReadValue().y;
        if (scroll != 0)
        {
            _zoom = Mathf.Clamp(_zoom - (_zoom * scroll * SCROLL_SENSITIVITY), MIN_ZOOM_DISTANCE, ShipCamProPlugin.maxZoom.Value);

            var offset = _thirdPersonOffset;
            var settings = __instance.CurrentSettings;
            settings._thirdPersonOffset = offset;
            settings._thirdPersonOffsetLowY = offset;
            settings._thirdPersonOffsetHighY = offset;
            __instance.CurrentSettings = settings;
        }

        if (iState.LookEnable)
        {
            if (!_isLooking)
            {
                _isLooking = true;
                _hasTurned = false;
                _releaseCamera = false;
            } else if (iState.Look != Vector2.zero)
            {
                _hasTurned = true;
            }
        }
        else if (_isLooking)
        {
            if (!_hasTurned)
            {
                _releaseCamera = true;
            }
            _hasTurned = false;
            _isLooking = false;
        }
    }

    [HarmonyPostfix]
    [HarmonyPriority(HarmonyLib.Priority.VeryLow)]
    [HarmonyPatch(typeof(PilotCinematikaController), nameof(PilotCinematikaController.UpdateInput))]
    private static void RestoreTargetAngles(PilotCinematikaController __instance, CameraInputState iState)
    {
        if (_thirdPerson && !_isOnTurret && !iState.LookEnable && ShipCamProPlugin.cameraLock.Value && !_releaseCamera)
        {
            __instance._targetAngles = new Vector2(_angleX, _angleY);
        }
    }
}