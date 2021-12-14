using System;
using ImGuiNET;

namespace ImGuiOtter;

public static partial class ImGuiRaii
{
    public static Id PushId(string id)
        => new Id().Push(id);

    public static Id PushId(int id)
        => new Id().Push(id);

    public sealed class Id : IDisposable
    {
        private int _count;

        public Id Push(string id, bool condition = true)
        {
            if (condition)
            {
                ImGui.PushID(id);
                ++_count;
            }

            return this;
        }

        public Id Push(int id, bool condition = true)
        {
            if (condition)
            {
                ImGui.PushID(id);
                ++_count;
            }

            return this;
        }

        public void Pop(int num = 1)
        {
            num    =  Math.Min(num, _count);
            _count -= num;
            while (num-- > 0)
                ImGui.PopID();
        }

        public void Dispose()
            => Pop(_count);
    }
}
