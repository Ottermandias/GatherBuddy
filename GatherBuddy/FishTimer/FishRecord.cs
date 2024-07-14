using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using GatherBuddy.Classes;
using GatherBuddy.Enums;
using GatherBuddy.Structs;
using GatherBuddy.Time;

namespace GatherBuddy.FishTimer;

public static class EffectsExtensions
{
    public static byte AmbitiousLure(this FishRecord.Effects effects)
        => (effects & (FishRecord.Effects.AmbitiousLure1 | FishRecord.Effects.AmbitiousLure2)) switch
        {
            0                                 => 0,
            FishRecord.Effects.AmbitiousLure1 => 1,
            FishRecord.Effects.AmbitiousLure2 => 2,
            _                                 => 3,
        };

    public static byte ModestLure(this FishRecord.Effects effects)
        => (effects & (FishRecord.Effects.ModestLure1 | FishRecord.Effects.ModestLure2)) switch
        {
            0                              => 0,
            FishRecord.Effects.ModestLure1 => 1,
            FishRecord.Effects.ModestLure2 => 2,
            _                              => 3,
        };

    public const FishRecord.Effects Lures = FishRecord.Effects.ModestLure1
      | FishRecord.Effects.ModestLure2
      | FishRecord.Effects.AmbitiousLure1
      | FishRecord.Effects.AmbitiousLure2;

    public static bool HasLure(this FishRecord.Effects effects)
        => (effects & Lures) != 0;
}

public struct FishRecord
{
    public const ushort MinBiteTime        = 1000;
    public const ushort MaxBiteTime        = 65000;
    public const byte   Version            = 1;
    public const int    Version1ByteLength = 4 + 4 + 4 + 4 + 4 + 2 + 2 + 2 + 2 + 2 + 1 + 1;
    public const int    ByteLength         = Version1ByteLength;

    public static readonly Effects ValidEffects = Enum.GetValues<Effects>().Aggregate((a, b) => a | b);

    [Flags]
    public enum Effects : uint
    {
        None           = 0x00000000,
        Snagging       = 0x00000001,
        Chum           = 0x00000002,
        Intuition      = 0x00000004,
        FishEyes       = 0x00000008,
        IdenticalCast  = 0x00000010,
        SurfaceSlap    = 0x00000020,
        PrizeCatch     = 0x00000040,
        Patience       = 0x00000080,
        Patience2      = 0x00000100,
        Collectible    = 0x00002000,
        Large          = 0x00004000,
        Valid          = 0x00008000,
        Legacy         = 0x00010000,
        BigGameFishing = 0x00020000,
        AmbitiousLure1 = 0x00040000,
        AmbitiousLure2 = 0x00080000,
        ModestLure1    = 0x00100000,
        ModestLure2    = 0x00200000,
    }

    private uint    _bait;
    private uint    _catch;
    public  int     ContentIdHash;
    private int     _timeStamp;
    public  Effects Flags;
    public  ushort  Bite;
    public  ushort  Perception;
    public  ushort  Gathering;
    public  ushort  Size;
    private ushort  _fishingSpot;
    private byte    _tugAndHook;
    public  byte    Amount;

    public TimeStamp TimeStamp
    {
        get => new(_timeStamp * 1000L);
        set => _timeStamp = (int)(value.Time / 1000);
    }

    public FishingSpot? FishingSpot
    {
        get => HasSpot ? GatherBuddy.GameData.FishingSpots.GetValueOrDefault(_fishingSpot) : null;
        set => _fishingSpot = (ushort)(value?.Id ?? 0);
    }

    public bool HasSpot
        => _fishingSpot != 0;

    public Bait Bait
    {
        get => HasBait
            ? GatherBuddy.GameData.Bait.TryGetValue(_bait, out var b) ? b :
            GatherBuddy.GameData.Fishes.TryGetValue(_bait, out var f) ? new Bait(f.ItemData) : Bait.Unknown
            : Bait.Unknown;
        set => _bait = value.Id;
    }

    public bool HasBait
        => _bait != 0;

    public Fish? Catch
    {
        get => HasCatch ? GatherBuddy.GameData.Fishes.GetValueOrDefault(_catch) : null;
        set => _catch = value?.ItemId ?? 0;
    }

    public uint CatchId
        => _catch;

    public uint BaitId
        => _bait;

    public ushort SpotId
        => _fishingSpot;

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
            _                  => 6 << 4,
        };
        _tugAndHook = (byte)b;
    }

    public BiteType Tug
        => (_tugAndHook & 0x0F) switch
        {
            0 => BiteType.None,
            1 => BiteType.Weak,
            2 => BiteType.Strong,
            3 => BiteType.Legendary,
            _ => BiteType.Unknown,
        };

    public HookSet Hook
        => (_tugAndHook >> 4) switch
        {
            0 => HookSet.None,
            1 => HookSet.Hook,
            2 => HookSet.Precise,
            3 => HookSet.Powerful,
            4 => HookSet.DoubleHook,
            5 => HookSet.TripleHook,
            _ => HookSet.Unknown,
        };

    public bool Escaped()
        => Hook != HookSet.None && Tug != BiteType.None;

    public bool MissedChance()
        => Tug != BiteType.None && Hook == HookSet.None;

    public bool NothingHooked()
        => Hook == HookSet.None && Tug != BiteType.None;

    public unsafe void ToBytes(byte[] bytes, int from)
    {
        if (bytes.Length < from + ByteLength)
            throw new ArgumentException("Not enough storage");

        fixed (FishRecord* ptr = &this)
        {
            Marshal.Copy((IntPtr)ptr, bytes, from, ByteLength);
        }
    }

    internal struct JsonStruct
    {
        public uint     ContentIdHash;
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
    }

    internal JsonStruct ToJson()
        => new()
        {
            ContentIdHash  = (uint)ContentIdHash,
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
            BiteTime       = Bite,
            Tug            = Tug,
            HookSet        = Hook,
            CatchItemId    = CatchId,
            Amount         = Amount,
            Size           = Size / 10f,
            Collectible    = Flags.HasFlag(Effects.Collectible),
            Large          = Flags.HasFlag(Effects.Large),
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

        if ((_tugAndHook & 0x0F) > 4 || _tugAndHook >> 4 > 6)
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
         || ContentIdHash == 0
         || _fishingSpot == 0)
            return false;

        if ((Flags & (Effects.AmbitiousLure1 | Effects.AmbitiousLure2)) != 0 && (Flags & (Effects.ModestLure1 | Effects.ModestLure2)) != 0)
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
