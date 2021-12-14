using ImGuiNET;

namespace ImGuiOtter.Table;

public class HeaderConfig<TItem>
{
    public string                Label = string.Empty;
    public ImGuiTableColumnFlags Flags = ImGuiTableColumnFlags.NoResize;

    public virtual float Width
        => -1f;

    public string FilterLabel
        => $"##{Label}Filter";

    public virtual bool DrawFilter()
    {
        ImGui.Text(Label);
        return false;
    }

    public virtual bool FilterFunc(TItem item)
        => true;

    public virtual int Compare(TItem lhs, TItem rhs)
        => 0;

    public virtual void DrawColumn(TItem item, int idx)
    { }

    public int CompareInv(TItem lhs, TItem rhs)
        => Compare(rhs, lhs);
}
