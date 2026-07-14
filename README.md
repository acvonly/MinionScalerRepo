# Minion Scaler

Minion Scaler is a Dalamud plugin that changes the displayed size of FFXIV minions on your local game client.

> [!CAUTION]
> Minion Scaler is an early beta and remains under active development. Every reasonable precaution is taken to prevent crashes, incorrect minion states, restoration failures, and loss or corruption of local plugin configuration data, but the absence of these problems cannot be guaranteed. No warranty is provided for crashes, data loss, damage, or any other result caused by using this plugin. Use it only if you understand and accept these risks.

## Features

- Lists minions currently within targetable range in the `Visible` tab.
- Changes scale from `0.10x` to `10.00x` with a slider or numeric input.
- Applies settings to `Everyone` or `Mine only`.
- Pins per-minion settings and reapplies them when matching minions appear.
- Filters the `Visible` and `Pinned` tabs by minion name.
- Targets the nearest matching minion when its icon is clicked.
- Reapplies saved settings after territory changes and in GPose.
- Resets or removes individual and all pinned settings.

Changes are local only. Minion Scaler does not modify server-side minion data or how minions appear to other players.

## Installation

Add the following URL to `Custom Plugin Repositories` under Dalamud Settings > `Experimental`:

```text
https://raw.githubusercontent.com/miqote69/MinionScalerRepo/main/repo.json
```

After saving the repository, open the Dalamud Plugin Installer, search for `Minion Scaler`, and install it.

## Basic Usage

1. Summon a minion or move close to a visible minion.
2. Open the plugin with `/minionscaler`.
3. Set the scale and application scope in the `Visible` tab.
4. Click the pin button to save the setting in the `Pinned` tab.

`Everyone` applies the scale to every visible instance of the same minion type. `Mine only` applies it only to the minion identified as belonging to the local player. New settings default to `Everyone`.

## Commands

```text
/minionscaler
/minionscale
/minionscalerconfig
```

All commands open the same configuration window.

## Languages

The UI supports:

- English
- Japanese
- German
- French

`Automatic` follows the FFXIV client language. Minion names are read separately from FFXIV game data.

Korean and Chinese FFXIV client versions have not been tested because no compatible test environment is available. Whether the plugin works correctly on those clients is unknown. Korean and Chinese UI translations are not currently included.

## Safety and Scope

- Minion Scaler modifies local `GameObject` and `DrawObject` scale values.
- It records the original values before applying a multiplier and attempts to restore them when settings are reset or removed.
- Complete restoration cannot be guaranteed after objects are recreated, the game is updated, or another plugin changes the same values.
- FFXIV, Dalamud, or FFXIVClientStructs updates may temporarily break the plugin.
- The plugin does not automate gameplay, send gameplay commands, upload minion information, or collect player data.

More detailed instructions, troubleshooting, and answers are available in the [Wiki](https://github.com/miqote69/MinionScalerRepo/wiki).

## License

Minion Scaler is licensed under the [MIT License](LICENSE).
