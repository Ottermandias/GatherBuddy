using System.Linq;
using System.Numerics;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Windowing;
using FFXIVClientStructs.FFXIV.Component.GUI;
using GatherBuddy.Classes;
using GatherBuddy.Config;
using GatherBuddy.Enums;
using GatherBuddy.SeFunctions;
using ImGuiNET;
using OtterGui;
using ImRaii = OtterGui.Raii.ImRaii;

namespace GatherBuddy.Spearfishing;

public partial class SpearfishingHelper : Window
{
    private const ImGuiWindowFlags WindowFlags = ImGuiWindowFlags.NoDecoration
      | ImGuiWindowFlags.NoInputs
      | ImGuiWindowFlags.AlwaysAutoResize
      | ImGuiWindowFlags.NoFocusOnAppearing
      | ImGuiWindowFlags.NoNavFocus
      | ImGuiWindowFlags.NoBackground;

    private        float            _uiScale       = 1;
    private        Vector2          _uiPos         = Vector2.Zero;
    private        Vector2          _uiSize        = Vector2.Zero;
    private unsafe SpearfishWindow* _addon         = null;
    private        Vector2          _listSizeText  = Vector2.Zero;
    private        Vector2          _listSizeIcons = Vector2.Zero;
    private const  float            _iconSize      = 30;

    private Vector2 ListSize
        => ImGuiHelpers.GlobalScale * (GatherBuddy.Config.ShowSpearfishListIconsAsText ? _listSizeText : _listSizeIcons);

    private unsafe void DrawFish(FishingSpot? spot, SpearfishWindow.Info info, AtkResNode* node, AtkResNode* fishLines, int idx)
    {
        if (!info.Available)
            return;

        var text = Identify(spot, info);
        var size = ImGui.CalcTextSize(text);
        var (x, y) = GatherBuddy.Config.FixNamesOnPosition
            ? (_uiSize.X * GatherBuddy.Config.FixNamesPercentage / 100,
                (fishLines->Y + fishLines->Height * idx / 7f) * _uiScale - ImGui.GetFrameHeight() / 2)
            : (node->X * _uiScale + node->Width * node->ScaleX * _uiScale / 2f - size.X / 2f,
                (node->Y + fishLines->Y + node->Height / 2f) * _uiScale - (size.Y + ImGui.GetStyle().FramePadding.Y) / 2f);
        ImGui.SetCursorPos(new Vector2(x, y));
        ImGuiUtil.DrawTextButton(text, Vector2.Zero, ColorId.SpearfishHelperBackgroundFish.Value(), ColorId.SpearfishHelperTextFish.Value());

        if (GatherBuddy.Config.ShowSpearfishSpeed)
        {
            ImGui.SameLine();
            ImGui.Text(info.Speed.ToName());
        }
    }

    private void DrawList()
    {
        if (!GatherBuddy.Config.ShowAvailableSpearfish || _currentSpot == null || _currentSpot.Items.Length == 0)
            return;

        ImGui.SetCursorPos(_uiSize * Vector2.UnitX);
        using var color = ImRaii.PushColor(ImGuiCol.ChildBg, ColorId.SpearfishHelperBackgroundList.Value());
        using var style = ImRaii.PushStyle(ImGuiStyleVar.ChildRounding, 5 * ImGuiHelpers.GlobalScale);
        using var child = ImRaii.Child("##ListChild", ListSize, true, ImGuiWindowFlags.NoScrollbar);
        if (!child)
            return;

        var iconSize = ImGuiHelpers.ScaledVector2(_iconSize, _iconSize);
        foreach (var fish in _currentSpot.Items)
        {
            var name = fish.Name[GatherBuddy.Language];
            if (GatherBuddy.Config.ShowSpearfishListIconsAsText)
            {
                name = $"{name} ({fish.Size.ToName()} and {fish.Speed.ToName()})";
            }
            else
            {
                if (Icons.FromSpeed(fish.Speed).TryGetWrap(out var speedWrap, out _))
                    ImGui.Image(speedWrap.ImGuiHandle, iconSize);
                else
                    ImGui.Dummy(iconSize);
                ImGui.SameLine();
                if (Icons.FromSize(fish.Size).TryGetWrap(out var sizeWrap, out _))
                    ImGui.Image(sizeWrap.ImGuiHandle, iconSize);
                else
                    ImGui.Dummy(iconSize);
                ImGui.SameLine();
            }

            if (Icons.DefaultStorage.TryLoadIcon(fish.ItemData.Icon, out var icon))
                ImGui.Image(icon.ImGuiHandle, iconSize);
            else
                ImGui.Dummy(iconSize);
            var pos = ImGui.GetCursorPos();
            ImGui.SameLine();
            ImGui.SetCursorPosY(ImGui.GetCursorPosY() + (iconSize.Y - ImGui.GetTextLineHeight()) / 2);
            ImGui.Text(name);
            ImGui.SetCursorPos(pos);
        }
    }

