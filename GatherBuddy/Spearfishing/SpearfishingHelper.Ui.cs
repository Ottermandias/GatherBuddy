using System.Numerics;
using Dalamud.Interface;
using FFXIVClientStructs.FFXIV.Component.GUI;
using GatherBuddy.Classes;
using GatherBuddy.Config;
using GatherBuddy.Enums;
using GatherBuddy.Gui;
using GatherBuddy.SeFunctions;
using ImGuiNET;
using ImGuiOtter;

namespace GatherBuddy.Spearfishing;

public partial class SpearfishingHelper
{
    private float   _uiScale = 1;
    private Vector2 _uiPos   = Vector2.Zero;
    private Vector2 _uiSize  = Vector2.Zero;

    private unsafe void DrawFish(FishingSpot? spot, SpearfishWindow.Info info, AtkResNode* node, AtkResNode* fishLines, int idx)
    {
        if (!info.Available)
            return;

        var text = Identify(spot, info);
        var size = ImGui.CalcTextSize(text);
        var (x, y) = GatherBuddy.Config.FixNamesOnPosition
            ? (_uiSize.X * GatherBuddy.Config.FixNamesPercentage / 100,
                fishLines->Y + fishLines->Height * _uiScale * idx / 7 - ImGui.GetFrameHeight() / 2)
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

        ImGuiHelpers.ForceNextWindowMainViewport();
        using var color = ImGuiRaii.PushColor(ImGuiCol.WindowBg, ColorId.SpearfishHelperBackgroundList.Value());
        ImGui.SetNextWindowPos(_uiSize * Vector2.UnitX + _uiPos);
        if (!ImGui.Begin("SpearfishingHelperList",
                ImGuiWindowFlags.NoDecoration | ImGuiWindowFlags.NoInputs | ImGuiWindowFlags.AlwaysAutoResize))
        {
            ImGui.End();
            return;
        }

        using var end      = ImGuiRaii.DeferredEnd(ImGui.End);
        var       iconSize = ImGuiHelpers.ScaledVector2(30, 30);
        foreach (var fish in _currentSpot.Items)
        {
            using var _    = ImGuiRaii.NewGroup();
            var       name = fish.Name[GatherBuddy.Language];
            if (GatherBuddy.Config.ShowSpearfishListIconsAsText)
            {
                name = $"{name} ({fish.Size.ToName()} and {fish.Speed.ToName()})";
            }
            else
            {
                ImGui.Image(IconId.FromSpeed(fish.Speed).ImGuiHandle, iconSize);
                ImGui.SameLine();
                ImGui.Image(IconId.FromSize(fish.Size).ImGuiHandle, iconSize);
                ImGui.SameLine();
            }

            ImGui.Image(Icons.DefaultStorage[fish.ItemData.Icon].ImGuiHandle, iconSize);
            ImGui.SameLine();
            ImGui.SetCursorPosY(ImGui.GetCursorPosY() + (iconSize.Y - ImGui.GetTextLineHeight()) / 2);
            ImGui.Text(name);
        }
    }

    private unsafe void DrawFishOverlay(SpearfishWindow* addon)
    {
        if (!GatherBuddy.Config.ShowSpearfishNames && !GatherBuddy.Config.ShowSpearfishCenterLine)
            return;

        ImGui.SetNextWindowSize(_uiSize);
        ImGui.SetNextWindowPos(_uiPos);
        ImGuiHelpers.ForceNextWindowMainViewport();
        if (!ImGui.Begin("SpearfishingHelper", ImGuiWindowFlags.NoBackground | ImGuiWindowFlags.NoDecoration | ImGuiWindowFlags.NoInputs))
        {
            ImGui.End();
        }
        else
        {
            using var end = ImGuiRaii.DeferredEnd(ImGui.End);
            if (GatherBuddy.Config.ShowSpearfishNames)
            {
                DrawFish(_currentSpot, addon->Fish1, addon->Fish1Node, addon->FishLines, 5);
                DrawFish(_currentSpot, addon->Fish2, addon->Fish2Node, addon->FishLines, 3);
                DrawFish(_currentSpot, addon->Fish3, addon->Fish3Node, addon->FishLines, 1);
            }

            if (GatherBuddy.Config.ShowSpearfishCenterLine)
            {
                var lineStart = _uiPos + new Vector2(_uiSize.X / 2, addon->FishLines->Y * _uiScale);
                var lineEnd   = lineStart + new Vector2(0,          addon->FishLines->Height * _uiScale);
                var list      = ImGui.GetWindowDrawList();
                list.AddLine(lineStart, lineEnd, ColorId.SpearfishHelperCenterLine.Value(), 3 * ImGuiHelpers.GlobalScale);
            }
        }
    }

    public unsafe void Draw()
    {
        if (!GatherBuddy.Config.ShowSpearfishHelper)
            return;

        var addon   = (SpearfishWindow*)Dalamud.GameGui.GetAddonByName("SpearFishing", 1);
        var oldOpen = _isOpen;
        _isOpen = addon != null && addon->Base.WindowNode != null;
        if (!_isOpen)
            return;

        if (_isOpen != oldOpen)
            _currentSpot = GetTargetFishingSpot();

        _uiScale = addon->Base.Scale;
        _uiPos   = new Vector2(addon->Base.X, addon->Base.Y);
        _uiSize = new Vector2(addon->Base.WindowNode->AtkResNode.Width * _uiScale,
            addon->Base.WindowNode->AtkResNode.Height * _uiScale);

        DrawFishOverlay(addon);
        DrawList();
    }
}
