using System.Linq;
using System.Numerics;
using GatherBuddy.Classes;
using GatherBuddy.Enums;
using GatherBuddy.Game;
using ImGuiNET;
using ImGuiScene;

namespace GatherBuddy.Gui
{
    public partial class Interface
    {
        private class CachedFish
        {
            public struct BaitOrder
            {
                public TextureWrap    Icon    { get; set; }
                public string         Name    { get; set; }
                public Fish?          Fish    { get; set; }
                public TextureWrap?   HookSet { get; set; }
                public (string, uint) Bite    { get; set; }
            }

            public struct Predator
            {
                public TextureWrap Icon   { get; set; }
                public string      Name   { get; set; }
                public string      Amount { get; set; }
            }

            public Fish            Fish             { get; }
            public TextureWrap     Icon             { get; }
            public string          Name             { get; }
            public string          NameLower        { get; }
            public string          Time             { get; }
            public TextureWrap[][] WeatherIcons     { get; }
            public BaitOrder[]     Bait             { get; }
            public string          Territory        { get; }
            public string          TerritoryLower   { get; }
            public string          FishingSpot      { get; }
            public string          FishingSpotLower { get; }
            public TextureWrap?    Snagging         { get; }
            public Predator[]      Predators        { get; }
            public string          Patch            { get; }

            public void PrintTime()
            {
                ImGui.PushStyleColor(ImGuiCol.Button,        0xFF008080);
                ImGui.PushStyleColor(ImGuiCol.ButtonHovered, 0xFF008080);
                ImGui.PushStyleColor(ImGuiCol.ButtonActive,  0xFF008080);

                ImGui.Button(Time);

                ImGui.PopStyleColor(3);
            }

            public void PrintWeather()
            {
                if (!Fish.FishRestrictions.HasFlag(FishRestrictions.Weather))
                {
                    ImGui.PushStyleColor(ImGuiCol.Button,        0xFFA0A000);
                    ImGui.PushStyleColor(ImGuiCol.ButtonHovered, 0xFFA0A000);
                    ImGui.PushStyleColor(ImGuiCol.ButtonActive,  0xFFA0A000);
                    ImGui.Button("No Weather Restrictions");
                    ImGui.PopStyleColor(3);
                }
                else if (WeatherIcons.Length == 0)
                {
                    ImGui.PushStyleColor(ImGuiCol.Button,        0xFFA0A000);
                    ImGui.PushStyleColor(ImGuiCol.ButtonHovered, 0xFFA0A000);
                    ImGui.PushStyleColor(ImGuiCol.ButtonActive,  0xFFA0A000);
                    ImGui.Button("Unknown Weather Restrictions");
                    ImGui.PopStyleColor(3);
                }
                else
                {
                    Vector2 pos;
                    var     space = _itemSpacing.X / 2;
                    if (WeatherIcons[0].Length > 0)
                    {
                        pos   =  AlignedTextToWeatherIcon(WeatherIcons[0].Length > 1 ? "Requires one of" : "Requires");
                        pos.X -= space;
                        foreach (var w in WeatherIcons[0])
                        {
                            ImGui.SetCursorPos(pos);
                            pos.X += _iconSize.X;
                            ImGui.Image(w.ImGuiHandle, _iconSize);
                        }

                        pos.X += space;
                        ImGui.SetCursorPos(pos);
                        pos   =  AlignedTextToWeatherIcon(WeatherIcons[1].Length > 1 ? "followed by one of" : "followed by");
                        pos.X -= space;
                    }
                    else
                    {
                        pos   =  AlignedTextToWeatherIcon(WeatherIcons[1].Length > 1 ? "Requires one of" : "Requires");
                        pos.X -= space;
                    }

                    if (WeatherIcons[1].Length == 0)
                    {
                        ImGui.SetCursorPos(pos);
                        AlignedTextToWeatherIcon(new Vector4(0f, 0.8f, 0f, 1f), " Anything");
                    }
                    else
                    {
                        foreach (var w in WeatherIcons[1])
                        {
                            ImGui.SetCursorPos(pos);
                            pos.X += _iconSize.X;
                            ImGui.Image(w.ImGuiHandle, _iconSize);
                        }
                    }
                }
            }

