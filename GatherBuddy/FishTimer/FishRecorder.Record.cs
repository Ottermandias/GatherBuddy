using System;
using System.Diagnostics;
using System.Linq;
using Dalamud;
using Dalamud.Game;
using Dalamud.Logging;
using GatherBuddy.Classes;
using GatherBuddy.Enums;
using GatherBuddy.FishTimer.Parser;
using GatherBuddy.SeFunctions;
using GatherBuddy.Structs;
using GatherBuddy.Time;
using Lumina.Excel.GeneratedSheets;
using FishingSpot = GatherBuddy.Classes.FishingSpot;

namespace GatherBuddy.FishTimer;

public partial class FishRecorder
{
    [Flags]
    internal enum CatchSteps
    {
        None           = 0x00,
        BeganFishing   = 0x01,
        IdentifiedSpot = 0x02,
        FishBit        = 0x04,
        FishCaught     = 0x08,
        Mooch          = 0x10,
        FishReeled     = 0x20,
    }

    public readonly FishingParser Parser    = new();
    internal        CatchSteps    Step      = 0;
    internal        FishingState  LastState = FishingState.None;
    internal        Stopwatch     Timer     = new();

    public Fish? LastCatch;

    public FishRecord Record;

    private static Bait GetCurrentBait()
    {
        var baitId = GatherBuddy.CurrentBait.Current;
        if (GatherBuddy.GameData.Bait.TryGetValue(baitId, out var bait))
            return bait;

        PluginLog.Error("Item with id {Id} is not a known type of bait.", baitId);
        return Bait.Unknown;
    }

    private void CheckBuffs()
    {
        if (Dalamud.ClientState.LocalPlayer?.StatusList == null)
            return;

        foreach (var buff in Dalamud.ClientState.LocalPlayer.StatusList)
        {
            Record.Flags |= buff.StatusId switch
            {
                761  => FishRecord.Effects.Snagging,
                763  => FishRecord.Effects.Chum,
                568  => FishRecord.Effects.Intuition,
                762  => FishRecord.Effects.FishEyes,
                1804 => FishRecord.Effects.IdenticalCast,
                1803 => FishRecord.Effects.SurfaceSlap,
                2780 => FishRecord.Effects.PrizeCatch,
                850  => FishRecord.Effects.Patience,
                765  => FishRecord.Effects.Patience2,
                _    => FishRecord.Effects.None,
            };
        }

        if (Record.Flags.HasFlag(FishRecord.Effects.Patience)
         && Record.Flags.HasFlag(FishRecord.Effects.Patience2))
            Record.Flags &= ~FishRecord.Effects.Patience;
    }

    private static readonly uint GatheringIdx =
        Dalamud.GameData.GetExcelSheet<BaseParam>(ClientLanguage.English)?
            .FirstOrDefault(r => r.Name == "Gathering")?.RowId
     ?? 72;

    private static readonly uint PerceptionIdx =
        Dalamud.GameData.GetExcelSheet<BaseParam>(ClientLanguage.English)?
            .FirstOrDefault(r => r.Name == "Perception")?.RowId
     ?? 73;

    private static int GetContentHash(ulong id)
    {
        var lower = ((id << 5) - id) & 0xFFFFFFFF;
        var upper = ((id << 4) + id) >> 32;
        return (int)((lower & 0x45551555) | ((upper ^ lower) & 0x2AAA8AAA) | 0x10002000);
    }

    private unsafe void CheckStats()
    {
        var uiState = FFXIVClientStructs.FFXIV.Client.Game.UI.UIState.Instance();
        if (uiState == null)
            return;

        Record.ContentIdHash = GetContentHash(uiState->PlayerState.ContentId);
        Record.Gathering     = (ushort)uiState->PlayerState.Attributes[GatheringIdx];
        Record.Perception    = (ushort)uiState->PlayerState.Attributes[PerceptionIdx];
    }


    private void Reset()
    {
        LastCatch = Record.Catch ?? LastCatch;
        Record = new FishRecord()
        {
            Flags = FishRecord.Effects.Valid,
        };
        Step             = CatchSteps.None;
        Record.TimeStamp = TimeStamp.Epoch;
        Timer.Reset();
    }

