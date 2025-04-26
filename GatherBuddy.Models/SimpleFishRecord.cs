namespace GatherBuddy.Models;

public class SimpleFishRecord
{
    public Guid    Id          { get; set; }
    public uint    BaitId      { get; set; }
    public uint    CatchId     { get; set; }
    public int     Timestamp   { get; set; }
    public Effects Effects     { get; set; }
    public ushort  Bite        { get; set; }
    public ushort  Perception  { get; set; }
    public ushort  Gathering   { get; set; }
    public ushort  FishingSpot { get; set; }
    public byte    TugAndHook  { get; set; }
    public byte    Amount      { get; set; }
    public float   PositionX   { get; set; }
    public float   PositionY   { get; set; }
    public float   PositionZ   { get; set; }
    public float   Rotation    { get; set; }

    public override bool Equals(object? obj)
        => obj is SimpleFishRecord record && Id == record.Id;

    public override int GetHashCode()
        => Id.GetHashCode();
}
