using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

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

    [HarmonyPatch(typeof(ItemDrop), nameof(ItemDrop.Awake))]
    public static class ItemDropPatchAwake
    {
        private static readonly string TrophySubstring = "Trophy";

        private static readonly HashSet<string> AllowedExactPrefabs = new HashSet<string>
        {
            "Copper",
            "CopperOre",
            "IronNails",
            "IronScrap",
            "Iron",
            "IronOre",
            "BlackMetal",
            "BlackMetalScrap",
            "Silver",
            "SilverOre",
            "Tin",
            "TinOre",
        };

        private static readonly FieldInfo FloatingField = AccessTools.Field(typeof(ItemDrop), "m_floating");

        private static Floating _referenceFloating;

        [HarmonyPostfix]
        private static void Postfix(ItemDrop __instance)
        {
            if (__instance == null)
            {
                return;
            }

            if (__instance.m_itemData == null)
            {
                return;
            }

            if (__instance.m_itemData.m_dropPrefab == null)
            {
                return;
            }

            if (FloatingField == null)
            {
                return;
            }

            var existingFloating = FloatingField.GetValue(__instance) as Floating;
            if (existingFloating != null)
            {
                return;
            }

            string prefabName = __instance.m_itemData.m_dropPrefab.name;

            if (!ShouldFloat(prefabName))
            {
                return;
            }

            Floating floating = __instance.GetComponent<Floating>();
            if (floating == null)
            {
                floating = __instance.gameObject.AddComponent<Floating>();
            }

            // copy floating values from wood if possible, to avoid hardcoding values in the mod and to allow for better compatibility with other mods that may change floating values on wood
            Floating reference = GetWoodFLoatingReference();

            if (reference != null)
            {
                floating.m_waterLevelOffset = reference.m_waterLevelOffset;
                floating.m_force = reference.m_force;
                floating.m_forceDistance = reference.m_forceDistance;
                floating.m_balanceForceFraction = reference.m_balanceForceFraction;
                floating.m_damping = reference.m_damping;
            }

            FloatingField.SetValue(__instance, floating);
        }

        private static bool ShouldFloat(string prefabName)
        {
            if (string.IsNullOrWhiteSpace(prefabName))
            {
                return false;
            }

            if (AllowedExactPrefabs.Contains(prefabName))
            {
                return true;
            }

            if (prefabName.Contains(TrophySubstring))
            {
                return true;
            }

            return false;
        }

        private static Floating GetWoodFLoatingReference()
        {
            if (_referenceFloating != null)
            {
                return _referenceFloating;
            }

            if (ObjectDB.instance == null)
            {
                return null;
            }

            GameObject woodPrefab = ObjectDB.instance.GetItemPrefab("Wood");
            if (woodPrefab == null)
            {
                return null;
            }

            ItemDrop itemDrop = woodPrefab.GetComponent<ItemDrop>();
            if (itemDrop == null)
            {
                return null;
            }

            _referenceFloating = itemDrop.GetComponent<Floating>();

            return _referenceFloating;
        }
    }
}