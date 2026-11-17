using BepInEx.Configuration;
using System;
using System.Collections.Generic;

namespace NowYouFloat
{
    internal static class ConfigService
    {
        private static ConfigEntry<bool> _isModEnabled;
        private static ConfigEntry<string> _allowedExactPrefabsConfig;
        private static ConfigEntry<string> _allowedNameContainsConfig;

        internal static bool IsModEnabled => _isModEnabled.Value;
        internal static HashSet<string> AllowedExactPrefabs { get; private set; }
        internal static HashSet<string> AllowedNameContains { get; private set; }

        internal static void InitializeConfig(ConfigFile config)
        {
            _isModEnabled = config.Bind("General", "IsModEnabled", true, "Enable or disable the mod.");
            _allowedExactPrefabsConfig = config.Bind("Prefabs", "AllowedExactPrefabs", "Copper,CopperOre,IronNails,BronzeNails,IronScrap,Iron,IronOre,BlackMetal,BlackMetalScrap,Silver,SilverOre,Tin,TinOre,SurtlingCore,DeerHide,CeramicPlate,Bronze", "Comma-separated list of exact prefab names that are allowed to float.");
            _allowedNameContainsConfig = config.Bind("Prefabs", "AllowedNameContains", "Trophy", "Comma-separated list of substrings. If a prefab name contains any of these substrings, it is allowed to float.");
            
            ReloadPrefabConfig();

            _allowedExactPrefabsConfig.SettingChanged += OnPrefabConfigChanged;
            _allowedNameContainsConfig.SettingChanged += OnPrefabConfigChanged;
        }

        internal static void UnsubscribeConfigEvents()
        {
            _allowedExactPrefabsConfig.SettingChanged -= OnPrefabConfigChanged;
            _allowedNameContainsConfig.SettingChanged -= OnPrefabConfigChanged;
        }

        internal static void ReloadPrefabConfig()
        {
            AllowedExactPrefabs = ParseConfigList(_allowedExactPrefabsConfig.Value);
            AllowedNameContains = ParseConfigListIgnoreCase(_allowedNameContainsConfig.Value);
        }

        private static void OnPrefabConfigChanged(object sender, EventArgs e)
        {
            ReloadPrefabConfig();
        }

        private static HashSet<string> ParseConfigList(string value)
        {
            return ParseConfigList(value, null);
        }

        private static HashSet<string> ParseConfigListIgnoreCase(string value)
        {
            return ParseConfigList(value, System.StringComparer.OrdinalIgnoreCase);
        }

        private static HashSet<string> ParseConfigList(string value, IEqualityComparer<string> comparer)
        {
            var result = (comparer == null ? new HashSet<string>() : new HashSet<string>(comparer));

            if (string.IsNullOrWhiteSpace(value))
            {
                return result;
            }

            string[] values = value.Split(',');

            foreach (string item in values)
            {
                string trimmed = item.Trim();

                if (!string.IsNullOrWhiteSpace(trimmed))
                {
                    result.Add(trimmed);
                }
            }

            return result;
        }
    }
}
