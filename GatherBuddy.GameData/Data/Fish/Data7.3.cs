using GatherBuddy.Enums;

namespace GatherBuddy.Data;

public static partial class Fish
{
    // @formatter:off
    private static void ApplyThePromiseOfTomorrow(this GameData data)
    {
        data.Apply(46188, Patch.ThePromiseOfTomorrow) // Goldentail
            .Bait(data, 43858)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(46189, Patch.ThePromiseOfTomorrow) // Prime Adjudicator
            .Bait(data, 43858)
            .Time(480, 960)
            .Transition(data, 2)
            .Weather(data, fog) //placeholder
            .Bite(data, HookSet.Unknown, BiteType.Legendary);
        data.Apply(46190, Patch.ThePromiseOfTomorrow) // Crenicichla Miyaka
            .Bait(data, 43858)
            .Time(360, 480)
            .Transition(data, 1, 2)
            .Weather(data, rain) //placeholder
            .Lure(Enums.Lure.Ambitious)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);
        data.Apply(46191, Patch.ThePromiseOfTomorrow) // Iron Shadowtongue
            .Bait(data, 43858)
            .Time(960, 1080)
            .Transition(data, 2, 3, 4)
            .Weather(data, rain) //placeholder
            .Bite(data, HookSet.Powerful, BiteType.Legendary);
        data.Apply(46192, Patch.ThePromiseOfTomorrow) // Lotl-in-waiting
            .Bait(data)
            .Bite(data, HookSet.Unknown, BiteType.Unknown);
        data.Apply(46193, Patch.ThePromiseOfTomorrow) // Azure Diver
            .Bait(data, 43857)
            .Time(1080, 1440)
            .Transition(data, 1, 2)
            .Weather(data, gales) //placeholder
            .Bite(data, HookSet.Precise, BiteType.Legendary);
        data.Apply(46194, Patch.ThePromiseOfTomorrow) // Sprouting Perch
            .Bait(data, 43858)
            .Time(1200, 1440)
            .Transition(data, clouds, umbralstatic) //placeholder
            .Weather(data, thunderstorms) //placeholder
            .Bite(data, HookSet.Powerful, BiteType.Legendary);
        data.Apply(46195, Patch.ThePromiseOfTomorrow) // Gigagiant Snakehead
            .Bait(data, 43858)
            .Time(240, 360)
            .Predators(data, 350, (43781, 3))
            .Transition(data, 2, clouds) //placeholder
            .Weather(data, rain) //placeholder
            .Bite(data, HookSet.Unknown, BiteType.Legendary);
        data.Apply(46196, Patch.ThePromiseOfTomorrow) // Gondola Louvar
            .Bait(data, 43859)
            .Time(480, 720)
            .Transition(data, rain) //placeholder
            .Weather(data, 2)
            .Bite(data, HookSet.Unknown, BiteType.Legendary);
        data.Apply(46249, Patch.ThePromiseOfTomorrow) // Purple Palate
            .Bait(data, 43858)
            .Bite(data, HookSet.Powerful, BiteType.Strong);
        data.Apply(46779, Patch.ThePromiseOfTomorrow) // Sunray Ray
            .Bait(data, 43857)
            .Bite(data, HookSet.Powerful, BiteType.Legendary);
    }
    // @formatter:on
}

