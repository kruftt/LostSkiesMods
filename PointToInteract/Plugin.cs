using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using UnityEngine;
using WildSkies.Service;
using Bossa.Cinematika;
using Il2CppInterop.Runtime.InteropTypes.Arrays;

namespace PointToInteract
{
    [BepInPlugin("org.kruft.plugins.PointToInteract", "Point To Interact", "0.1.2")]
    public class PointToInteractPlugin : BasePlugin
    {
        private static CameraManager _cameraManager;

        public override void Load()
        {
            new Harmony("kruft.PointToInteract").PatchAll(typeof(PointToInteractPlugin));
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(CharacterInteract), nameof(CharacterInteract.UpdateInteractionDetectionList))]
        static void InteractionListPrefix(CharacterInteract __instance, ref int resultCount)
        {
            if (resultCount > 0)
            {
                Transform cameraTransform = _cameraManager.Camera.gameObject.transform;
                Il2CppStructArray<RaycastHit> _occlusionCheckBuffer = __instance._occlusionCheckBuffer;
                Il2CppReferenceArray<Collider> _interactablesNearby = __instance._interactablesNearby;

                int ray_hits = Physics.RaycastNonAlloc(
                    new Ray(cameraTransform.position, cameraTransform.forward),
                    _occlusionCheckBuffer,
                    __instance._interactRadius + Vector3.Distance(cameraTransform.position, __instance._character.WorldCenterOfMass),
                    __instance._interactableLayerMask,
                    QueryTriggerInteraction.Collide
                );

                int i, j;
                int[] colliderIDs = new int[ray_hits];

                for (i = 0; i < ray_hits; i++)
                {
                    colliderIDs[i] = _occlusionCheckBuffer[i].colliderInstanceID;
                }

                int hits = 0;
                for (i = 0; i < resultCount; i++) {
                    Collider collider = _interactablesNearby[i];
                    int colliderID = collider.GetInstanceID();
                    
                    for (j = 0; j < ray_hits; j++ ) {
                        if (colliderIDs[j] == colliderID)
                        {
                            _interactablesNearby[hits] = collider;
                            hits++;
                            break;
                        }
                    }
                }

                resultCount = hits;
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(LocalPlayerService), nameof(LocalPlayerService.RegisterLocalPlayer))]
        static void SetCameraPostfix(LocalPlayerService __instance)
        {
            _cameraManager = __instance.LocalPlayer.CameraManager;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(LocalPlayerService), nameof(LocalPlayerService.UnregisterPlayer))]
        static void UnsetCameraPostfix(LocalPlayerService __instance)
        {
            _cameraManager = null;
        }
    }
}