using Dalamud.Interface.Windowing;
using Dalamud.Bindings.ImGui;
using System.Numerics;

namespace MinionScaler;

public sealed class ConfigWindow : Window, IDisposable
{
    private static readonly Vector2 IconSize = new(24.0f, 24.0f);

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

        ImGui.Columns(2, "##minion-columns", true);

        ImGui.TextUnformatted("Visible minions");

        var visibleMinions = plugin.GetVisibleMinions()
            .Where(minion => !config.MinionScales.ContainsKey(minion.Key))
            .ToArray();
        if (visibleMinions.Length == 0)
            ImGui.TextDisabled("No unpinned minions are currently visible.");

        foreach (var minion in visibleMinions)
        {
            ImGui.PushID($"visible-{minion.Key}");

            DrawMinionLabel(minion.Name, minion.IsOwn, minion.IconId);
            DrawScaleControls(minion.Key, minion.Name, minion.IconId, false);
            ImGui.Separator();

            ImGui.PopID();
        }

        ImGui.NextColumn();
        ImGui.TextUnformatted("Pinned minions");

        if (config.MinionScales.Count == 0)
            ImGui.TextDisabled("Pinned minions will appear here.");

        foreach (var setting in config.MinionScales.Values.OrderBy(x => x.Name).ToArray())
        {
            ImGui.PushID($"pinned-{setting.Key}");
            var iconId = setting.IconId != 0 ? setting.IconId : plugin.GetIconIdForKey(setting.Key);
            DrawMinionLabel(setting.Name, false, iconId);
            DrawScaleControls(setting.Key, setting.Name, iconId, true);
            ImGui.Separator();

            ImGui.PopID();
        }

        ImGui.Columns(1);
    }

    private void DrawMinionLabel(string name, bool isOwn, uint iconId)
    {
        if (plugin.TryGetIconTexture(iconId, out var icon))
        {
            ImGui.Image(icon.Handle, IconSize);
            ImGui.SameLine();
        }

        ImGui.AlignTextToFramePadding();
        ImGui.TextUnformatted(isOwn ? $"{name} (Mine)" : name);
    }

    private void DrawScaleControls(string key, string name, uint iconId, bool isSaved)
    {
        var scale = plugin.GetScaleForKey(key);
        var availableWidth = ImGui.GetContentRegionAvail().X;
        var sliderWidth = Math.Max(120.0f, availableWidth - 100.0f);

        ImGui.AlignTextToFramePadding();
        ImGui.TextUnformatted("Scale");
        ImGui.SameLine();
        ImGui.SetNextItemWidth(sliderWidth);
        if (ImGui.SliderFloat("##scale-slider", ref scale, 0.1f, 10.0f, "%.2fx"))
        {
            if (isSaved)
                plugin.UpdateSavedMinionScale(key, scale);
            else
                plugin.SetPreviewScale(key, scale);
        }

        ImGui.SameLine();
        ImGui.SetNextItemWidth(90);
        if (ImGui.InputFloat("##scale-input", ref scale, 0.01f, 0.10f, "%.2f"))
        {
            if (isSaved)
                plugin.UpdateSavedMinionScale(key, scale);
            else
                plugin.SetPreviewScale(key, scale);
        }

        var applyToAll = plugin.GetApplyToAllForKey(key);
        ImGui.AlignTextToFramePadding();
        if (ImGui.RadioButton("Everyone", applyToAll))
        {
            if (isSaved)
                plugin.UpdateSavedApplyToAll(key, true);
            else
                plugin.SetPreviewApplyToAll(key, true);
        }

        ImGui.SameLine();
        if (ImGui.RadioButton("Mine only", !applyToAll))
        {
            if (isSaved)
                plugin.UpdateSavedApplyToAll(key, false);
            else
                plugin.SetPreviewApplyToAll(key, false);
        }

        if (!isSaved)
        {
            ImGui.SameLine();
            if (ImGui.Button("Pin"))
            {
                plugin.SaveMinionScale(key, name, iconId);
            }
        }

        ImGui.SameLine();
        if (ImGui.Button("Default"))
        {
            if (isSaved)
                plugin.ResetSavedMinionScale(key);
            else
                plugin.ResetMinionScale(key);
        }

        if (isSaved)
        {
            ImGui.SameLine();
            if (ImGui.Button("Delete"))
            {
                plugin.DeleteMinionScale(key);
            }
        }
    }
}
