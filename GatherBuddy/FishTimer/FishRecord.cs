using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using ECommons.MathHelpers;
using GatherBuddy.Classes;
using GatherBuddy.Enums;
using GatherBuddy.Models;
using GatherBuddy.Structs;
using GatherBuddy.Time;
using Lumina.Excel.Sheets;
using MessagePack;
using Newtonsoft.Json;
using FishingSpot = GatherBuddy.Classes.FishingSpot;

namespace GatherBuddy.FishTimer;

public static class EffectsExtensions
{
    public static byte AmbitiousLure(this Effects effects)
        => (effects & (Effects.AmbitiousLure1 | Effects.AmbitiousLure2)) switch
        {
            0                      => 0,
            Effects.AmbitiousLure1 => 1,
            Effects.AmbitiousLure2 => 2,
            _                      => 3,
        };

    public static byte ModestLure(this Effects effects)
        => (effects & (Effects.ModestLure1 | Effects.ModestLure2)) switch
        {
            0                   => 0,
            Effects.ModestLure1 => 1,
            Effects.ModestLure2 => 2,
            _                   => 3,
        };

    public const Effects Lures = Effects.ModestLure1
      | Effects.ModestLure2
      | Effects.AmbitiousLure1
      | Effects.AmbitiousLure2;

    public static bool HasLure(this Effects effects)
        => (effects & Lures) != 0;

    public static bool HasValidLure(this Effects effects)
        => (effects & Effects.ValidLure) != 0;
}

[MessagePackObject(AllowPrivate = true)]
public partial class FishRecord
{
    public const ushort MinBiteTime        = 1000;
    public const ushort MaxBiteTime        = 65000;
    public const byte   Version            = 2;
    public const int    Version1ByteLength = 4 + 4 + 4 + 4 + 4 + 2 + 2 + 2 + 2 + 2 + 1 + 1;
    public const int    Version2ByteLength = 4 + 4 + 4 + 4 + 4 + 2 + 2 + 2 + 2 + 2 + 1 + 1 + 4 + 4 + 4 + 4 + 16 + 4;
    public const int    ByteLength         = Version2ByteLength;

    public static readonly Effects ValidEffects = Enum.GetValues<Effects>().Aggregate((a, b) => a | b);


    [Key(0)]
    private uint _bait;

    [Key(1)]
    private uint _catch;

    [Key(2)]
    public int ContentIdHash { get; set; }

    [Key(3)]
    private int _timeStamp;

    [Key(4)]
    public Effects Flags { get; set; }

    [Key(5)]
    public ushort Bite { get; set; }

    [Key(6)]
    public ushort Perception { get; set; }

    [Key(7)]
    public ushort Gathering { get; set; }

    [Key(8)]
    public ushort Size { get; set; }

    [Key(9)]
    private ushort _fishingSpot;

    [Key(10)]
    private byte _tugAndHook;

    [Key(11)]
    public byte Amount { get; set; }

    [Key(12)]
    private float _x;

    [Key(13)]
    private float _y;

    [Key(14)]
    private float _z;

    [Key(15)]
    private float _rotation;

