using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using GatherBuddy.Game;
using ImGuiNET;

namespace GatherBuddy.Gui
{
    public partial class Interface
    {
        private readonly        string[] _allTimedNodesNames;
        private static readonly string[] SoundNameList = Enum.GetNames(typeof(Sounds)).Where(s => s != "Unknown").ToArray();

        private string _newAlarmName     = "";
        private int    _newAlarmIdx      = 0;
        private bool   _focusComboFilter = false;
        private string _nodeFilter       = "";

        private          float _alarmsSpacing;
        private readonly float _longestNodeStringLength = 0f;

        private void DrawDeleteAndEnable()
        {
            ImGui.BeginGroup();
            ImGui.Dummy(new Vector2(0, _textHeight));
            for (var idx = 0; idx < _plugin.Alarms!.Alarms.Count; ++idx)
            {
                var alarm = _plugin.Alarms.Alarms[idx];
                if (ImGui.Button($"  −  ##{idx}"))
                    _plugin.Alarms.RemoveNode(idx--);
                if (ImGui.IsItemHovered())
                    ImGui.SetTooltip("Delete this alarm.");

                HorizontalSpace(_alarmsSpacing);
                var enabled = alarm.Enabled;
                if (ImGui.Checkbox($"##enabled_{idx}", ref enabled) && enabled != alarm.Enabled)
                    _plugin.Alarms.ChangeNodeStatus(idx, enabled);
                if (ImGui.IsItemHovered())
                    ImGui.SetTooltip("Enable or disable this alarm.");
            }

            ImGui.EndGroup();
        }

        public void DrawNames()
        {
            ImGui.BeginGroup();
            ImGui.Text("Name");
            ImGui.SameLine();
            ImGui.Dummy(new Vector2(0, _textHeight));
            foreach (var alarm in _plugin.Alarms!.Alarms)
            {
                ImGui.AlignTextToFramePadding();
                ImGui.Text(alarm.Name);
                HorizontalSpace(_alarmsSpacing);
                ImGui.NewLine();
            }

            ImGui.EndGroup();
        }

        public void DrawOffsets()
        {
            ImGui.BeginGroup();
            ImGui.Text("Pre");
            if (ImGui.IsItemHovered())
                ImGui.SetTooltip("Trigger the respective alarm the given number (0-1439) of Eorzea Minutes earlier.");
            ImGui.SameLine();
            ImGui.Dummy(new Vector2(0, _textHeight));
            for (var idx = 0; idx < _plugin.Alarms!.Alarms.Count; ++idx)
            {
                var alert  = _plugin.Alarms.Alarms[idx];
                var offset = alert.MinuteOffset.ToString();
                ImGui.SetNextItemWidth(ImGui.CalcTextSize("99999").X);
                if (!ImGui.InputText($"##Offset{idx}", ref offset, 4, ImGuiInputTextFlags.CharsDecimal))
                    continue;

                if (int.TryParse(offset, out var minutes))
                {
                    minutes %= 24 * 60;
                    if (minutes != alert.MinuteOffset)
                        _plugin.Alarms.ChangeNodeOffset(idx, minutes);
                }
                else if (offset.Length == 0 && alert.MinuteOffset != 0)
                {
                    _plugin.Alarms.ChangeNodeOffset(idx, 0);
                }
            }

            ImGui.EndGroup();
        }

