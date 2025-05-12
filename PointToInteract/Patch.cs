using Bossa.Cinematika;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using UnityEngine;
using WildSkies.Service;
namespace PointToInteract;

class PointToInteractPatch
{
    private static CameraManager _cameraManager;
    private static int[] _colliderIDs = new int[10];

    [HarmonyPrefix]
    [HarmonyPatch(typeof(CharacterInteract), nameof(CharacterInteract.UpdateInteractionDetectionList))]
    private static void FilterNearbyInteractables(CharacterInteract __instance, ref int resultCount)
    {
        if (resultCount > 1)
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

            if (ray_hits > 0)
            {
                int hits = 0;

                for (int i = 0; i < ray_hits; i++)
                {
                    _colliderIDs[i] = _occlusionCheckBuffer[i].colliderInstanceID;
                }

                for (int i = 0; i < resultCount && ray_hits > 0; i++)
                {
                    Collider collider = _interactablesNearby[i];
                    int colliderID = collider.GetInstanceID();

                    for (int j = 0; j < ray_hits; j++)
                    {
                        if (_colliderIDs[j] == colliderID)
                        {
                            _interactablesNearby[hits] = collider;
                            hits++;

                            ray_hits--;
                            _colliderIDs[j] = _colliderIDs[ray_hits];

                            break;
                        }
                    }
                }

                if (hits > 0) resultCount = hits;
            }
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(LocalPlayerService), nameof(LocalPlayerService.RegisterLocalPlayer))]
    private static void SetCamera(LocalPlayerService __instance)
    {
        _cameraManager = __instance.LocalPlayer.CameraManager;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(LocalPlayerService), nameof(LocalPlayerService.UnregisterPlayer))]
    private static void UnsetCamera(LocalPlayerService __instance)
    {
        _cameraManager = null;
    }
}