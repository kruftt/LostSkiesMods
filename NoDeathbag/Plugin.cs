using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using Bossa.Cinematika.Modules;

namespace NoDeathBag
{
    [BepInPlugin("org.kruft.plugins.deathbag", "No Deathbag", "0.1.1")]
    public class DeathbagPlugin : BasePlugin
    {
        public override void Load()
        {
            new Harmony("kruft.NoDeathbag").PatchAll();
        }
    }

    [HarmonyPatch(typeof(WildSkies.Player.LocalPlayer), "OnEntityDeath")]
    class DeathbagPatch
    {
        static bool Prefix(WildSkies.Player.LocalPlayer __instance)
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