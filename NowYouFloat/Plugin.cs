using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace NowYouFloat
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "hex.nowyoufloat";
        public const string PluginName = "Now You Float";
        public const string PluginVersion = "1.0.0";

        internal static Plugin Instance { get; private set; }
        internal static Harmony HarmonyInstance { get; private set; }
        internal static ManualLogSource Log { get; private set; }

        private void Awake()
        {
            Instance = this;
            Log = Logger;

            HarmonyInstance = new Harmony(PluginGuid);
            HarmonyInstance.PatchAll();

            Log.LogInfo($"[{PluginName}] loaded (v{PluginVersion}).");
        }

        private void OnDestroy()
        {
            Instance = null;
            HarmonyInstance?.UnpatchSelf();
            Log.LogInfo($"[{PluginName}] unloaded.");
        }
    }
}