            public void PrintBait()
            {
                if (Bait.Length == 0)
                {
                    ImGui.PushStyleColor(ImGuiCol.Button,        0xFF0000A0);
                    ImGui.PushStyleColor(ImGuiCol.ButtonHovered, 0xFF0000A0);
                    ImGui.PushStyleColor(ImGuiCol.ButtonActive,  0xFF0000A0);
                    ImGui.Button("Unknown Catch Method");
                    ImGui.PopStyleColor(3);
                    return;
                }

                ImGui.PushStyleVar(ImGuiStyleVar.ItemSpacing, _itemSpacing / 2);

                var startPos = ImGui.GetCursorPos();
                var size     = Vector2.Zero;
                if (Snagging != null)
                {
                    ImGui.Image(Snagging.ImGuiHandle, new Vector2(Snagging.Width, Snagging.Height));
                    ImGui.SameLine();
                }

                foreach (var bait in Bait)
                {
                    size = new Vector2(bait.Icon.Width, bait.Icon.Height);

                    ImGui.Image(bait.Icon.ImGuiHandle, size);

                    if (!Fish.IsSpearFish)
                    {
                        ImGui.PushStyleVar(ImGuiStyleVar.ItemSpacing, Vector2.One);
                        ImGui.SameLine();
                        ImGui.BeginGroup();
                        ImGui.PushStyleVar(ImGuiStyleVar.FramePadding, Vector2.Zero);
                        var smallSize = size / 2f;
                        ImGui.Image(bait.HookSet!.ImGuiHandle, smallSize);
                        ImGui.PushStyleColor(ImGuiCol.Button, bait.Bite.Item2);
                        ImGui.Button(bait.Bite.Item1, smallSize);
                        ImGui.PopStyleColor(1);
                        ImGui.PopStyleVar(2);
                        ImGui.EndGroup();
                        ImGui.SameLine();
                    }
                    else
                    {
                        ImGui.SameLine();
                    }

                    var pos = ImGui.GetCursorPosY();
                    ImGui.SetCursorPosY(pos + (bait.Icon.Height - ImGui.GetTextLineHeight()) / 2);
                    ImGui.Text(bait.Name);
                    if (bait.Equals(Bait.Last()))
                        break;

                    ImGui.SameLine();
                    ImGui.Text(" → ");
                    ImGui.SameLine();
                    ImGui.SetCursorPosY(pos);
                }

                ImGui.PopStyleVar();
                ImGui.SetCursorPos(startPos + new Vector2(0, size.Y + ImGui.GetStyle().ItemSpacing.Y));
            }

            public void PrintPredators()
            {
                if (Predators.Length == 0)
                    return;

                ImGui.PushStyleColor(ImGuiCol.Button, LegendaryBite.Item2);
                ImGui.PushStyleVar(ImGuiStyleVar.ItemSpacing, Vector2.One);
                var size   = new Vector2(Predators[0].Icon.Width, Predators[0].Icon.Height) / 1.5f * ImGui.GetIO().FontGlobalScale;
                var offset = (size.Y - _textHeight) / 2f;
                foreach (var predator in Predators)
                {
                    ImGui.BeginGroup();
                    ImGui.Button(predator.Amount, size);
                    ImGui.SameLine();
                    ImGui.Image(predator.Icon.ImGuiHandle, size);
                    ImGui.SameLine();
                    ImGui.SetCursorPosY(ImGui.GetCursorPosY() + offset);
                    ImGui.Text(predator.Name);
                    ImGui.EndGroup();
                }

                ImGui.PopStyleColor();
                ImGui.PopStyleVar();
            }

            public void PrintFolklore()
            {
                if (Fish.Folklore.Length != 0)
                {
                    ImGui.PushStyleColor(ImGuiCol.Button, 0xFF802080);
                    ImGui.Button(Fish.Folklore);
                    ImGui.PopStyleColor();
                    ImGui.SameLine();
                }

                ImGui.PushStyleColor(ImGuiCol.Button, 0xFFC0C0C0);
                ImGui.PushStyleColor(ImGuiCol.Text,   0xFF000000);
                ImGui.Button(Patch);
                ImGui.PopStyleColor(2);
            }

            private static string SetTime(Interface i, Fish fish)
            {
                if (!fish.FishRestrictions.HasFlag(FishRestrictions.Time))
                    return "Always Up";
                if (fish.CatchData?.Hours.AlwaysUp() ?? true)
                    return "Unknown Uptime";

                return fish.CatchData!.Hours.PrintHours();
            }

            private static TextureWrap[][] SetWeather(Interface i, Fish fish)
            {
                if (!fish.FishRestrictions.HasFlag(FishRestrictions.Weather))
                    return new TextureWrap[0][];
                if (fish.CatchData == null || fish.CatchData.PreviousWeather.Length == 0 && fish.CatchData.CurrentWeather.Length == 0)
                    return new TextureWrap[0][];

                return new TextureWrap[][]
                {
                    fish.CatchData.PreviousWeather.Select(w => i._icons[i._weatherSheet.GetRow(w).Icon]).ToArray(),
                    fish.CatchData.CurrentWeather.Select(w => i._icons[i._weatherSheet.GetRow(w).Icon]).ToArray(),
                };
            }

