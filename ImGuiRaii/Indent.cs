using System;
using System.Diagnostics;
using Dalamud.Interface;
using ImGuiNET;

namespace ImGuiOtter;

public static partial class ImGuiRaii
{
    public static Indent PushIndent(float f, bool scaled = true, bool condition = true)
        => new Indent().Push(f, scaled, condition);

    public static Indent PushIndent(int i = 1, bool scaled = true, bool condition = true)
        => new Indent().Push(i, scaled, condition);

    public sealed class Indent : IDisposable
    {
        public float Indentation { get; private set; }

        public Indent Push(float indent, bool scaled = true, bool condition = true)
        {
            Debug.Assert(indent >= 0f);
            if (condition)
            {
                if (scaled)
                    indent *= ImGuiHelpers.GlobalScale;

                ImGui.Indent(indent);
                Indentation += indent;
            }

            return this;
        }

        public Indent Push(int i = 1, bool scaled = true, bool condition = true)
        {
            if (condition)
            {
                var spacing = i * ImGui.GetStyle().IndentSpacing * (scaled ? ImGuiHelpers.GlobalScale : 1f);
                Debug.Assert(spacing >= 0);
                ImGui.Indent(spacing);
                Indentation += spacing;
            }

            return this;
        }

        public void Pop(float indent, bool scaled = true)
        {
            if (scaled)
                indent *= ImGuiHelpers.GlobalScale;

            Debug.Assert(indent >= 0f);
            ImGui.Unindent(indent);
            Indentation -= indent;
        }

        public void Pop(int i, bool scaled = true)
        {
            var spacing = i * ImGui.GetStyle().IndentSpacing * (scaled ? ImGuiHelpers.GlobalScale : 1f);
            Debug.Assert(spacing >= 0);
            ImGui.Unindent(spacing);
            Indentation -= spacing;
        }

        public void Dispose()
            => Pop(Indentation, false);
    }
}