    private void SubscribeToParser()
    {
        Parser.BeganFishing   += OnBeganFishing;
        Parser.BeganMooching  += OnMooch;
        Parser.CaughtFish     += OnCatch;
        Parser.IdentifiedSpot += OnIdentification;
        Parser.HookedIn       += OnHooking;
    }

    private void OnBeganFishing(FishingSpot? spot)
    {
        Reset();
        Record.TimeStamp = GatherBuddy.Time.ServerTime;
        Timer.Start();
        Step = CatchSteps.BeganFishing;
        CheckBuffs();
        CheckStats();
        Record.Bait        = GetCurrentBait();
        Record.FishingSpot = spot;
        if (Record.HasSpot)
            Step |= CatchSteps.IdentifiedSpot;

        PluginLog.Verbose("Began fishing at {FishingSpot:l} using {Bait:l}.",
            spot?.Name ?? "Undiscovered Fishing Hole", Record.Bait.Name);
    }

    private void OnBite()
    {
        Timer.Stop();
        Record.SetTugHook(GatherBuddy.TugType.Bite, Record.Hook);
        Step |= CatchSteps.FishBit;
        PluginLog.Verbose("Fish bit with {BiteType} after {Milliseconds}.", Record.Tug, Timer.ElapsedMilliseconds);
    }

    private void OnIdentification(FishingSpot spot)
    {
        Record.FishingSpot =  spot;
        Step               |= CatchSteps.IdentifiedSpot;
        PluginLog.Verbose("Identified previously unknown fishing spot as {FishingSpot:l}.", spot.Name);
    }

    private void OnHooking(HookSet hook)
    {
        Record.SetTugHook(Record.Tug, hook);
        PluginLog.Verbose("Hooking {BiteType:l} tug with {HookSet:l}.", Record.Tug, hook);
    }

    private void OnCatch(Fish fish, ushort size, byte amount, bool large, bool collectible)
    {
        Step          |= CatchSteps.FishCaught;
        Record.Catch  =  fish;
        Record.Size   =  size;
        Record.Amount =  amount;
        if (large)
            Record.Flags |= FishRecord.Effects.Large;
        if (collectible)
            Record.Flags |= FishRecord.Effects.Collectible;
        PluginLog.Verbose("Caught {Amount} {Large:l}{Collectible:l}{Fish:l} of size {Size:F1}.",
            amount, large ? "large " : string.Empty, collectible ? "collectible " : string.Empty,
            Record.Catch.Name[ClientLanguage.English], size / 10f);
    }

    private void OnMooch()
    {
        var spot = Record.FishingSpot;
        Reset();
        Record.TimeStamp = GatherBuddy.Time.ServerTime;
        Timer.Start();
        Step = CatchSteps.BeganFishing | CatchSteps.Mooch;
        CheckBuffs();
        CheckStats();
        Record.Bait        = LastCatch != null ? new Bait(LastCatch.ItemData) : GetCurrentBait();
        Record.FishingSpot = spot;
        if (Record.HasSpot)
            Step |= CatchSteps.IdentifiedSpot;
        PluginLog.Verbose("Mooching with {Fish} at {FishingSpot}.",
            Record.Bait.Name,
            spot?.Name ?? "Undiscovered Fishing Hole");
    }

    private void OnFishingStop()
    {
        if (Timer.IsRunning)
        {
            Timer.Stop();
            return;
        }

        if (!Step.HasFlag(CatchSteps.BeganFishing))
            return;

        Record.Bite = (ushort)Math.Clamp(Timer.ElapsedMilliseconds, 0, ushort.MaxValue);
        if (!Record.VerifyValidity())
            Record.Flags &= ~FishRecord.Effects.Valid;

        Step = CatchSteps.None;
        if (GatherBuddy.Config.StoreFishRecords)
            Add(Record);
    }

    private void OnFrameworkUpdate(Framework _)
    {
        var state = GatherBuddy.EventFramework.FishingState;
        if (LastState == state)
            return;

        LastState = state;

        switch (state)
        {
            case FishingState.Bite:
                OnBite();
                break;
            case FishingState.Reeling:
                Step |= CatchSteps.FishReeled;
                break;
            case FishingState.PoleReady:
            case FishingState.Quit:
                OnFishingStop();
                break;
        }
    }
}