            private static Predator[] SetPredators(Interface i, Fish fish)
            {
                if (fish.CatchData == null || fish.CatchData.Predator.Length == 0)
                    return new Predator[0];

                return fish.CatchData.Predator.Select(p =>
                {
                    var f    = i._plugin.Gatherer!.FishManager.Fish[p.Item1];
                    var icon = i._icons[f.ItemData.Icon];
                    return new Predator()
                    {
                        Amount = p.Item2.ToString(),
                        Name   = f.Name[i._lang],
                        Icon   = icon,
                    };
                }).ToArray();
            }

            private static readonly (string, uint) WeakBite      = ("  !  ", 0xFFFF6020);
            private static readonly (string, uint) StrongBite    = (" ! ! ", 0xFF60D030);
            private static readonly (string, uint) LegendaryBite = (" !!! ", 0xFF0040C0);
            private static readonly (string, uint) UnknownBite   = (" ? ? ", 0xFF404040);

            private static (string, uint) FromBiteType(BiteType bite)
            {
                return bite switch
                {
                    BiteType.Weak      => WeakBite,
                    BiteType.Strong    => StrongBite,
                    BiteType.Legendary => LegendaryBite,
                    _                  => UnknownBite,
                };
            }

            private static TextureWrap FromHookSet(Interface i, HookSet hook)
            {
                return hook switch
                {
                    HookSet.Precise  => i._precisionHookSet,
                    HookSet.Powerful => i._powerfulHookSet,
                    _                => i._hookSet,
                };
            }

            private static BaitOrder[] SetBait(Interface i, Fish fish)
            {
                if (fish.IsSpearFish)
                    return new BaitOrder[1]
                    {
                        new()
                        {
                            Name = $"{fish.Gig} Gig Head",
                            Fish = null,
                            Icon = fish.Gig switch
                            {
                                GigHead.Small  => i._smallGig,
                                GigHead.Normal => i._normalGig,
                                GigHead.Large  => i._largeGig,
                                _              => i._gigs,
                            },
                            Bite    = UnknownBite,
                            HookSet = null,
                        },
                    };

                if (fish.CatchData == null || fish.CatchData.BaitOrder.Length == 0)
                    return new BaitOrder[0];

                var ret  = new BaitOrder[fish.CatchData.BaitOrder.Length];
                var bait = i._plugin.Gatherer!.FishManager.Bait[fish.CatchData.BaitOrder[0]];
                ret[0] = new BaitOrder()
                {
                    Icon = i._icons[bait.Data.Icon],
                    Name = bait.Name[i._lang],
                    Fish = null,
                };
                for (var idx = 1; idx < fish.CatchData.BaitOrder.Length; ++idx)
                {
                    var tmp  = fish.CatchData.BaitOrder[idx];
                    var f    = i._plugin.Gatherer!.FishManager.Fish[tmp];
                    var hook = f.CatchData?.HookSet ?? HookSet.Unknown;
                    ret[idx - 1].HookSet = FromHookSet(i, hook);
                    ret[idx - 1].Bite    = FromBiteType(f.CatchData?.BiteType ?? f.Record.BiteType);
                    ret[idx] = new BaitOrder()
                    {
                        Icon = i._icons[f.ItemData.Icon],
                        Name = f.Name[i._lang],
                        Fish = f,
                    };
                }

                ret[ret.Length - 1].HookSet = FromHookSet(i, fish.CatchData?.HookSet ?? HookSet.Unknown);
                ret[ret.Length - 1].Bite    = FromBiteType(fish.CatchData?.BiteType ?? fish.Record.BiteType);
                return ret;
            }

            private static TextureWrap? SetSnagging(Interface i, BaitOrder[] baitOrder, Fish fish)
            {
                if ((fish.CatchData?.Snagging ?? Classes.Snagging.Unknown) == Classes.Snagging.Required)
                    return i._snagging;

                return baitOrder.Any(bait => (bait.Fish?.CatchData?.Snagging ?? Classes.Snagging.Unknown) == Classes.Snagging.Required)
                    ? i._snagging
                    : null;
            }

            public CachedFish(Interface i, Fish fish)
            {
                Fish             = fish;
                Icon             = i._icons[fish.ItemData.Icon];
                Name             = fish.Name[i._lang];
                NameLower        = Name.ToLowerInvariant();
                Time             = SetTime(i, fish);
                WeatherIcons     = SetWeather(i, fish);
                Bait             = SetBait(i, fish);
                Predators        = SetPredators(i, fish);
                Territory        = fish.FishingSpots.First().Territory!.Name[i._lang];
                TerritoryLower   = Territory.ToLowerInvariant();
                FishingSpot      = fish.FishingSpots.First().PlaceName![i._lang];
                FishingSpotLower = FishingSpot.ToLowerInvariant();
                Snagging         = SetSnagging(i, Bait, fish);
                Patch            = $"Patch {fish.CatchData?.Patch.ToVersionString() ?? "???"}";
            }
        }
    }
}
