using System.ComponentModel;
using System.Numerics;
using GatherBuddy.Classes;
using GatherBuddy.Enums;
using ImGuiNET;

namespace GatherBuddy.Gui
{
    public partial class Interface
    {
        private void DrawDeleteAndEnable()
        {
            using var group = ImGuiRaii.NewGroup();
            ImGui.Dummy(new Vector2(0, _textHeight));
            for (var idx = 0; idx < _plugin.Alarms!.Alarms.Count; ++idx)
            {
                var alarm = _plugin.Alarms.Alarms[idx];
                if (ImGui.Button($"  −  ##{idx}"))
                    _plugin.Alarms.RemoveAlarm(idx--);
                ImGuiHelper.HoverTooltip("Delete this alarm.");

                HorizontalSpace(_alarmsSpacing);
                var enabled = alarm.Enabled;
                if (ImGui.Checkbox($"##enabled_{idx}", ref enabled) && enabled != alarm.Enabled)
                    _plugin.Alarms.ChangeNodeStatus(idx, enabled);
                ImGuiHelper.HoverTooltip("Enable or disable this alarm.");
            }
        }

        public void DrawNames()
        {
            using var group = ImGuiRaii.NewGroup();
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
        }

        public void DrawOffsets()
        {
            using var group = ImGuiRaii.NewGroup();
            ImGui.Text("Pre");
            ImGuiHelper.HoverTooltip(
                "Trigger the respective alarm the given number (0-1439) of Eorzea Minutes earlier.\nFor fish, the offset is in real minutes instead of Eorzea minutes.");

            ImGui.SameLine();
            ImGui.Dummy(new Vector2(0, _textHeight));

            for (var idx = 0; idx < _plugin.Alarms!.Alarms.Count; ++idx)
            {
                var alarm  = _plugin.Alarms.Alarms[idx];
                var offset = alarm.MinuteOffset.ToString();
                ImGui.SetNextItemWidth(_alarmCache.OffsetSize * _globalScale);
                if (!ImGui.InputText($"##Offset{idx}", ref offset, 4, ImGuiInputTextFlags.CharsDecimal))
                    continue;

                if (int.TryParse(offset, out var minutes))
                {
                    minutes %= 24 * 60;
                    if (minutes != alarm.MinuteOffset)
                        _plugin.Alarms.ChangeNodeOffset(idx, minutes);
                }
                else if (offset.Length == 0 && alarm.MinuteOffset != 0)
                {
                    _plugin.Alarms.ChangeNodeOffset(idx, 0);
                }
            }
        }

        private void DrawAlarms()
        {
            using var group = ImGuiRaii.NewGroup();
            ImGui.Text("Alarm Sound");
            ImGuiHelper.HoverTooltip("Set an optional sound that should be played when this alarm triggers.\n"
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

                ImGui.SetNextItemWidth(_alarmCache.SoundSize * _globalScale);
                if (!ImGui.Combo($"##sound{idx}", ref sound, _alarmCache.SoundNames, _alarmCache.SoundNames.Length))
                    continue;

                var tmp = SoundsExtensions.FromIdx(sound);
                if (tmp != Sounds.Unknown && tmp != alert.SoundId)
                    _plugin.Alarms.ChangeNodeSound(idx, tmp);
            }
        }

        private void DrawPrintMessageBoxes()
        {
            using var group = ImGuiRaii.NewGroup();
            ImGui.Text("Chat");
            ImGuiHelper.HoverTooltip("Toggle whether the alarm is printed to chat or not.");

            ImGui.SameLine();
            ImGui.Dummy(new Vector2(0, _textHeight));

            for (var idx = 0; idx < _plugin.Alarms!.Alarms.Count; ++idx)
            {
                var alert = _plugin.Alarms.Alarms[idx];
                var print = alert.PrintMessage;
                if (ImGui.Checkbox($"##print{idx}", ref print) && print != alert.PrintMessage)
                    _plugin.Alarms.ChangePrintStatus(idx, print);
            }
        }

        private void DrawHours()
        {
            using var group = ImGuiRaii.NewGroup();
            ImGui.Text("Alarm Times / Fish");
            ImGuiHelper.HoverTooltip("The uptimes for the node monitored in this alarm, or the respective fish");

            ImGui.SameLine();
            ImGui.Dummy(new Vector2(0, _textHeight));

            foreach (var alarm in _alarmCache.Manager.Alarms)
            {
                ImGui.AlignTextToFramePadding();
                switch (alarm.Type)
                {
                    case AlarmType.Node:
                        ImGui.Text(alarm.Node!.Times!.PrintHours(true, " | "));
                        ImGuiHelper.HoverTooltip(alarm.Node!.Items!.PrintItems("\n", GatherBuddy.Language));
                        break;
                    case AlarmType.Fish:
                        ImGui.Text(alarm.Fish!.Name[GatherBuddy.Language]);
                        break;
                    default: throw new InvalidEnumArgumentException();
                }
            }
        }

        private void DrawNewAlarm()
        {
            if (ImGui.Button("  + "))
                _alarmCache.AddAlarm();

            ImGui.SameLine();
            ImGui.SetNextItemWidth(_alarmCache.NameSize * _globalScale);
            ImGui.InputTextWithHint("##Name", "New Alarm Name", ref _alarmCache.NewName, 64);
            ImGui.SameLine();
            ImGui.SetNextItemWidth(-1);

            var comboSize = _alarmCache.LongestNodeNameLength * _globalScale;
            DrawComboWithFilter("##Node",    _alarmCache.AllTimedNodeNames, ref _alarmCache.NewIdx, ref _alarmCache.NodeFilter,
                ref _alarmCache.FocusFilter, comboSize,                     6);
        }

        private void DrawAlarmsTab()
        {
            _alarmsSpacing = _itemSpacing.X / 2;

            var       listSize = new Vector2(-1, -_textHeight - 2 * _framePadding.X);
            using var raii     = new ImGuiRaii();
            if (raii.BeginChild("##alarmlist", listSize, true))
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
            }

            raii.End();

            DrawNewAlarm();
        }
    }
}
