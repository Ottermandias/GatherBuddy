using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using Dalamud.Interface.Utility;
using GatherBuddy.Classes;
using GatherBuddy.Config;
using GatherBuddy.Enums;
using GatherBuddy.Interfaces;
using GatherBuddy.Time;
using ImGuiNET;
using OtterGui;
using ImRaii = OtterGui.Raii.ImRaii;
using Dalamud.Interface.Textures;

namespace GatherBuddy.Gui;

public partial class Interface
{
    public class ExtendedFish
    {
        public struct BaitOrder
        {
            public ISharedImmediateTexture  Icon;
            public string                   Name;
            public object?                  Fish;
            public ISharedImmediateTexture? HookSet;
            public (string, uint)           Bite;
        }

        public struct Predator
        {
            public Fish                    Fish;
            public ISharedImmediateTexture Icon;
            public string                  Name;
            public string                  Amount;
        }

        public static class Bites
        {
            public static readonly (string, uint) Weak      = ("  !  ", 0xFFFF6020);
            public static readonly (string, uint) Strong    = (" ! ! ", 0xFF60D030);
            public static readonly (string, uint) Legendary = (" !!! ", 0xFF0040C0);
            public static readonly (string, uint) Unknown   = (" ? ? ", 0xFF404040);

            public static (string, uint) FromBiteType(BiteType bite)
                => bite switch
                {
                    BiteType.Weak      => Weak,
                    BiteType.Strong    => Strong,
                    BiteType.Legendary => Legendary,
                    _                  => Unknown,
                };
        }

        public Fish                    Data;
        public ISharedImmediateTexture Icon;
        public string                  Territories;
        public string                  SpotNames;
        public string                  Aetherytes;

        public string                    Time;
        public ISharedImmediateTexture[] WeatherIcons;
        public ISharedImmediateTexture[] TransitionIcons;
        public BaitOrder[]               Bait;
        public ISharedImmediateTexture?  Snagging;
        public ISharedImmediateTexture?  Lure;
        public Predator[]                Predators;
        public string                    Patch;
        public string                    UptimeString;
        public string                    Intuition;
        public string                    FishType;
        public bool                      UptimeDependency;
        public ushort                    UptimePercent;
        public bool                      Unlocked = false;
        public bool                      Collectible;

        public (ILocation, TimeInterval) Uptime
            => GatherBuddy.UptimeManager.BestLocation(Data);

        private static ushort SetUptime(Fish fish)
        {
            var uptime = 10000L;
            if (!fish.Interval.AlwaysUp())
                uptime = uptime * fish.Interval.OnTime / EorzeaTimeStampExtensions.MillisecondsPerEorzeaHour / RealTime.HoursPerDay;
            ushort bestUptime = 0;
            foreach (var spot in fish.FishingSpots)
            {
                var tmp = uptime
                  * spot.Territory.WeatherRates.ChanceForWeather(fish.PreviousWeather)
                  * spot.Territory.WeatherRates.ChanceForWeather(fish.CurrentWeather)
                  / 10000;
                if (tmp > bestUptime)
                    bestUptime = (ushort)tmp;
            }

            return bestUptime;
        }

        private static ISharedImmediateTexture[] SetWeather(Fish fish)
        {
            if (!fish.FishRestrictions.HasFlag(FishRestrictions.Weather) || fish.CurrentWeather.Length == 0)
                return [];

            return fish.CurrentWeather.Select(w
                => Icons.DefaultStorage.TextureProvider.GetFromGameIcon(new GameIconLookup((uint)w.Data.Icon))).ToArray();
        }

        private static ISharedImmediateTexture[] SetTransition(Fish fish)
        {
            if (!fish.FishRestrictions.HasFlag(FishRestrictions.Weather) || fish.PreviousWeather.Length == 0)
                return [];

            return fish.PreviousWeather.Select(w
                => Icons.DefaultStorage.TextureProvider.GetFromGameIcon(new GameIconLookup((uint)w.Data.Icon))).ToArray();
        }

        private static Predator[] SetPredators(Fish fish)
        {
            if (fish.Predators.Length == 0)
                return [];

            return fish.Predators.Select(p => new Predator
            {
                Fish   = p.Item1,
                Amount = p.Item2.ToString(),
                Name   = p.Item1.Name[GatherBuddy.Language],
                Icon   = Icons.DefaultStorage.TextureProvider.GetFromGameIcon(new GameIconLookup(p.Item1.ItemData.Icon)),
            }).ToArray();
        }

