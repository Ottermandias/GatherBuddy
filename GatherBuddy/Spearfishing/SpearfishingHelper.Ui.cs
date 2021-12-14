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
    private static unsafe void DrawFish(FishingSpot? spot, SpearfishWindow.Info info, AtkResNode* node, float yOffset)
    {
        if (!info.Available)
            return;

        var text = Identify(spot, info);
        var size = ImGui.CalcTextSize(text);
        var f1 = new Vector2(node->X + node->ScaleX * node->Width / 2f - size.X / 2,
            node->Y + yOffset + (node->ScaleY * node->Height - size.Y - ImGui.GetStyle().FramePadding.Y) / 2f);
        ImGui.SetCursorPos(f1);
        ImGuiUtil.DrawTextButton(text, Vector2.Zero, ColorId.SpearfishHelperBackgroundFish.Value(), ColorId.SpearfishHelperTextFish.Value());

        if (GatherBuddy.Config.ShowSpearfishSpeed)
        {
            ImGui.SameLine();
            ImGui.Text(info.Speed.ToName());
        }
    }

    private void DrawList(Vector2 pos, Vector2 size)
    {
        if (!GatherBuddy.Config.ShowAvailableSpearfish || _currentSpot == null || _currentSpot.Items.Length == 0)
            return;

        ImGuiHelpers.ForceNextWindowMainViewport();
        using var color = ImGuiRaii.PushColor(ImGuiCol.WindowBg, ColorId.SpearfishHelperBackgroundList.Value());
        ImGui.SetNextWindowPos(size * Vector2.UnitX + pos);
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

    private unsafe void DrawFishOverlay(SpearfishWindow* addon, Vector2 pos, Vector2 size)
    {
        if (!GatherBuddy.Config.ShowSpearfishNames && !GatherBuddy.Config.ShowSpearfishCenterLine)
            return;

        ImGui.SetNextWindowSize(size);
        ImGui.SetNextWindowPos(pos);
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
                DrawFish(_currentSpot, addon->Fish1, addon->Fish1Node, addon->FishLines->Y);
                DrawFish(_currentSpot, addon->Fish2, addon->Fish2Node, addon->FishLines->Y);
                DrawFish(_currentSpot, addon->Fish3, addon->Fish3Node, addon->FishLines->Y);
            }

            if (GatherBuddy.Config.ShowSpearfishCenterLine)
            {
                var lineStart = pos + new Vector2(size.X / 2, addon->FishLines->Y * addon->FishLines->ScaleY);
                var lineEnd   = lineStart + new Vector2(0,    addon->FishLines->ScaleY * addon->FishLines->Height);
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

        var pos = new Vector2(addon->Base.X, addon->Base.Y);
        var size = new Vector2(addon->Base.WindowNode->AtkResNode.Width * addon->Base.WindowNode->AtkResNode.ScaleX,
            addon->Base.WindowNode->AtkResNode.Height * addon->Base.WindowNode->AtkResNode.ScaleY);

        DrawFishOverlay(addon, pos, size);
        DrawList(pos, size);
    }
}
