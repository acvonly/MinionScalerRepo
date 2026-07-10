using Dalamud.Interface.Windowing;
using Dalamud.Bindings.ImGui;

namespace MinionScaler;

public sealed class ConfigWindow : Window, IDisposable
{
    private readonly Plugin plugin;

    public ConfigWindow(Plugin plugin)
        : base($"{Plugin.DisplayName} v{Plugin.DisplayVersion}###MinionScalerConfig")
    {
        this.plugin = plugin;
        SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new System.Numerics.Vector2(500, 260),
            MaximumSize = new System.Numerics.Vector2(760, 620),
        };
    }

    public void Dispose()
    {
    }

    public override void Draw()
    {
        var config = plugin.Configuration;

        var ownOnly = config.OwnMinionOnly;
        if (ImGui.Checkbox("Only my minion", ref ownOnly))
        {
            config.OwnMinionOnly = ownOnly;
            plugin.Save();
        }

        ImGui.Separator();
        ImGui.TextUnformatted("Visible minions");

        var visibleMinions = plugin.GetVisibleMinions();
        if (visibleMinions.Count == 0)
            ImGui.TextDisabled("No matching minions are currently visible.");

        foreach (var minion in visibleMinions)
        {
            ImGui.PushID($"visible-{minion.Key}");

            ImGui.TextUnformatted(minion.Name);

            var scale = plugin.GetScaleForMinion(minion);
            ImGui.SetNextItemWidth(260);
            if (ImGui.SliderFloat("Scale", ref scale, 0.1f, 10.0f, "%.2fx"))
            {
                plugin.SetPreviewScale(minion, scale);
            }

            ImGui.SameLine();
            if (ImGui.Button("Save"))
            {
                plugin.SaveMinionScale(minion);
            }

            ImGui.PopID();
        }

        ImGui.Separator();
        ImGui.TextUnformatted("Saved minions");

        if (config.MinionScales.Count == 0)
            ImGui.TextDisabled("Saved minions will appear here.");

        foreach (var setting in config.MinionScales.Values.OrderBy(x => x.Name).ToArray())
        {
            ImGui.BulletText($"{setting.Name}: {setting.Scale:0.00}x");
        }
    }
}
