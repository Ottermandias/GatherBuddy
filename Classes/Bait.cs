using GatherBuddy.Utility;

namespace GatherBuddy.Classes
{
    public readonly struct Bait
    {
        public uint   Id   { get; }
        public FFName Name { get; }

        public Bait(uint id, FFName name)
        {
            Id   = id;
            Name = name;
        }

        public static Bait Unknown { get; } = new Bait(0, new FFName());

        public override int GetHashCode()
            => Id.GetHashCode();
    }
}
