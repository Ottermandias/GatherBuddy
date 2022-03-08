using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using Dalamud.Game.ClientState.Keys;
using Dalamud.Interface;
using Dalamud.Interface.Windowing;
using GatherBuddy.Classes;
using GatherBuddy.Config;
using GatherBuddy.Gui;
using GatherBuddy.Interfaces;
using GatherBuddy.Plugin;
using GatherBuddy.Time;
using ImGuiNET;
using ImGuiOtter;

namespace GatherBuddy.GatherHelper;

public class GatherWindow : Window
{
    private readonly GatherBuddy _plugin;

    private int       _deleteSet              = -1;
    private int       _deleteItemIdx          = -1;
    private TimeStamp _earliestKeyboardToggle = TimeStamp.Epoch;

    public IGatherable? LastClick = null;

    public GatherWindow(GatherBuddy plugin)
        : base("##GatherHelper",
            ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoFocusOnAppearing | ImGuiWindowFlags.NoNavFocus,
            false)
    {
        _plugin            = plugin;
        IsOpen             = GatherBuddy.Config.ShowGatherWindow;
        RespectCloseHotkey = false;
        Namespace          = "GatherHelper";
        SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = Vector2.Zero,
            MaximumSize = Vector2.One * 10000,
        };
    }

    private static void DrawTime(ILocation? loc, TimeInterval time)
    {
        if (!GatherBuddy.Config.ShowGatherWindowTimers || !ImGui.TableNextColumn())
            return;
        if (time.Equals(TimeInterval.Always))
            return;

        if (time.Start > GatherBuddy.Time.ServerTime)
        {
            using var color = ImGuiRaii.PushColor(ImGuiCol.Text, ColorId.UpcomingItem.Value());
            ImGui.Text(TimeInterval.DurationString(time.Start, GatherBuddy.Time.ServerTime, false));
        }
        else
        {
            using var color = ImGuiRaii.PushColor(ImGuiCol.Text, ColorId.AvailableItem.Value());
            ImGui.Text(TimeInterval.DurationString(time.End, GatherBuddy.Time.ServerTime, false));
        }

        CreateTooltip(null, loc, time);
    }

    private static string TooltipText(ILocation? loc, TimeInterval time)
    {
        var sb = new StringBuilder();
        sb.Append(loc == null
            ? "Unknown Location\nUnknown Territory\nUnknown Aetheryte\n"
            : $"{loc.Name}\n{loc.Territory.Name}\n{loc.ClosestAetheryte?.Name ?? "No Aetheryte"}\n");

        sb.Append(time.Equals(TimeInterval.Always)
            ? "Always Up"
            : $"{time.Start}\n{time.End}\n{time.DurationString()}\n{TimeInterval.DurationString(time.Start > GatherBuddy.Time.ServerTime ? time.Start : time.End, GatherBuddy.Time.ServerTime, false)}");

        return sb.ToString();
    }

    private static void CreateTooltip(IGatherable? item, ILocation? loc, TimeInterval time)
    {
        if (!ImGui.IsItemHovered())
            return;

        if (item is not Fish fish)
        {
            ImGui.SetTooltip(TooltipText(loc, time));
            return;
        }

        var extendedFish = Interface.ExtendedFishList.FirstOrDefault(f => f.Data == fish);
        if (extendedFish == null)
        {
            ImGui.SetTooltip(TooltipText(loc, time));
            return;
        }

        using var tt = ImGuiRaii.NewTooltip();

        ImGui.Text(TooltipText(loc, time));
        ImGui.NewLine();
        extendedFish.SetTooltip(ImGuiHelpers.ScaledVector2(40, 40), ImGuiHelpers.ScaledVector2(20, 20), ImGuiHelpers.ScaledVector2(30, 30),
            false);
    }

    private void DrawItem(IGatherable item)
    {
        var (loc, time) = GatherBuddy.UptimeManager.BestLocation(item);
        if (GatherBuddy.Config.ShowGatherWindowOnlyAvailable && time.Start > GatherBuddy.Time.ServerTime)
            return;

        if (ImGui.TableNextColumn())
        {
            using var style = ImGuiRaii.PushStyle(ImGuiStyleVar.ItemSpacing, ImGui.GetStyle().ItemSpacing / 2);
            ImGuiUtil.HoverIcon(Icons.DefaultStorage[item.ItemData.Icon], Vector2.One * ImGui.GetTextLineHeight());
            ImGui.SameLine();
            var colorId = time == TimeInterval.Always    ? ColorId.GatherWindowText :
                time.Start > GatherBuddy.Time.ServerTime ? ColorId.GatherWindowUpcoming : ColorId.GatherWindowAvailable;
            using var color = ImGuiRaii.PushColor(ImGuiCol.Text, colorId.Value());
            if (ImGui.Selectable(item.Name[GatherBuddy.Language], false))
            {
                if (LastClick != item)
                    _plugin.Executor.GatherItem(item);
                else if (item is Gatherable)
                    _plugin.Executor.GatherItemByName("next");
                else
                    _plugin.Executor.GatherFishByName("next");
                LastClick = item;
            }

            var clicked = ImGui.IsItemClicked(ImGuiMouseButton.Right);
            color.Pop();
            CreateTooltip(item, loc, time);

            if (clicked && Functions.CheckModifier(GatherBuddy.Config.GatherWindowDeleteModifier, false))
                for (var i = 0; i < _plugin.GatherWindowManager.Presets.Count; ++i)
                {
                    var preset = _plugin.GatherWindowManager.Presets[i];
                    if (!preset.Enabled)
                        continue;

                    var idx = preset.Items.IndexOf(item);
                    if (idx < 0)
                        continue;

                    _deleteSet     = i;
                    _deleteItemIdx = idx;
                    break;
                }
        }

        DrawTime(loc, time);
    }

    private void DeleteItem()
    {
        if (_deleteSet < 0 || _deleteItemIdx < 0)
            return;

        var preset = _plugin.GatherWindowManager.Presets[_deleteSet];
        _plugin.GatherWindowManager.RemoveItem(preset, _deleteItemIdx);
        if (preset.Items.Count == 0)
            _plugin.GatherWindowManager.DeletePreset(_deleteSet);
        _deleteSet     = -1;
        _deleteItemIdx = -1;
    }

    private void CheckHotkeys()
    {
        if (_earliestKeyboardToggle > GatherBuddy.Time.ServerTime || !Functions.CheckKeyState(GatherBuddy.Config.GatherWindowHotkey, false))
            return;

        _earliestKeyboardToggle             = GatherBuddy.Time.ServerTime.AddMilliseconds(500);
        GatherBuddy.Config.ShowGatherWindow = !GatherBuddy.Config.ShowGatherWindow;
        GatherBuddy.Config.Save();
    }

    private static bool CheckHoldKey()
    {
        if (!GatherBuddy.Config.OnlyShowGatherWindowHoldingKey || GatherBuddy.Config.GatherWindowHoldKey == VirtualKey.NO_KEY)
            return false;

        return !Dalamud.Keys[GatherBuddy.Config.GatherWindowHoldKey];
    }

    private static bool CheckDuty()
        => GatherBuddy.Config.HideGatherWindowInDuty && Functions.BoundByDuty();

    public override void PreOpenCheck()
    {
        CheckHotkeys();
        IsOpen = GatherBuddy.Config.ShowGatherWindow;
    }

    public override bool DrawConditions()
        => !(CheckHoldKey() || CheckDuty() || _plugin.GatherWindowManager.ActiveItems.Count == 0);

    public override void PreDraw()
    {
        ImGui.PushStyleColor(ImGuiCol.WindowBg, ColorId.GatherWindowBackground.Value());
        ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, Vector2.One * 2 * ImGuiHelpers.GlobalScale);
        if (GatherBuddy.Config.LockGatherWindow)
            Flags |= ImGuiWindowFlags.NoMove;
        else
            Flags &= ~
                ImGuiWindowFlags.NoMove;
    }

    public override void PostDraw()
    {
        DeleteItem();
        ImGui.PopStyleVar();
        ImGui.PopStyleColor();
    }

    public override void Draw()
    {
        using var end = ImGuiRaii.DeferredEnd(ImGui.End);
        if (!ImGui.BeginTable("##table", GatherBuddy.Config.ShowGatherWindowTimers ? 2 : 1))
            return;

        end.Push(ImGui.EndTable);

        foreach (var item in _plugin.GatherWindowManager.GetList())
            DrawItem(item);
    }
}