    [Key(16)]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Key(17)]
    private uint _worldId;


    [NotMapped]
    [IgnoreMember]
    [JsonIgnore]
    public World World
    {
        get => Dalamud.GameData.GetExcelSheet<World>().GetRow(_worldId);
        set => _worldId = value.RowId;
    }

    [NotMapped]
    [IgnoreMember]
    [JsonIgnore]
    public TimeStamp TimeStamp
    {
        get => new(_timeStamp * 1000L);
        set => _timeStamp = (int)(value.Time / 1000);
    }

    [NotMapped]
    [IgnoreMember]
    [JsonIgnore]
    public Vector3 Position
    {
        get => new(_x, _y, _z);
        set
        {
            _x = value.X;
            _y = value.Y;
            _z = value.Z;
        }
    }

    [NotMapped]
    [IgnoreMember]
    [JsonIgnore]
    public Angle RotationAngle
    {
        get => new Angle(_rotation);
        set => _rotation = value.Rad;
    }

    [IgnoreMember]
    public bool PositionDataValid
        => _x != 0 && _y != 0 && _z != 0 && _rotation != 0;

    [NotMapped]
    [IgnoreMember]
    [JsonIgnore]
    public FishingSpot? FishingSpot
    {
        get => HasSpot ? GatherBuddy.GameData.FishingSpots.GetValueOrDefault(_fishingSpot) : null;
        set => _fishingSpot = (ushort)(value?.Id ?? 0);
    }

    [IgnoreMember]
    public bool HasSpot
        => _fishingSpot != 0;

    [NotMapped]
    [IgnoreMember]
    [JsonIgnore]
    public Bait Bait
    {
        get => HasBait
            ? GatherBuddy.GameData.Bait.TryGetValue(_bait, out var b) ? b :
            GatherBuddy.GameData.Fishes.TryGetValue(_bait, out var f) ? new Bait(f.ItemData) : Bait.Unknown
            : Bait.Unknown;
        set => _bait = value.Id;
    }

    [IgnoreMember]
    public bool HasBait
        => _bait != 0;

    [NotMapped]
    [IgnoreMember]
    [JsonIgnore]
    public Fish? Catch
    {
        get => HasCatch ? GatherBuddy.GameData.Fishes.GetValueOrDefault(_catch) : null;
        set => _catch = value?.ItemId ?? 0;
    }

    [IgnoreMember]
    public uint CatchId
        => _catch;

    [IgnoreMember]
    public uint BaitId
        => _bait;

    [IgnoreMember]
    public ushort SpotId
        => _fishingSpot;

    [IgnoreMember]
    public bool HasCatch
        => _catch != 0;

    public void SetTugHook(BiteType bite, HookSet set)
    {
        var b = bite switch
        {
            BiteType.None      => 0,
            BiteType.Weak      => 1,
            BiteType.Strong    => 2,
            BiteType.Legendary => 3,
            _                  => 4,
        };
        b |= set switch
        {
            HookSet.None       => 0,
            HookSet.Hook       => 1 << 4,
            HookSet.Precise    => 2 << 4,
            HookSet.Powerful   => 3 << 4,
            HookSet.DoubleHook => 4 << 4,
            HookSet.TripleHook => 5 << 4,
            HookSet.Stellar    => 7 << 4,
            _                  => 6 << 4,
        };
        _tugAndHook = (byte)b;
    }

    [IgnoreMember]
    public BiteType Tug
        => (_tugAndHook & 0x0F) switch
        {
            0 => BiteType.None,
            1 => BiteType.Weak,
            2 => BiteType.Strong,
            3 => BiteType.Legendary,
            _ => BiteType.Unknown,
        };

    [IgnoreMember]
    public HookSet Hook
        => (_tugAndHook >> 4) switch
        {
            0 => HookSet.None,
            1 => HookSet.Hook,
            2 => HookSet.Precise,
            3 => HookSet.Powerful,
            4 => HookSet.DoubleHook,
            5 => HookSet.TripleHook,
            7 => HookSet.Stellar,
            _ => HookSet.Unknown,
        };

    public bool Escaped()
        => Hook != HookSet.None && Tug != BiteType.None;

    public bool MissedChance()
        => Tug != BiteType.None && Hook == HookSet.None;

    public bool NothingHooked()
        => Hook == HookSet.None && Tug != BiteType.None;

    public SimpleFishRecord ToSimpleRecord()
    {
        var newRecord = new SimpleFishRecord();
        newRecord.Id          = Id;
        newRecord.BaitId      = _bait;
        newRecord.CatchId     = _catch;
        newRecord.Timestamp   = _timeStamp;
        newRecord.Effects     = Flags;
        newRecord.Bite        = Bite;
        newRecord.Perception  = Perception;
        newRecord.Gathering   = Gathering;
        newRecord.FishingSpot = _fishingSpot;
        newRecord.TugAndHook  = _tugAndHook;
        newRecord.Amount      = Amount;
        newRecord.PositionX   = _x;
        newRecord.PositionY   = _y;
        newRecord.PositionZ   = _z;
        newRecord.Rotation    = _rotation;
        return newRecord;
    }

    public static FishRecord FromSimpleRecord(SimpleFishRecord simpleRecord)
    {
        var record = new FishRecord();
        record.Id           = simpleRecord.Id;
        record._bait        = simpleRecord.BaitId;
        record._catch       = simpleRecord.CatchId;
        record._timeStamp   = simpleRecord.Timestamp;
        record.Flags        = simpleRecord.Effects;
        record.Bite         = simpleRecord.Bite;
        record.Perception   = simpleRecord.Perception;
        record.Gathering    = simpleRecord.Gathering;
        record._fishingSpot = simpleRecord.FishingSpot;
        record._tugAndHook  = simpleRecord.TugAndHook;
        record.Amount       = simpleRecord.Amount;
        record._x           = simpleRecord.PositionX;
        record._y           = simpleRecord.PositionY;
        record._z           = simpleRecord.PositionZ;
        record._rotation    = simpleRecord.Rotation;
        return record;
    }

    public struct JsonStruct
    {
        public ushort   Gathering;
        public ushort   Perception;
        public bool     Valid;
        public int      TimeStamp;
        public uint     BaitItemId;
        public ushort   FishingSpotId;
        public bool     Snagging;
        public bool     Chum;
        public bool     Intuition;
        public bool     FishEyes;
        public bool     IdenticalCast;
        public bool     SurfaceSlap;
        public bool     PrizeCatch;
        public bool     Patience;
        public bool     Patience2;
        public bool     BigGameFishing;
        public byte     AmbitiousLure;
        public byte     ModestLure;
        public ushort   BiteTime;
        public BiteType Tug;
        public HookSet  HookSet;
        public uint     CatchItemId;
        public byte     Amount;
        public float    Size;
        public bool     Collectible;
        public bool     Large;
        public bool     ValidLure;
        public float    X;
        public float    Y;
        public float    Z;
        public float    Rotation;
        public Guid     Id;
        public uint     WorldId;
    }

    public JsonStruct ToJson()
        => new()
        {
            Gathering      = Gathering,
            Perception     = Perception,
            Valid          = Flags.HasFlag(Effects.Valid),
            TimeStamp      = _timeStamp,
            BaitItemId     = BaitId,
            FishingSpotId  = SpotId,
            Snagging       = Flags.HasFlag(Effects.Snagging),
            Chum           = Flags.HasFlag(Effects.Chum),
            Intuition      = Flags.HasFlag(Effects.Intuition),
            FishEyes       = Flags.HasFlag(Effects.FishEyes),
            IdenticalCast  = Flags.HasFlag(Effects.IdenticalCast),
            SurfaceSlap    = Flags.HasFlag(Effects.SurfaceSlap),
            PrizeCatch     = Flags.HasFlag(Effects.PrizeCatch),
            Patience       = Flags.HasFlag(Effects.Patience),
            Patience2      = Flags.HasFlag(Effects.Patience2),
            BigGameFishing = Flags.HasFlag(Effects.BigGameFishing),
            AmbitiousLure  = Flags.AmbitiousLure(),
            ModestLure     = Flags.ModestLure(),
            ValidLure      = Flags.HasValidLure(),
            BiteTime       = Bite,
            Tug            = Tug,
            HookSet        = Hook,
            CatchItemId    = CatchId,
            Amount         = Amount,
            Size           = Size / 10f,
            Collectible    = Flags.HasFlag(Effects.Collectible),
            Large          = Flags.HasFlag(Effects.Large),
            X              = _x,
            Y              = _y,
            Z              = _z,
            Rotation       = _rotation,
            Id             = Id,
            WorldId        = _worldId,
        };

    private static ushort From2Bytes(ReadOnlySpan<byte> bytes, int from)
        => (ushort)(bytes[from] | (bytes[from + 1] << 8));

    private static uint From4Bytes(ReadOnlySpan<byte> bytes, int from)
        => (uint)(bytes[from] | (bytes[from + 1] << 8) | (bytes[from + 2] << 16) | (bytes[from + 3] << 24));

    private bool VerifyData()
    {
        var ts = TimeStamp;
        if (ts < TimeStamp.Epoch || ts > GatherBuddy.Time.ServerTime)
            return false;

        if (_bait != 0 && Bait.Equals(Bait.Unknown))
            return false;

        if (_catch != 0 && Catch == null)
            return false;

        if (_fishingSpot != 0 && FishingSpot == null)
            return false;

        if ((Flags & ~ValidEffects) != 0)
            return false;

        if ((_tugAndHook & 0x0F) > 4 || ((_tugAndHook >> 4) > 7))
            return false;

        return true;
    }

    public bool VerifyValidity()
    {
        if (!Flags.HasFlag(Effects.Valid))
            return false;

        if (_bait == 0
         || TimeStamp <= TimeStamp.Epoch
         || TimeStamp > GatherBuddy.Time.ServerTime
         || Bite is < MinBiteTime or > MaxBiteTime
         || Gathering == 0
         || _fishingSpot == 0)
            return false;

        if ((Flags & (Effects.AmbitiousLure1 | Effects.AmbitiousLure2)) != 0 && (Flags & (Effects.ModestLure1 | Effects.ModestLure2)) != 0)
            return false;

        if (Flags.HasValidLure() && !Flags.HasLure())
            return false;

        if (_catch == 0 && (Amount > 0 || Size != 0 || Flags.HasFlag(Effects.Collectible | Effects.Large)))
            return false;

        if (_catch != 0 && (Amount == 0 || Size == 0 || Tug is BiteType.None or BiteType.Unknown || Hook is HookSet.None or HookSet.Unknown))
            return false;

        if (!Flags.HasFlag(Effects.Patience) && !Flags.HasFlag(Effects.Patience2) && Hook is HookSet.Powerful or HookSet.Precise)
            return false;

        if (Amount > 1 && Hook is not (HookSet.DoubleHook or HookSet.TripleHook))
            return false;

        if (_catch != 0 && Flags.HasFlag(Effects.PrizeCatch) && !Flags.HasFlag(Effects.Large))
            return false;
        if (Flags.HasFlag(Effects.Patience) && Flags.HasFlag(Effects.Patience2))
            return false;

        return true;
    }


    public static bool FromBytesV1(ReadOnlySpan<byte> bytes, int from, out FishRecord record)
    {
        record = new FishRecord();
        if (bytes.Length < from + Version1ByteLength)
            return false;

        record._bait         = From4Bytes(bytes, from);
        record._catch        = From4Bytes(bytes, from += 4);
        record.ContentIdHash = (int)From4Bytes(bytes,     from += 4);
        record._timeStamp    = (int)From4Bytes(bytes,     from += 4);
        record.Flags         = (Effects)From4Bytes(bytes, from += 4);
        record.Bite          = From2Bytes(bytes, from += 4);
        record.Perception    = From2Bytes(bytes, from += 2);
        record.Gathering     = From2Bytes(bytes, from += 2);
        record.Size          = From2Bytes(bytes, from += 2);
        record._fishingSpot  = From2Bytes(bytes, from += 2);
        record._tugAndHook   = bytes[from             += 2];
        record.Amount        = bytes[from             += 1];
        return record.VerifyData();
    }
}