        private static BaitOrder[] SetBait(Fish fish)
        {
            if (fish.IsSpearFish)
                return new BaitOrder[]
                {
                    new()
                    {
                        Name    = string.Intern($"{fish.Size.ToName()} and {fish.Speed.ToName()}"),
                        Fish    = null,
                        Icon    = Icons.FromSize(fish.Size),
                        Bite    = Bites.Unknown,
                        HookSet = null,
                    },
                };

            var ret  = new BaitOrder[fish.Mooches.Length + 1];
            var bait = fish.InitialBait;
            ret[0] = new BaitOrder()
            {
                Icon = Icons.DefaultStorage.TextureProvider.GetFromGameIcon(new GameIconLookup(bait.Data.Icon)),
                Name = bait.Name,
                Fish = bait,
            };
            for (var idx = 0; idx < fish.Mooches.Length; ++idx)
            {
                var f = fish.Mooches[idx];
                ret[idx].HookSet = Icons.FromHookSet(f.HookSet);
                ret[idx].Bite    = Bites.FromBiteType(f.BiteType);
                ret[idx + 1] = new BaitOrder()
                {
                    Icon = Icons.DefaultStorage.TextureProvider.GetFromGameIcon(new GameIconLookup(f.ItemData.Icon)),
                    Name = f.Name[GatherBuddy.Language],
                    Fish = f,
                };
            }

            ret[^1].HookSet = Icons.FromHookSet(fish.HookSet);
            ret[^1].Bite    = Bites.FromBiteType(fish.BiteType);
            return ret;
        }

        private static ISharedImmediateTexture? SetSnagging(Fish fish, IEnumerable<BaitOrder> baitOrder)
        {
            if (fish.Snagging == Enums.Snagging.Required)
                return Icons.Snagging;

            return baitOrder.Any(bait => bait.Fish is Fish { Snagging: Enums.Snagging.Required })
                ? Icons.Snagging
                : null;
        }

        private static ISharedImmediateTexture? SetLure(Fish fish, IEnumerable<BaitOrder> baitOrder)
        {
            var lure = fish.Lure;
            if (lure is Enums.Lure.None)
                lure = baitOrder.Select(b => b.Fish).OfType<Fish>().FirstOrDefault(f => f.Lure is not Enums.Lure.None)?.Lure ?? Enums.Lure.None;

            return lure switch
            {
                Enums.Lure.Ambitious => Icons.AmbitiousLure,
                Enums.Lure.Modest    => Icons.ModestLure,
                _                    => null,
            };
        }

        private static bool SetUptimeDependency(Fish fish, IEnumerable<BaitOrder> baitOrder)
        {
            foreach (var bait in baitOrder)
            {
                if (bait.Fish is not Fish f)
                    continue;

                if (CheckRestrictions(f))
                    return true;
            }

            return fish.Predators.Any(p => CheckRestrictions(p.Item1));

            bool CheckRestrictions(Fish f)
            {
                if (f.FishRestrictions.HasFlag(FishRestrictions.Time))
                {
                    if (!fish.FishRestrictions.HasFlag(FishRestrictions.Time))
                        return true;
                    if (!f.Interval.Contains(fish.Interval))
                        return true;
                }

                if (f.FishRestrictions.HasFlag(FishRestrictions.Weather))
                {
                    if (!fish.FishRestrictions.HasFlag(FishRestrictions.Weather))
                        return true;

                    if (f.CurrentWeather.Length > 0 && fish.CurrentWeather.Any(w => !f.CurrentWeather.Contains(w)))
                        return true;

                    if (f.PreviousWeather.Length > 0 && fish.PreviousWeather.Any(w => !f.PreviousWeather.Contains(w)))
                        return true;
                }

                return false;
            }
        }

        private static string SetIntuition(Fish data)
        {
            var intuition = data.IntuitionLength;
            if (intuition <= 0)
                return string.Empty;

            var minutes = intuition / RealTime.SecondsPerMinute;
            var seconds = intuition % RealTime.SecondsPerMinute;
            if (seconds == 0)
                return minutes == 1 ? "Intuition for 1 Minute" : string.Intern($"Intuition for {minutes} Minutes");

            return string.Intern($"Intuition for {minutes}:{seconds:D2} Minutes");
        }

