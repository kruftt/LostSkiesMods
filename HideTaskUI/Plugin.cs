using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;

namespace HideTaskUI
{
    [BepInPlugin("org.kruft.plugins.HideTaskUI", "Hide Task UI", "0.1.0")]
    public class HideTaskUIPlugin : BasePlugin
    {
        public override void Load()
        {
            Harmony harmony = new Harmony("kruft.HideTaskUI");
            harmony.PatchAll(typeof(HideTaskUIPatch));
        }
    }
}