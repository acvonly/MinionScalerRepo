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
            MinimumSize = new System.Numerics.Vector2(620, 320),
            MaximumSize = new System.Numerics.Vector2(840, 720),
        };
    }

    public void Dispose()
    {
    }

    public override void Draw()
    {
        var config = plugin.Configuration;

        ImGui.TextUnformatted("Visible minions");

        var visibleMinions = plugin.GetVisibleMinions();
        if (visibleMinions.Count == 0)
            ImGui.TextDisabled("No matching minions are currently visible.");

        foreach (var minion in visibleMinions)
        {
            ImGui.PushID($"visible-{minion.Key}");

            ImGui.TextUnformatted(minion.IsOwn ? $"{minion.Name} (Mine)" : minion.Name);
            DrawScaleControls(minion.Key, minion.Name, false);

            ImGui.PopID();
        }

        ImGui.Separator();
        ImGui.TextUnformatted("Saved minions");

        if (config.MinionScales.Count == 0)
            ImGui.TextDisabled("Saved minions will appear here.");

        foreach (var setting in config.MinionScales.Values.OrderBy(x => x.Name).ToArray())
        {
            ImGui.PushID($"saved-{setting.Key}");
            ImGui.TextUnformatted(setting.Name);
            DrawScaleControls(setting.Key, setting.Name, true);

            ImGui.PopID();
        }
    }

    private void DrawScaleControls(string key, string name, bool showDelete)
    {
        var scale = plugin.GetScaleForKey(key);
        ImGui.AlignTextToFramePadding();
        ImGui.TextUnformatted("Scale");
        ImGui.SameLine();
        ImGui.SetNextItemWidth(240);
        if (ImGui.SliderFloat("##scale-slider", ref scale, 0.1f, 10.0f, "%.2fx"))
        {
            plugin.SetPreviewScale(key, scale);
        }

        ImGui.SameLine();
        ImGui.SetNextItemWidth(90);
        if (ImGui.InputFloat("##scale-input", ref scale, 0.01f, 0.10f, "%.2f"))
        {
            plugin.SetPreviewScale(key, scale);
        }

        var applyToAll = plugin.GetApplyToAllForKey(key);
        ImGui.AlignTextToFramePadding();
        ImGui.TextUnformatted("Apply");
        ImGui.SameLine();
        if (ImGui.RadioButton("Mine only", !applyToAll))
        {
            plugin.SetPreviewApplyToAll(key, false);
        }

        ImGui.SameLine();
        if (ImGui.RadioButton("Everyone", applyToAll))
        {
            plugin.SetPreviewApplyToAll(key, true);
        }

        ImGui.SameLine();
        if (ImGui.Button("Save"))
        {
            plugin.SaveMinionScale(key, name);
        }

        ImGui.SameLine();
        if (ImGui.Button("Default"))
        {
            plugin.ResetMinionScale(key);
        }

        if (showDelete)
        {
            ImGui.SameLine();
            if (ImGui.Button("Delete"))
            {
                plugin.DeleteMinionScale(key);
            }
        }
    }
}
