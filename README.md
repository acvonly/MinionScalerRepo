# Minion Scaler

Dalamud plugin prototype that changes visible minion size locally.

## Install from a custom repository

After a release is published and GitHub Pages is enabled, add this URL in Dalamud:

```text
https://acvonly.github.io/MinionScalerRepo/repo.json
```

In-game:

1. Open Dalamud settings.
2. Open the Experimental tab.
3. Add the custom plugin repository URL above.
4. Open the plugin installer and install `Minion Scaler`.

## Use

- Command: `/minionscaler`
- Default behavior: only scales your own summoned minion.
- Disable the plugin or uncheck `Enable` to restore tracked minions to their original scale.

## Notes

This is a cosmetic client-side plugin prototype. It does not automate gameplay, send network requests, or collect player data. It writes to `GameObject.Scale`, so it may need adjustment when FFXIV or Dalamud changes object structures.