        public ExtendedFish(Fish data)
        {
            Data        = data;
            Collectible = data.ItemData.IsCollectable;
            Icon        = Icons.DefaultStorage.TextureProvider.GetFromGameIcon(new GameIconLookup(data.ItemData.Icon));
            Territories = string.Join("\n", data.FishingSpots.Select(f => f.Territory.Name).Distinct());
            if (!Territories.Contains("\n"))
                Territories = '\0' + Territories;
            SpotNames = string.Join("\n", data.FishingSpots.Select(f => f.Name).Distinct());
            if (!SpotNames.Contains("\n"))
                SpotNames = '\0' + SpotNames;
            Aetherytes = string.Join("\n",
                data.FishingSpots.Where(f => f.ClosestAetheryte != null).Select(f => f.ClosestAetheryte!.Name).Distinct());
            if (!Aetherytes.Contains("\n"))
                Aetherytes = '\0' + Aetherytes;
            Patch = string.Intern($"Patch {data.Patch.ToVersionString()}");
            FishType = data.OceanFish ? "Ocean Fish" :
                data.IsSpearFish      ? "Spearfishing" :
                data.IsBigFish        ? "Big Fish" : "Regular Fish";

            Time = !data.FishRestrictions.HasFlag(FishRestrictions.Time)
                ? "Always Up"
                : data.OceanFish
                    ? PrintOceanTime(data.OceanTime)
                    : data.Interval.AlwaysUp()
                        ? "Unknown Uptime"
                        : string.Intern(data.Interval.PrintHours());

            UptimePercent = SetUptime(data);
            UptimeString  = string.Intern($"{(UptimePercent / 100f).ToString("F1", CultureInfo.InvariantCulture)}%");
            if (UptimeString == "0.0%")
                UptimeString = "<0.1%";
            WeatherIcons     = SetWeather(data);
            TransitionIcons  = SetTransition(data);
            Predators        = SetPredators(data);
            Bait             = SetBait(data);
            Snagging         = SetSnagging(data, Bait);
            Lure             = SetLure(data, Bait);
            UptimeDependency = SetUptimeDependency(data, Bait);
            Intuition        = SetIntuition(data);
            Unlocked         = GatherBuddy.FishLog.IsUnlocked(data);
        }

        private static string PrintOceanTime(OceanTime time)
        {
            return time switch
            {
                OceanTime.Sunset                   => "Sunset",
                OceanTime.Sunset | OceanTime.Night => "Sunset or Night",
                OceanTime.Sunset | OceanTime.Day   => "Sunset or Day",
                OceanTime.Night                    => "Night",
                OceanTime.Night | OceanTime.Day    => "Day or Night",
                OceanTime.Day                      => "Day",
                _                                  => "Unknown Uptime",
            };
        }

        private static void PrintTime(ExtendedFish fish)
            => ImGuiUtil.DrawTextButton(fish.Time, Vector2.Zero, ColorId.HeaderEorzeaTime.Value());