    private unsafe void DrawFishOverlay()
    {
        if (!GatherBuddy.Config.ShowSpearfishNames)
            return;

        DrawFish(_currentSpot, _addon->Fish1, _addon->Fish1Node, _addon->FishLines, 5);
        DrawFish(_currentSpot, _addon->Fish2, _addon->Fish2Node, _addon->FishLines, 3);
        DrawFish(_currentSpot, _addon->Fish3, _addon->Fish3Node, _addon->FishLines, 1);
    }

    private unsafe void DrawFishCenterLine()
    {
        if (!GatherBuddy.Config.ShowSpearfishCenterLine)
            return;

        var lineStart = _uiPos + new Vector2(_uiSize.X / 2, _addon->FishLines->Y * _uiScale);
        var lineEnd   = lineStart + new Vector2(0,          _addon->FishLines->Height * _uiScale);
        var list      = ImGui.GetWindowDrawList();
        list.AddLine(lineStart, lineEnd, ColorId.SpearfishHelperCenterLine.Value(), 3 * ImGuiHelpers.GlobalScale);
    }

    private void ComputeListSize()
    {
        if (_currentSpot == null)
        {
            _listSizeIcons = Vector2.Zero;
            _listSizeText  = Vector2.Zero;
            return;
        }

        var padding = ImGui.GetStyle().WindowPadding / ImGuiHelpers.GlobalScale;
        var spacing = ImGui.GetStyle().ItemSpacing / ImGuiHelpers.GlobalScale;
        var y       = padding.Y * 1.5f + (spacing.Y + _iconSize) * _currentSpot.Items.Length;
        var xIcons = padding.X * 2
          + (spacing.X + _iconSize) * 3
          + _currentSpot.Items.Max(i => ImGui.CalcTextSize(i.Name[GatherBuddy.Language]).X) / ImGuiHelpers.GlobalScale;
        var xText = padding.X * 2
          + spacing.X
          + _iconSize
          + _currentSpot.Items.Max(i => ImGui.CalcTextSize($"{i.Name[GatherBuddy.Language]} ({i.Size.ToName()} and {i.Speed.ToName()})").X)
          / ImGuiHelpers.GlobalScale;
        _listSizeIcons = new Vector2(xIcons, y);
        _listSizeText  = new Vector2(xText,  y);
    }

    public override unsafe bool DrawConditions()
    {
        var oldOpen = _isOpen;
        
        _addon  = (SpearfishWindow*)Dalamud.GameGui.GetAddonByName("SpearFishing", 1);
        _isOpen = _addon != null && _addon->Base.WindowNode != null;
        if (!_isOpen)
            return false;

        if (_isOpen != oldOpen)
        {
            _currentSpot = GetTargetFishingSpot();
            ComputeListSize();
        }

        var drawOverlay = GatherBuddy.Config.ShowSpearfishNames
         || GatherBuddy.Config.ShowSpearfishCenterLine
         || GatherBuddy.Config.ShowAvailableSpearfish && _currentSpot != null && _currentSpot.Items.Length > 0;
        return drawOverlay;
    }

    public override void PreOpenCheck()
    {
        IsOpen = GatherBuddy.Config.ShowSpearfishHelper;
    }

    public override unsafe void PreDraw()
    {
        _uiScale = _addon->Base.Scale;
        _uiPos   = new Vector2(_addon->Base.X, _addon->Base.Y);
        _uiSize = new Vector2(_addon->Base.WindowNode->AtkResNode.Width * _uiScale,
            _addon->Base.WindowNode->AtkResNode.Height * _uiScale);

        Position = _uiPos;
        SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = _uiSize,
            MaximumSize = Vector2.One * 10000,
        };
    }

    public override void Draw()
    {
        DrawFishOverlay();
        DrawFishCenterLine();
        DrawList();
    }
}
