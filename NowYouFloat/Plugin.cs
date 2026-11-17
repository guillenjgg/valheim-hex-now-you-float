using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.Reflection;

namespace NowYouFloat
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "hex.nowyoufloat";
        public const string PluginName = "NowYouFloat";
        public const string PluginVersion = "1.0.3";

        internal static Plugin Instance { get; private set; }
        internal static Harmony HarmonyInstance { get; private set; }
        internal static ManualLogSource Log { get; private set; }

        private void Awake()
        {
            Instance = this;
            Log = Logger;

            ConfigService.InitializeConfig(Config);

            HarmonyInstance = new Harmony(PluginGuid);
            HarmonyInstance.PatchAll(Assembly.GetExecutingAssembly());

            Log.LogInfo($"{PluginName} v{PluginVersion} loaded.");
        }

        private void OnDestroy()
        {
            Log.LogInfo($"{PluginName} v{PluginVersion} unloaded.");

            ConfigService.UnsubscribeConfigEvents();

            HarmonyInstance?.UnpatchSelf();
            HarmonyInstance = null;
            Instance = null;
        }
    }
}