using GatherBuddy.Utility;
using ItemRow = Lumina.Excel.GeneratedSheets.Item;

namespace GatherBuddy.Game
{
    public readonly struct Bait
    {
        public ItemRow Data { get; }
        public FFName  Name { get; }

        public uint Id
            => Data.RowId;

        public Bait(ItemRow data, FFName name)
        {
            Data = data;
            Name = name;
        }

        public static Bait Unknown { get; } = new(new ItemRow { RowId = 0 }, new FFName());

        public override int GetHashCode()
            => Id.GetHashCode();
    }
}