        private void DrawAlarms()
        {
            var boxSize = ImGui.CalcTextSize("Sound9999999").X;
            ImGui.BeginGroup();
            ImGui.Text("Alarm Sound");
            if (ImGui.IsItemHovered())
                ImGui.SetTooltip("Set an optional sound that should be played when this alarm triggers.\n"
                  + "The sounds are the same as in the character configuration -> log window -> notification sounds.");
            ImGui.SameLine();
            ImGui.Dummy(new Vector2(0, _textHeight));

            for (var idx = 0; idx < _plugin.Alarms!.Alarms.Count; ++idx)
            {
                var alert = _plugin.Alarms.Alarms[idx];
                var sound = alert.SoundId.ToIdx();
                if (sound == -1)
                {
                    _plugin.Alarms.ChangeNodeSound(idx, Sounds.None);
                    sound = 0;
                }

                ImGui.SetNextItemWidth(boxSize);
                if (!ImGui.Combo($"##sound_{idx}", ref sound, SoundNameList, SoundNameList.Length))
                    continue;

                var tmp = SoundsExtensions.FromIdx(sound);
                if (tmp != Sounds.Unknown && tmp != alert.SoundId)
                    _plugin.Alarms.ChangeNodeSound(idx, tmp);
            }

            ImGui.EndGroup();
        }

        private void DrawPrintMessageBoxes()
        {
            ImGui.BeginGroup();
            ImGui.Text("Chat");
            if (ImGui.IsItemHovered())
                ImGui.SetTooltip("Toggle whether the alarm is printed to chat or not.");
            ImGui.SameLine();
            ImGui.Dummy(new Vector2(0, _textHeight));

            for (var idx = 0; idx < _plugin.Alarms!.Alarms.Count; ++idx)
            {
                var alert = _plugin.Alarms.Alarms[idx];
                var print = alert.PrintMessage;
                if (ImGui.Checkbox($"##print{idx}", ref print) && print != alert.PrintMessage)
                    _plugin.Alarms.ChangePrintStatus(idx, print);
            }

            ImGui.EndGroup();
        }

        private void DrawHours()
        {
            ImGui.BeginGroup();
            ImGui.Text("Alarm Times");
            if (ImGui.IsItemHovered())
                ImGui.SetTooltip("The uptimes for the node monitored in this alarm. Hover for the items.");
            ImGui.SameLine();
            ImGui.Dummy(new Vector2(0, _textHeight));

            foreach (var alert in _plugin.Alarms!.Alarms)
            {
                ImGui.AlignTextToFramePadding();
                ImGui.Text(alert.Node!.Times!.PrintHours(true, " | "));
                if (ImGui.IsItemHovered())
                    ImGui.SetTooltip(alert.Node!.Items!.PrintItems("\n", _lang));
            }

            ImGui.EndGroup();
        }

        private void DrawNewAlarm()
        {
            if (ImGui.Button("  + "))
            {
                _plugin.Alarms!.AddNode(_newAlarmName, _plugin.Alarms!.AllTimedNodes[_newAlarmIdx]);
                _newAlarmName = "";
            }

            ImGui.SameLine();
            ImGui.SetNextItemWidth(ImGui.CalcTextSize("mmmmmmmmmmmm").X);
            ImGui.InputTextWithHint("##Name", "New Alarm Name", ref _newAlarmName, 64);
            ImGui.SameLine();
            ImGui.SetNextItemWidth(-1);

            var comboSize = _longestNodeStringLength * _globalScale;
            DrawComboWithFilter("##Node", _allTimedNodesNames, ref _newAlarmIdx, ref _nodeFilter, ref _focusComboFilter, comboSize, 6);
        }
        private void DrawAlarmsTab()
        {
            _alarmsSpacing = _itemSpacing.X / 2;
            var listSize = new Vector2(-1, -ImGui.GetTextLineHeightWithSpacing() - 2 * ImGui.GetStyle().FramePadding.X);
            if (ImGui.BeginChild("##alarmlist", listSize, true))
            {
                DrawDeleteAndEnable();
                ImGui.SameLine();
                DrawNames();
                ImGui.SameLine();
                DrawOffsets();
                ImGui.SameLine();
                DrawAlarms();
                ImGui.SameLine();
                DrawPrintMessageBoxes();
                ImGui.SameLine();
                DrawHours();
                ImGui.EndChild();
            }

            DrawNewAlarm();
        }
    }
}
