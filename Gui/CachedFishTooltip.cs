using System.Linq;
using System.Numerics;
using GatherBuddy.Enums;
using GatherBuddy.Gui.Cache;
using ImGuiNET;

namespace GatherBuddy.Gui
{
    public partial class Interface
    {
        private static void PrintTime(Cache.Fish fish)
            => DrawButtonText(fish.Time, Vector2.Zero, Colors.FishTab.Time);

        private static void PrintWeather(Cache.Fish fish)
        {
            if (!fish.Base.FishRestrictions.HasFlag(FishRestrictions.Weather))
            {
                DrawButtonText("No Weather Restrictions", Vector2.Zero, Colors.FishTab.Weather);
            }
            else if (fish.WeatherIcons.Length == 0)
            {
                DrawButtonText("Unknown Weather Restrictions", Vector2.Zero, Colors.FishTab.Weather);
            }
            else
            {
                Vector2 pos;
                var     space = _itemSpacing.X / 2;
                if (fish.WeatherIcons[0].Length > 0)
                {
                    pos   =  AlignedTextToWeatherIcon(fish.WeatherIcons[0].Length > 1 ? "Requires one of" : "Requires");
                    pos.X -= space;
                    foreach (var w in fish.WeatherIcons[0])
                    {
                        ImGui.SetCursorPos(pos);
                        pos.X += _iconSize.X;
                        ImGui.Image(w.ImGuiHandle, _weatherIconSize);
                    }

                    pos.X += space;
                    ImGui.SetCursorPos(pos);
                    pos   =  AlignedTextToWeatherIcon(fish.WeatherIcons[1].Length > 1 ? "followed by one of" : "followed by");
                    pos.X -= space;
                }
                else
                {
                    pos   =  AlignedTextToWeatherIcon(fish.WeatherIcons[1].Length > 1 ? "Requires one of" : "Requires");
                    pos.X -= space;
                }

                if (fish.WeatherIcons[1].Length == 0)
                {
                    ImGui.SetCursorPos(pos);
                    AlignedTextToWeatherIcon(Colors.FishTab.WeatherVec4, " Anything");
                }
                else
                {
                    foreach (var w in fish.WeatherIcons[1])
                    {
                        ImGui.SetCursorPos(pos);
                        pos.X += _iconSize.X;
                        ImGui.Image(w.ImGuiHandle, _weatherIconSize);
                    }
                }
            }
        }

        private static void PrintBait(Cache.Fish fish)
        {
            if (fish.Bait.Length == 0)
            {
                DrawButtonText("Unknown Catch Method", Vector2.Zero, Colors.FishTab.Bait);
                return;
            }

            ImGui.PushStyleVar(ImGuiStyleVar.ItemSpacing, _itemSpacing / 2);

            var startPos = ImGui.GetCursorPos();
            var size     = Vector2.Zero;
            if (fish.Snagging != null)
            {
                ImGui.Image(fish.Snagging.ImGuiHandle, _iconSize);
                ImGui.SameLine();
            }

            foreach (var bait in fish.Bait)
            {
                size = _iconSize;
                ImGui.Image(bait.Icon.ImGuiHandle, size);

                if (!fish.Base.IsSpearFish)
                {
                    ImGui.PushStyleVar(ImGuiStyleVar.ItemSpacing, Vector2.One);
                    ImGui.SameLine();
                    ImGui.BeginGroup();
                    ImGui.PushStyleVar(ImGuiStyleVar.FramePadding, Vector2.Zero);
                    ImGui.Image(bait.HookSet!.ImGuiHandle, _smallIconSize);
                    ImGui.PushStyleColor(ImGuiCol.Button, bait.Bite.Item2);
                    ImGui.Button(bait.Bite.Item1, _smallIconSize);
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
                ImGui.SetCursorPosY(pos + (_iconSize.Y - ImGui.GetTextLineHeight()) / 2);
                ImGui.Text(bait.Name);
                if (bait.Equals(fish.Bait.Last()))
                    break;

                ImGui.SameLine();
                ImGui.Text(" → ");
                ImGui.SameLine();
                ImGui.SetCursorPosY(pos);
            }

            ImGui.PopStyleVar();
            ImGui.SetCursorPos(startPos + new Vector2(0, size.Y + _itemSpacing.Y));
        }

        private static void PrintPredators(Cache.Fish fish)
        {
            if (fish.Predators.Length == 0)
                return;

            ImGui.PushStyleColor(ImGuiCol.Button, Colors.FishTab.Predator);
            ImGui.PushStyleVar(ImGuiStyleVar.ItemSpacing, Vector2.One);
            var size   = _iconSize / 1.5f;
            var offset = (size.Y - _textHeight) / 2f;
            foreach (var predator in fish.Predators)
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

        private static void PrintFolklore(Cache.Fish fish)
        {
            if (fish.Base.Folklore.Length != 0)
            {
                ImGui.PushStyleColor(ImGuiCol.Button, Colors.FishTab.Folklore);
                ImGui.Button(fish.Base.Folklore);
                ImGui.PopStyleColor();
                ImGui.SameLine();
            }

            ImGui.PushStyleColor(ImGuiCol.Button, Colors.FishTab.Patch);
            ImGui.PushStyleColor(ImGuiCol.Text,   Colors.FishTab.PatchText);
            ImGui.Button(fish.Patch);
            ImGui.PopStyleColor(2);
        }

        private static void SetTooltip(Cache.Fish fish)
        {
            ImGui.BeginTooltip();
            ImGui.PushStyleVar(ImGuiStyleVar.ItemSpacing, new Vector2(_itemSpacing.X, _itemSpacing.Y * 1.5f));
            PrintTime(fish);
            PrintWeather(fish);
            PrintBait(fish);
            PrintPredators(fish);
            PrintFolklore(fish);
            ImGui.PopStyleVar();
            ImGui.EndTooltip();
        }
    }
}
