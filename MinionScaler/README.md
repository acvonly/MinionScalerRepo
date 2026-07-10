# Minion Scaler

Dalamud plugin prototype that changes visible minion size locally.

## Use

- Command: `/minionscaler`
- Alias: `/minionscale`
- Settings command: `/minionscalerconfig`
- Default behavior: only scales your own summoned minion.
- Disable the plugin or uncheck `Enable` to restore tracked minions to their original scale.

## Notes

This is a cosmetic client-side plugin prototype. It does not automate gameplay, send network requests, or collect player data. It writes to `GameObject.Scale`, so it may need adjustment when FFXIV or Dalamud changes object structures.
