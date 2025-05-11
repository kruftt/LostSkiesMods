using Bossa.Cinematika.Modules;
using HarmonyLib;
using WildSkies.Player;

namespace NoDeathbag
{
    public class NoDeathbagPatch
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(LocalPlayer), nameof(LocalPlayer.OnEntityDeath))]
        private static bool OnEntityDeath_SkipDeathbag(LocalPlayer __instance)
        {
            __instance._cameraManager.GetModule<CameraVisualSettings>().AddVisualAction(new BloomFlash(1f, 4f));
            __instance._cameraManager.GetModule<CameraVisualSettings>().AddVisualAction(__instance._deathCameraEffect);
            return false;
        }

        //static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        //{
        //    return new CodeMatcher(instructions)
        //        .MatchForward(false, new CodeMatch(OpCodes.Ldfld, AccessTools.Field(typeof(WildSkies.Player.LocalPlayer), "_playerInventoryService")))
        //        .MatchBack(false, new CodeMatch(OpCodes.Ldarg_0))
        //        .Insert(new CodeInstruction(OpCodes.Ret))
        //        .Instructions();
        //}
    }
}
