using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;

namespace NoDeathbag
{
    [BepInPlugin("org.kruft.plugins.NoDeathbag", "No Deathbag", "0.1.1")]
    public class NoDeathbagPlugin : BasePlugin
    {
        public override void Load()
        {
            Harmony harmony = new Harmony("kruft.NoDeathbag");
            harmony.PatchAll(typeof(NoDeathbagPatch));
        }
    }
}