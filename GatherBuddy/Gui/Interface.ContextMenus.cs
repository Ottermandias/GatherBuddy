using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Dalamud.Game;
using Dalamud.Game.Text.SeStringHandling;
using GatherBuddy.Alarms;
using GatherBuddy.Classes;
using GatherBuddy.Config;
using GatherBuddy.GatherHelper;
using GatherBuddy.Interfaces;
using GatherBuddy.Plugin;
using GatherBuddy.Structs;
using ImGuiNET;
using ImRaii = OtterGui.Raii.ImRaii;

namespace GatherBuddy.Gui;

public partial class Interface
{
    private const string AutomaticallyGenerated = "Automatically generated from context menu.";

    private void DrawAddAlarm(IGatherable item)
    {
        // Only timed items.
        if (item.InternalLocationId <= 0)
            return;

        var current = _alarmCache.Selector.EnsureCurrent();
        if (ImGui.Selectable("Add to Alarm Preset"))
        {
            if (current == null)
            {
                _plugin.AlarmManager.AddGroup(new AlarmGroup()
                {
                    Description = AutomaticallyGenerated,
                    Enabled     = true,
                    Alarms      = new List<Alarm> { new(item) { Enabled = true } },
                });
                current = _alarmCache.Selector.EnsureCurrent();
            }
            else
            {
                _plugin.AlarmManager.AddAlarm(current, new Alarm(item));
            }
        }

        if (ImGui.IsItemHovered())
            ImGui.SetTooltip(
                $"Add {item.Name[GatherBuddy.Language]} to {(current == null ? "a new alarm preset." : CheckUnnamed(current.Name))}");
    }

    private void DrawAddToGatherGroup(IGatherable item)
    {
        var       current = _gatherGroupCache.Selector.EnsureCurrent();
        using var color   = ImRaii.PushColor(ImGuiCol.Text, ColorId.DisabledText.Value(), current == null);
        if (ImGui.Selectable("Add to Gather Group") && current != null)
            if (_plugin.GatherGroupManager.ChangeGroupNode(current, current.Nodes.Count, item, null, null, null, false))
                _plugin.GatherGroupManager.Save();

        color.Pop();

        if (ImGui.IsItemHovered())
            ImGui.SetTooltip(current == null
                ? "Requires a Gather Group to be setup and selected."
                : $"Add {item.Name[GatherBuddy.Language]} to {current.Name}");
    }

    private void DrawAddGatherWindow(Gatherable item)
    {
        var current = _gatherWindowCache.Selector.EnsureCurrent();

        if (ImGui.Selectable("Add to Current Auto-Gather Preset"))
        {
            if (current == null)
                CreateAndAddPreset(item);
            else
                _plugin.GatherWindowManager.AddItem(current, item);
        }

        if (ImGui.IsItemHovered())
            ImGui.SetTooltip(
                $"Add {item.Name[GatherBuddy.Language]} to {(current == null ? "a new gather window preset." : CheckUnnamed(current.Name))}");
    }

    private static string TeamCraftAddressEnd(string type, uint id)
    {
        var lang = GatherBuddy.Language switch
        {
            ClientLanguage.English  => "en",
            ClientLanguage.German   => "de",
            ClientLanguage.French   => "fr",
            ClientLanguage.Japanese => "ja",
            _                       => "en",
        };

        return $"db/{lang}/{type}/{id}";
    }

    private static string TeamCraftAddressEnd(FishingSpot s)
        => s.Spearfishing
            ? TeamCraftAddressEnd("spearfishing-spot", s.SpearfishingSpotData!.Value.GatheringPointBase.RowId)
            : TeamCraftAddressEnd("fishing-spot",      s.Id);

    private static string GarlandToolsItemAddress(uint itemId)
        => $"https://www.garlandtools.org/db/#item/{itemId}";

    private static void DrawOpenInGarlandTools(uint itemId)
    {
        if (itemId == 0)
            return;

        if (!ImGui.Selectable("Open in GarlandTools"))
            return;

        try
        {
            Process.Start(new ProcessStartInfo(GarlandToolsItemAddress(itemId)) { UseShellExecute = true });
        }
        catch (Exception e)
        {
            GatherBuddy.Log.Error($"Could not open GarlandTools:\n{e.Message}");
        }
    }

    private static void DrawOpenInTeamCraft(uint itemId)
    {
        if (itemId == 0)
            return;

        if (ImGui.Selectable("Open in TeamCraft (Browser)"))
            OpenInTeamCraftWeb(TeamCraftAddressEnd("item", itemId));

        if (ImGui.Selectable("Open in TeamCraft (App)"))
            OpenInTeamCraftLocal(TeamCraftAddressEnd("item", itemId));
    }

