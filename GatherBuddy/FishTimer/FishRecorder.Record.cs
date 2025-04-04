using System;
using System.Diagnostics;
using System.Linq;
using Dalamud.Game;
using Dalamud.Plugin.Services;
using GatherBuddy.Classes;
using GatherBuddy.Enums;
using GatherBuddy.FishTimer.Parser;
using GatherBuddy.SeFunctions;
using GatherBuddy.Structs;
using GatherBuddy.Time;
using Lumina.Excel.Sheets;
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

    public readonly   FishingParser Parser;
    internal          CatchSteps    Step      = 0;
    internal          FishingState  LastState = FishingState.None;
    internal readonly Stopwatch     Timer     = new();
    internal readonly Stopwatch     LureTimer     = new(); // Timer
    private byte hasLureEffect = 0; // For checking new lures. Init to default fishing state. 
    private byte oldEffect = 9; // For checking new lures. Init to default fishing state. 

    public Fish? LastCatch;

    public FishRecord Record;

    private static Bait GetCurrentBait()
    {
        if (GatherBuddy.EventFramework.CurrentSwimBait is { } fishId)
        {
            if (GatherBuddy.GameData.Fishes.TryGetValue(fishId, out var fish))
                return new Bait(fish.ItemData);

            GatherBuddy.Log.Error($"Item with id {fishId} is not a known type of fish to be used as bait.");
            return Bait.Unknown;
        }

        var baitId = GatherBuddy.CurrentBait.Current;
        if (GatherBuddy.GameData.Bait.TryGetValue(baitId, out var bait))
            return bait;

        GatherBuddy.Log.Error($"Item with id {baitId} is not a known type of bait.");
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
                3907 => FishRecord.Effects.BigGameFishing,
                850  => FishRecord.Effects.Patience,
                765  => FishRecord.Effects.Patience2,
                _    => FishRecord.Effects.None,
            };
        }

        if (Record.Flags.HasFlag(FishRecord.Effects.Patience)
         && Record.Flags.HasFlag(FishRecord.Effects.Patience2))
            Record.Flags &= ~FishRecord.Effects.Patience;
    }

    private void UpdateLure()
    {
        if (Dalamud.ClientState.LocalPlayer?.StatusList == null)
            return;

        LureTimer.Stop(); // When we bite and update lure, stop the timer.
        foreach (var buff in Dalamud.ClientState.LocalPlayer.StatusList)
        {
            Record.Flags |= buff.StatusId switch
            {
                3972 when buff.Param == 1 => FishRecord.Effects.AmbitiousLure1,
                3972 when buff.Param == 2 => FishRecord.Effects.AmbitiousLure2,
                3972 when buff.Param == 3 => FishRecord.Effects.AmbitiousLure1 | FishRecord.Effects.AmbitiousLure2,
                3973 when buff.Param == 1 => FishRecord.Effects.ModestLure1,
                3973 when buff.Param == 2 => FishRecord.Effects.ModestLure2,
                3973 when buff.Param == 3 => FishRecord.Effects.ModestLure1 | FishRecord.Effects.ModestLure2,
                _                              => FishRecord.Effects.None,
            };
        }
    }

    private static readonly uint GatheringIdx =
        Dalamud.GameData.GetExcelSheet<BaseParam>(ClientLanguage.English).Cast<BaseParam?>()
            .FirstOrDefault(r => r!.Value.Name == "Gathering")?.RowId
     ?? 72;

    private static readonly uint PerceptionIdx =
        Dalamud.GameData.GetExcelSheet<BaseParam>(ClientLanguage.English).Cast<BaseParam?>()
            .FirstOrDefault(r => r!.Value.Name == "Perception")?.RowId
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
        Record.Gathering     = (ushort)uiState->PlayerState.Attributes[(int)GatheringIdx];
        Record.Perception    = (ushort)uiState->PlayerState.Attributes[(int)PerceptionIdx];
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
        LureTimer.Reset(); // Reset
        oldEffect = 0;
        hasLureEffect = 0;
    }

    private void SubscribeToParser()
    {
        Parser.BeganFishing   += OnBeganFishing;
        Parser.BeganMooching  += OnMooch;
        Parser.CaughtFish     += OnCatch;
        Parser.IdentifiedSpot += OnIdentification;
        Parser.HookedIn       += OnHooking;
        // Parser.UsedLure        += OnLure; // Where I would put my event if it worked right :c
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

        GatherBuddy.Log.Verbose($"Began fishing at {spot?.Name ?? "Undiscovered Fishing Hole"} using {Record.Bait.Name}.");
    }

    private void OnBite()
    {
        Timer.Stop();
        UpdateLure();
        Record.SetTugHook(GatherBuddy.TugType.Bite, Record.Hook);
        Step |= CatchSteps.FishBit;
        if (LureTimer.ElapsedMilliseconds > 0) 
            GatherBuddy.Log.Verbose($"Fish bit with {Record.Tug} after {Timer.ElapsedMilliseconds} ms. Time since last lure: {LureTimer.ElapsedMilliseconds} ms.");
        else
            GatherBuddy.Log.Verbose($"Fish bit with {Record.Tug} after {Timer.ElapsedMilliseconds} ms.");
    }

    private void OnIdentification(FishingSpot spot)
    {
        Record.FishingSpot =  spot;
        Step               |= CatchSteps.IdentifiedSpot;
        GatherBuddy.Log.Verbose($"Identified previously unknown fishing spot as {spot.Name}.");
    }

    private void OnHooking(HookSet hook)
    {
        Record.SetTugHook(Record.Tug, hook);
        GatherBuddy.Log.Verbose($"Hooking {Record.Tug} tug with {hook}.");
    }

    // Code to run whenever a lure is used... Ideally would be hooked from gaining the status event.
    private void OnLure() 
    {
        LureTimer.Restart();
        GatherBuddy.Log.Verbose($"Restarting lure timer!");
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
        GatherBuddy.Log.Verbose(
            $"Caught {amount} {(large ? "large " : string.Empty)}{(collectible ? "collectible " : string.Empty)}{Record.Catch.Name[ClientLanguage.English]} of size {size / 10f:F1}.");
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
        GatherBuddy.Log.Verbose($"Mooching with {Record.Bait.Name} at {spot?.Name ?? "Undiscovered Fishing Hole"}.");
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
        if((ushort)Math.Clamp(LureTimer.ElapsedMilliseconds, 0, Timer.ElapsedMilliseconds) >= 5200)
            Record.Flags |= FishRecord.Effects.ValidLure;
        UpdateLure();
        if (!Record.VerifyValidity())
            Record.Flags &= ~FishRecord.Effects.Valid;

        Step = CatchSteps.None;
        if (GatherBuddy.Config.StoreFishRecords)
            Add(Record);
    }

    private void OnFrameworkUpdate(IFramework _)
    {
        TimedSave();
        var state = GatherBuddy.EventFramework.FishingState;
        if (LastState == state)
            return;

        LastState = state;

        //There has to be a better way of checking this..... Presumably with Action/Events, but I'm unsure how I could create an event to fire when I gain a buff.
        { // Check if I have a lure buff, then check if it is different from the last one. If I have a new lure buff, run OnLure() code. 
            {
                foreach (var buff in Dalamud.ClientState.LocalPlayer.StatusList)
                {
                    hasLureEffect = buff.StatusId switch
                    {
                        3972 => (byte)buff.Param,
                        3973 => (byte)buff.Param,
                        _    => hasLureEffect,
                    };
                }
                // GatherBuddy.Log.Information($"{oldEffect} | {hasLureEffect}"); //There's a single frame where hasLureEffect is both Lure1 and Lure2 when first gaining the Lure2 status
                if (oldEffect != hasLureEffect)
                {
                    if (hasLureEffect != 0)
                        OnLure();
                    oldEffect = hasLureEffect;
                }
            }
        }

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