        private static void PrintWeather(ExtendedFish fish, Vector2 weatherIconSize)
        {
            if (!fish.Data.FishRestrictions.HasFlag(FishRestrictions.Weather))
            {
                ImGuiUtil.DrawTextButton("No Weather Restrictions", Vector2.Zero, ColorId.HeaderWeather.Value());
                return;
            }

            if (fish.WeatherIcons.Length == 0 && fish.TransitionIcons.Length == 0)
            {
                ImGuiUtil.DrawTextButton("Unknown Weather Restrictions", Vector2.Zero, ColorId.HeaderWeather.Value());
                return;
            }

            using var style = ImRaii.PushStyle(ImGuiStyleVar.ItemSpacing, ImGui.GetStyle().ItemSpacing / 2);
            if (fish.TransitionIcons.Length > 0)
            {
                AlignTextToSize(fish.TransitionIcons.Length > 1 ? "Requires one of" : "Requires", weatherIconSize);
                style.Push(ImGuiStyleVar.ItemSpacing, Vector2.One * ImGuiHelpers.GlobalScale);
                foreach (var w in fish.TransitionIcons)
                {
                    ImGui.SameLine();
                    if (w.TryGetWrap(out var wrap, out _))
                        ImGui.Image(wrap.ImGuiHandle, weatherIconSize);
                    else
                        ImGui.Dummy(weatherIconSize);
                }

                style.Pop();

                ImGui.SameLine();
                AlignTextToSize(fish.WeatherIcons.Length > 1 ? "followed by one of" : "followed by", weatherIconSize);
                if (fish.WeatherIcons.Length == 0)
                {
                    ImGui.SameLine();
                    AlignTextToSize(" Anything", weatherIconSize);
                }
                else
                {
                    style.Push(ImGuiStyleVar.ItemSpacing, Vector2.One * ImGuiHelpers.GlobalScale);
                    foreach (var w in fish.WeatherIcons)
                    {
                        ImGui.SameLine();
                        if (w.TryGetWrap(out var wrap, out _))
                            ImGui.Image(wrap.ImGuiHandle, weatherIconSize);
                        else
                            ImGui.Dummy(weatherIconSize);
                    }
                }
            }
            else if (fish.WeatherIcons.Length > 0)
            {
                AlignTextToSize(fish.WeatherIcons.Length > 1 ? "Requires one of" : "Requires", weatherIconSize);
                style.Push(ImGuiStyleVar.ItemSpacing, Vector2.One * ImGuiHelpers.GlobalScale);
                foreach (var w in fish.WeatherIcons)
                {
                    ImGui.SameLine();
                    if (w.TryGetWrap(out var wrap, out _))
                        ImGui.Image(wrap.ImGuiHandle, weatherIconSize);
                    else
                        ImGui.Dummy(weatherIconSize);
                }
            }
        }

        private static void PrintBait(ExtendedFish fish, Territory territory, Vector2 iconSize, Vector2 smallIconSize)
        {
            if (fish.Bait.Length == 0)
            {
                ImGuiUtil.DrawTextButton("Unknown Catch Method", Vector2.Zero, 0xFF0000A0);
                return;
            }

            using var style = ImRaii.PushStyle(ImGuiStyleVar.ItemSpacing, ImGui.GetStyle().ItemSpacing / 2);

            var startPos = ImGui.GetCursorPos();
            var size     = Vector2.Zero;
            if (fish.Snagging != null)
            {
                if (fish.Snagging.TryGetWrap(out var wrap, out _))
                    ImGui.Image(wrap.ImGuiHandle, iconSize);
                else
                    ImGui.Dummy(iconSize);
                ImGui.SameLine();
            }

            if (fish.Lure != null)
            {
                if (fish.Lure.TryGetWrap(out var wrap, out _))
                    ImGui.Image(wrap.ImGuiHandle, iconSize);
                else
                    ImGui.Dummy(iconSize);
                ImGui.SameLine();
            }

            var offsetSmall = (smallIconSize.Y - ImGui.GetTextLineHeight()) / 2;
            var offsetBig   = (iconSize.Y - ImGui.GetTextLineHeight()) / 2;
            foreach (var bait in fish.Bait)
            {
                size = iconSize;
                if (bait.Icon.TryGetWrap(out var wrap, out _))
                    ImGui.Image(wrap.ImGuiHandle, size);
                else
                    ImGui.Dummy(size);

                if (!fish.Data.IsSpearFish)
                {
                    style.Push(ImGuiStyleVar.ItemSpacing, Vector2.One);
                    ImGui.SameLine();
                    using var _ = ImRaii.Group();
                    style.Push(ImGuiStyleVar.FramePadding, Vector2.Zero);
                    if (bait.HookSet!.TryGetWrap(out wrap, out var _))
                        ImGui.Image(wrap.ImGuiHandle, smallIconSize);
                    else
                        ImGui.Dummy(smallIconSize);
                    using var color = ImRaii.PushColor(ImGuiCol.Button, bait.Bite.Item2);
                    ImGui.Button(bait.Bite.Item1, smallIconSize);
                    style.Pop(2);
                }

                ImGui.SameLine();

                var pos = ImGui.GetCursorPosY();

                var printed = false;
                if (bait.Fish is Fish f)
                {
                    var uptime = GatherBuddy.UptimeManager.NextUptime(f, territory, GatherBuddy.Time.ServerTime);
                    if (uptime != TimeInterval.Always)
                    {
                        using var group = ImRaii.Group();
                        ImGui.SetCursorPosY(pos + offsetSmall);
                        ImGui.TextUnformatted(bait.Name);
                        ImGui.SetCursorPosY(pos + smallIconSize.Y + offsetSmall);
                        DrawTimeInterval(uptime, false, false);
                        printed = true;
                    }
                }

                ImGui.SetCursorPosY(pos + offsetBig);
                if (!printed)
                    ImGui.TextUnformatted(bait.Name);
                if (bait.Equals(fish.Bait.Last()))
                    break;

                ImGui.SameLine();
                ImGui.SetCursorPosY(pos + offsetBig);
                ImGui.TextUnformatted(" → ");
                ImGui.SameLine();
            }

            ImGui.SetCursorPos(startPos + new Vector2(0, size.Y + ImGui.GetStyle().ItemSpacing.Y));
        }