    private static void OpenInTeamCraftWeb(string addressEnd)
    {
        Process.Start(new ProcessStartInfo($"https://ffxivteamcraft.com/{addressEnd}")
        {
            UseShellExecute = true,
        });
    }

    private static void OpenInTeamCraftLocal(string addressEnd)
    {
        Task.Run(() =>
        {
            try
            {
                using var request  = new HttpRequestMessage(HttpMethod.Get, $"http://localhost:14500/{addressEnd}");
                using var response = GatherBuddy.HttpClient.Send(request);
            }
            catch
            {
                try
                {
                    if (Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ffxiv-teamcraft")))
                        Process.Start(new ProcessStartInfo($"teamcraft:///{addressEnd}")
                        {
                            UseShellExecute = true,
                        });
                }
                catch
                {
                    GatherBuddy.Log.Error("Could not open local teamcraft.");
                }
            }
        });
    }

    private static void DrawOpenInTeamCraft(FishingSpot fs)
    {
        if (fs.Id == 0)
            return;

        if (ImGui.Selectable("Open in TeamCraft (Browser)"))
            OpenInTeamCraftWeb(TeamCraftAddressEnd(fs));

        if (ImGui.Selectable("Open in TeamCraft (App)"))
            OpenInTeamCraftLocal(TeamCraftAddressEnd(fs));
    }

    public void CreateContextMenu(IGatherable item)
    {
        if (ImGui.IsItemClicked(ImGuiMouseButton.Right))
            ImGui.OpenPopup(item.Name[GatherBuddy.Language]);

        using var popup = ImRaii.Popup(item.Name[GatherBuddy.Language]);
        if (!popup)
            return;

        DrawAddAlarm(item);
        DrawAddToGatherGroup(item);
        if (item is Gatherable gatherable)
        {
            DrawAddGatherWindow(gatherable);
            DrawAddToAutoGather(gatherable);
        }
        if (ImGui.Selectable("Create Link"))
            Communicator.Print(SeString.CreateItemLink(item.ItemId));
        DrawOpenInGarlandTools(item.ItemId);
        DrawOpenInTeamCraft(item.ItemId);
    }

    private const string PresetName = "From Gatherables List";

    private static void DrawAddToAutoGather(Gatherable item)
    {
        if (ImGui.Selectable($"Add to Separate Auto-Gather Preset"))
        {
            // Fetch preset if exists.
            var preset = _plugin.GatherWindowManager.Presets.FirstOrDefault(p => p.Name == PresetName);

            if (preset == null)
                // Create and add preset if it doesn't exist.
                CreateAndAddPreset(item);
            else
                // Add item to existing preset.
                _plugin.GatherWindowManager.AddItem(preset, item);
        }

        if (ImGui.IsItemHovered())
            ImGui.SetTooltip(
                $"Add {item.Name[GatherBuddy.Language]} to {PresetName}");
    }

    private static GatherWindowPreset CreateAndAddPreset(Gatherable item)
    {
        var preset = new GatherWindowPreset
        {
            Enabled     = true,
            Description = AutomaticallyGenerated,
            Name        = PresetName
        };
        preset.Add(item);
        _plugin.GatherWindowManager.AddPreset(preset);

        return preset;
    }

    public static void CreateGatherWindowContextMenu(IGatherable item, bool clicked)
    {
        if (clicked)
            ImGui.OpenPopup(item.Name[GatherBuddy.Language]);

        using var popup = ImRaii.Popup(item.Name[GatherBuddy.Language]);
        if (!popup)
            return;

        if (ImGui.Selectable("Create Link"))
            Communicator.Print(SeString.CreateItemLink(item.ItemId));
        DrawOpenInGarlandTools(item.ItemId);
        DrawOpenInTeamCraft(item.ItemId);
    }

    public static void CreateContextMenu(Bait bait)
    {
        if (bait.Id == 0)
            return;

        if (ImGui.IsItemClicked(ImGuiMouseButton.Right))
            ImGui.OpenPopup(bait.Name);

        using var popup = ImRaii.Popup(bait.Name);
        if (!popup)
            return;

        if (ImGui.Selectable("Create Link"))
            Communicator.Print(SeString.CreateItemLink(bait.Id));
        DrawOpenInGarlandTools(bait.Id);
        DrawOpenInTeamCraft(bait.Id);
    }

    public static void CreateContextMenu(FishingSpot? spot)
    {
        if (spot == null)
            return;

        if (ImGui.IsItemClicked(ImGuiMouseButton.Right))
            ImGui.OpenPopup(spot.Name);

        using var popup = ImRaii.Popup(spot.Name);
        if (!popup)
            return;

        DrawOpenInTeamCraft(spot);
    }
}
