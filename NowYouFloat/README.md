# NowYouFloat

Makes supported dropped items float on water in Valheim.

## Features

- Supports ores, metals, trophies, and other configurable prefabs
- Configure additional prefab names through the config file or the official BepInEx Configuration Manager
- Simple quality-of-life mod

![Floating item example](images/float.png)

![Configuration example](images/config.png)

---

## Compatibility

- NowYouFloat adds a `Floating` component to configured dropped item prefabs that normally sink.
- Mods that modify the same dropped item prefabs or Floating behavior may be incompatible.

---

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
Enabled = true

## Exact prefab names that should float
# Comma-separated list
AllowedExactPrefabs = Copper,CopperOre,IronNails,BronzeNails,IronScrap,Iron,IronOre,BlackMetal,BlackMetalScrap,Silver,SilverOre,Tin,TinOre,SurtlingCore,DeerHide,CeramicPlate,Bronze

## Prefab name contains filters
# Any prefab containing these values will float
AllowedNameContains = Trophy
```

---

## Requirements

- denikson-BepInExPack_Valheim-5.4.2333

---

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

---

## Multiplayer

- Primarily tested on a dedicated server with the mod installed client-side only
- Multiplayer compatibility with other players and mod combinations has not been extensively tested