        private static void PrintPredators(ExtendedFish fish, Territory territory, Vector2 iconSize)
        {
            if (fish.Predators.Length == 0 && fish.Intuition.Length == 0)
                return;

            using var color  = ImRaii.PushColor(ImGuiCol.Button, 0xFF0040C0);
            using var style  = ImRaii.PushStyle(ImGuiStyleVar.ItemSpacing, ImGui.GetStyle().ItemSpacing with { X = ImGuiHelpers.GlobalScale });
            var       size   = iconSize / 1.5f;
            var       offset = (size.Y - ImGui.GetTextLineHeightWithSpacing()) / 2f;
            foreach (var predator in fish.Predators)
            {
                var       pos   = ImGui.GetCursorPosY();
                using var group = ImRaii.Group();
                ImGui.Button(predator.Amount, size);
                ImGui.SameLine();
                if (predator.Icon.TryGetWrap(out var wrap, out _))
                    ImGui.Image(wrap.ImGuiHandle, size);
                else
                    ImGui.Dummy(size);
                style.Push(ImGuiStyleVar.ItemSpacing, new Vector2(3 * ImGuiHelpers.GlobalScale, 0));
                ImGui.SameLine();
                ImGui.SetCursorPosY(pos + offset);
                ImGui.TextUnformatted(predator.Name);
                var uptime = GatherBuddy.UptimeManager.NextUptime(predator.Fish, territory, GatherBuddy.Time.ServerTime);
                if (uptime != TimeInterval.Always)
                {
                    style.Push(ImGuiStyleVar.ItemSpacing, new Vector2(10 * ImGuiHelpers.GlobalScale, 0));
                    ImGui.SameLine();
                    ImGui.SetCursorPosY(pos + offset);
                    DrawTimeInterval(uptime, false, false);
                    style.Pop();
                }

                style.Pop();
            }


            if (fish.Intuition.Length == 0)
                return;

            color.Push(ImGuiCol.Button, 0xFF802000);
            ImGui.Button(fish.Intuition);
        }

        private static void PrintFolklore(ExtendedFish fish)
        {
            using var color = new ImRaii.Color();
            if (fish.Data.Folklore.Length != 0)
            {
                color.Push(ImGuiCol.Button, 0xFF802080);
                ImGui.Button(fish.Data.Folklore);
                color.Pop();
                ImGui.SameLine();
            }

            color.Push(ImGuiCol.Button, 0xFFC0C0C0)
                .Push(ImGuiCol.Text, 0xFF000000);
            ImGui.Button(fish.Patch);
        }

        public void SetTooltip(Territory territory, Vector2 iconSize, Vector2 smallIconSize, Vector2 weatherIconSize, bool standAlone = true)
        {
            using var tooltip = standAlone ? ImRaii.Tooltip() : ImRaii.IEndObject.Empty;
            using var style   = ImRaii.PushStyle(ImGuiStyleVar.ItemSpacing, ImGui.GetStyle().ItemSpacing * new Vector2(1f, 1.5f));
            PrintTime(this);
            PrintWeather(this, weatherIconSize);
            PrintBait(this, territory, iconSize, smallIconSize);
            PrintPredators(this, territory, iconSize);
            PrintFolklore(this);
        }
    }
}
