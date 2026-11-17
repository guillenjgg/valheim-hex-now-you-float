using HarmonyLib;
using NowYouFloat;
using System.Reflection;
using UnityEngine;

[HarmonyPatch(typeof(ItemDrop), nameof(ItemDrop.Awake))]
internal static class ItemDropPatchAwake
{
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

        // Copy floating values from Wood when possible instead of hardcoding physics values.
        Floating reference = GetWoodFloatingReference();

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

        if (ConfigService.AllowedExactPrefabs != null && ConfigService.AllowedExactPrefabs.Contains(prefabName))
        {
            return true;
        }

        if (ConfigService.AllowedNameContains != null)
        {
            foreach (var allowedSubstring in ConfigService.AllowedNameContains)
            {
                if (prefabName.IndexOf(allowedSubstring, System.StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private static Floating GetWoodFloatingReference()
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