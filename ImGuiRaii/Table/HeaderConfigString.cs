using System;
using System.Text.RegularExpressions;
using Dalamud.Interface;
using ImGuiNET;

namespace ImGuiOtter.Table;

public class HeaderConfigString<TItem> : HeaderConfig<TItem>
{
    public HeaderConfigString()
        => Flags &= ~ImGuiTableColumnFlags.NoResize;

    public    string FilterValue = string.Empty;
    protected Regex? FilterRegex;

    public virtual string ToName(TItem item)
        => item!.ToString() ?? string.Empty;

    public override int Compare(TItem lhs, TItem rhs)
        => string.Compare(ToName(lhs), ToName(rhs), StringComparison.InvariantCulture);

    public override bool DrawFilter()
    {
        using var style = ImGuiRaii.PushStyle(ImGuiStyleVar.FrameRounding, 0);

        ImGui.SetNextItemWidth(-Table.ArrowWidth * ImGuiHelpers.GlobalScale);
        var tmp = FilterValue;
        if (ImGui.InputTextWithHint(FilterLabel, Label, ref tmp, 64) && tmp != FilterValue)
        {
            FilterValue = tmp;
            try
            {
                FilterRegex = new Regex(FilterValue, RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
            }
            catch
            {
                FilterRegex = null;
            }

            return true;
        }

        return false;
    }

    public override bool FilterFunc(TItem item)
    {
        var name = ToName(item);
        if (FilterValue.Length == 0)
            return true;

        return FilterRegex?.IsMatch(name) ?? name.Contains(FilterValue, StringComparison.InvariantCultureIgnoreCase);
    }

    public override void DrawColumn(TItem item, int _)
    {
        ImGui.Text(ToName(item));
    }
}
