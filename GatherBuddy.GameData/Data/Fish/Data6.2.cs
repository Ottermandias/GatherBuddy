using GatherBuddy.Enums;

namespace GatherBuddy.Data;

public static partial class Fish
{
    // @formatter:off
    private static void ApplyBuriedMemory(this GameData data)
    {
        data.Apply(37697, Patch.BuriedMemory) // Mayashell
            .Bait(data, 36591, 36447)
            .Bite(HookSet.Powerful, BiteType.Strong)
            .Time(180, 360); 
        data.Apply(37845, Patch.BuriedMemory) // Greatsword Snook
            .Bait(data, 36591)
            .Bite(HookSet.Unknown, BiteType.Legendary)
            .Time(960, 1440)
            .Transition(data, 2, 3, 15)
            .Weather(data, 15);
        data.Apply(37846, Patch.BuriedMemory) // Swampsucker Bowfin
            .Bait(data, 36590)
            .Bite(HookSet.Unknown, BiteType.Legendary)
            .Time(480, 960)
            .Transition(data, 1, 2)
            .Weather(data, 3);
        data.Apply(37847, Patch.BuriedMemory) // Lale Crab
            .Bait(data, 36592)
            .Bite(HookSet.Powerful, BiteType.Legendary)
            .Transition(data, 1, 2)
            .Weather(data, 4);
        data.Apply(37848, Patch.BuriedMemory) // Bigcuda
            .Bait(data, 36590)
            .Bite(HookSet.Unknown, BiteType.Legendary)
            .Time(480, 960)
            .Transition(data, 2)
            .Weather(data, 7);
        data.Apply(37849, Patch.BuriedMemory) // Sovereign Shadow
            .Bait(data, 36591)
            .Bite(HookSet.Unknown, BiteType.Legendary)
            .Time(0, 480)
            .Transition(data, 3)
            .Weather(data, 15);
        data.Apply(37850, Patch.BuriedMemory) // Disappirarucu
           .Bait(data, 36591)
           .Bite(HookSet.Unknown, BiteType.Legendary)
           .Time(0, 480);
        data.Apply(37851, Patch.BuriedMemory) // Starscryer
           .Bait(data, 36591)
           .Bite(HookSet.Unknown, BiteType.Legendary)
           .Time(120, 240)
           .Transition(data, 2, 149)
           .Weather(data, 149);
        data.Apply(37852, Patch.BuriedMemory) // Argonauta argo
           .Bait(data, 36597)
           .Bite(HookSet.Unknown, BiteType.Legendary)
           .Time(120, 240)
           .Transition(data, 2, 49)
           .Weather(data, 49);
        data.Apply(37853, Patch.BuriedMemory) // Planetes
           .Bait(data, 29717, 36478)
           .Bite(HookSet.Powerful, BiteType.Legendary)
           .Weather(data, 49);
    }
    // @formatter:on
}
