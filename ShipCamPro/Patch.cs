using HarmonyLib;
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

        [HarmonyPostfix]
        [HarmonyPatch(typeof(PilotCinematikaController), nameof(PilotCinematikaController.UpdateTransform))]
        static void FixCamRotation(PilotCinematikaController __instance, Transform t)
        {
            if (_thirdPerson && !_isOnTurret)
            {
                t.rotation = Quaternion.LookRotation(__instance._manager._target.position - t.position, new Vector3(0f, 1f, 0f));
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(LocalPlayer), nameof(LocalPlayer.DropInteraction))]
        static void ResetRemoteAiming(LocalPlayer __instance)
        {
            _isOnTurret = false;
            UserControlShip userControlShip = __instance._userControlShip;
            if (userControlShip._isRemoteAiming)
            {
                userControlShip._isRemoteAiming = false;
                userControlShip._currentRemoteWeapon.SetPilot(null);
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(LocalPlayer), nameof(LocalPlayer.PilotTurret))]
        static void SetIsOnTurret(LocalPlayer __instance)
        {
            _isOnTurret = true;
        }


        [HarmonyPostfix]
        [HarmonyPatch(typeof(LocalPlayer), nameof(LocalPlayer.TakeControlOfShip))]
        static void SetCameraState(LocalPlayer __instance, Helm helm)
        {
            _isOnTurret = false;
            if (_thirdPerson)
            {
                __instance._cameraManager.GetController<PilotCinematikaController>().ThirdPerson = true;
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(PilotCinematikaController), nameof(PilotCinematikaController.ThirdPersonToggle))]
        static void StoreCameraState(PilotCinematikaController __instance)
        {
            _thirdPerson = __instance.ThirdPerson;
        }
    }
}