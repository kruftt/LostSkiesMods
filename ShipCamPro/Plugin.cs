using BepInEx.Unity.IL2CPP;
using BepInEx;
using HarmonyLib;

namespace ShipCamPro
{
    [BepInPlugin("org.kruft.plugins.ShipCamPro", "Ship Cam Pro", "0.1.0")]
    public class ShipCamProPlugin : BasePlugin
    {
        public override void Load()
        {
            Harmony harmony = new Harmony("kruft.ShipCamPro");
            harmony.PatchAll(typeof(ShipCamProPatch));
        }
    }
}