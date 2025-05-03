using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;

namespace PointToInteract
{
    [BepInPlugin("org.kruft.plugins.PointToInteract", "Point To Interact", "0.2.2")]
    public class PointToInteractPlugin : BasePlugin
    {
        public override void Load()
        {
            Harmony harmony = new Harmony("kruft.PointToInteract");
            harmony.PatchAll(typeof(PointToInteractPatch));
        }
    }
}