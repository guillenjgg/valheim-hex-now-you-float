# NowYouFloat

Items that normally sink can now float. Follow the configuration instructions on how to add your own prefabs.

For example, say you wanted to make `Bloodgold` float. You would add the items in game prefab name `Gold` to the `AllowedExactPrefabs` configuration setting.

Note, the prefab name is not the same as the item name you may see in game. You can refer to the Valheim wiki to get most prefab names you need.

## Features

- Supports ores, metals, trophies, and other configurable prefabs
- Configure additional prefab names through the config file or the official BepInEx Configuration Manager

![Config](https://raw.githubusercontent.com/guillenjgg/valheim-hex-mod-images/main/hexnowyoufloat/nowyoufloat_1.png)

## Compatibility

- Adds a `Floating` component to any prefab that is configured in the BepInEx configuration.
- Mods that modify the same dropped item prefabs or Floating behavior may be incompatible.

## Configuration

Config file location:

```text
BepInEx/config/hex.nowyoufloat.cfg
```

Example configuration:

```ini
[General]

## Enable or disable the mod
# Setting type: Boolean
# Default value: true
IsModEnabled = true

[Prefabs]

## Exact prefab names that should float
# Comma-separated list
AllowedExactPrefabs = Copper,CopperOre,IronNails,BronzeNails,IronScrap,Iron,IronOre,BlackMetal,BlackMetalScrap,Silver,SilverOre,Tin,TinOre,SurtlingCore,DeerHide,CeramicPlate,Bronze

## Prefab name contains filters
# Any prefab containing these values will float
AllowedNameContains = Trophy
```

## Requirements

- BepInExPack

## Installation

### Thunderstore / r2modman

Install using a Thunderstore-compatible mod manager such as r2modman.

### Manual Installation

1. Install BepInExPack Valheim
2. Extract this package
3. Place the DLL inside:

```text
BepInEx/plugins/NowYouFloat/
```

Example:

```text
BepInEx/plugins/NowYouFloat/NowYouFloat.dll
```

## Support and Feedback
https://discord.gg/wU2FXD94v4

## Source Code
https://github.com/guillenjgg/valheim-hex-now-you-float
