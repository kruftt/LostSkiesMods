using HarmonyLib;
using Bossa.Cinematika;
using Bossa.Cinematika.Controllers;
using UnityEngine;
using WildSkies.Input;
using WildSkies.Player;
using WildSkies.ShipParts;

namespace ShipCamPro
{
    class ShipCamProPatch
    {
        private static bool _isOnTurret = false;
        private static bool _thirdPerson = false;
        private static float _angleX = 0f;
        private static float _angleY = 0f;

        [HarmonyPostfix]
        [HarmonyPatch(typeof(LocalPlayer), nameof(LocalPlayer.DropInteraction))]
        static void ResetRemoteAiming(LocalPlayer __instance)
        {
            _isOnTurret = false;
            UserControlShip userControlShip = __instance._userControlShip;
            userControlShip._isRemoteAiming = false;
            ShipWeapon weapon = userControlShip._currentRemoteWeapon;
            if (weapon != null) weapon.SetPilot(null);
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(LocalPlayer), nameof(LocalPlayer.PilotTurret))]
        static void SetOnTurret(LocalPlayer __instance)
        {
            _isOnTurret = true;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(LocalPlayer), nameof(LocalPlayer.TakeControlOfShip))]
        static void SyncThirdPersonState(LocalPlayer __instance, Helm helm)
        {
            _isOnTurret = false;
            __instance._cameraManager.GetController<PilotCinematikaController>().ThirdPerson = _thirdPerson;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(PilotCinematikaController), nameof(PilotCinematikaController.ThirdPersonToggle))]
        static void ThirdPersonToggle(PilotCinematikaController __instance)
        {
            _thirdPerson = __instance.ThirdPerson;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(PilotCinematikaController), nameof(PilotCinematikaController.UpdateInput))]
        static void StoreTargetAngles(PilotCinematikaController __instance, CameraInputState iState)
        {
            if (_thirdPerson && !_isOnTurret && !iState.LookEnable)
            {
                _angleX = __instance._targetAngles.x;
                _angleY = __instance._targetAngles.y;
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(PilotCinematikaController), nameof(PilotCinematikaController.UpdateInput))]
        static void RestoreTargetAngles(PilotCinematikaController __instance, CameraInputState iState)
        {
            if (_thirdPerson && !_isOnTurret && !iState.LookEnable)
            {
                __instance._targetAngles = new Vector2(_angleX, _angleY);
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(PilotCinematikaController), nameof(PilotCinematikaController.UpdateTransform))]
        static void FixCamRotation(PilotCinematikaController __instance, Transform t)
        {
            if (_thirdPerson && !_isOnTurret)
            {
                t.rotation = Quaternion.LookRotation(__instance._manager.Target.position - t.position, Vector3.up);
            }
        }
    }
}