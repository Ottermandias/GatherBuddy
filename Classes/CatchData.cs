using System.Linq;
using GatherBuddy.Enums;
using GatherBuddy.Game;
using GatherBuddy.Managers;
using static GatherBuddy.Classes.Uptime;

namespace GatherBuddy.Classes
{
    public class CatchData
    {
        public CatchData(Patch patch = 0)
            => Patch = patch;

        internal CatchData Transition(params uint[] values)
        {
            PreviousWeather = values;
            return this;
        }

        internal CatchData Weather(params uint[] values)
        {
            CurrentWeather = values;
            return this;
        }

        internal CatchData Bait(FishManager fish, params uint[] values)
        {
            if (values.Length > 0)
                InitialBait = fish.Bait[values[0]];
            if (values.Length > 1)
                Mooches = values.Skip(1).Select(f => fish.Fish[f]).ToArray();
            return this;
        }

        internal CatchData Predators(FishManager fish, params (uint, int)[] values)
        {
            if (values.Length > 0)
                Predator = values.Select(f => (fish.Fish[f.Item1], f.Item2)).ToArray();
            return this;
        }

        internal CatchData Uptime(uint startHour, uint endHour)
        {
            Hours = FromHours(startHour, endHour);
            return this;
        }

        internal CatchData Snag(Snagging value)
        {
            Snagging = value;
            if (value != Snagging.None)
                GigHead = GigHead.None;

            return this;
        }

        internal CatchData HookType(HookSet hook)
        {
            HookSet = hook;
            if (hook != HookSet.None)
                GigHead = GigHead.None;

            return this;
        }

        internal CatchData Tug(BiteType bite)
        {
            BiteType = bite;
            return this;
        }

        internal CatchData Gig(GigHead gigHead)
        {
            GigHead = gigHead;
            if (gigHead != GigHead.None)
            {
                Snagging    = Snagging.None;
                HookSet     = HookSet.None;
                InitialBait = Game.Bait.Unknown;
                Mooches     = new Fish[0];
            }

            return this;
        }

        public uint[] PreviousWeather { get; private set; } = new uint[0];
        public uint[] CurrentWeather  { get; private set; } = new uint[0];

        public Bait   InitialBait { get; private set; } = Game.Bait.Unknown;
        public Fish[] Mooches     { get; private set; } = new Fish[0];

        public (Fish, int)[] Predator { get; private set; } = new (Fish, int)[0];

        public Uptime Hours { get; private set; } = AllHours;

        public Patch    Patch    { get; }
        public Snagging Snagging { get; private set; } = Snagging.Unknown;
        public HookSet  HookSet  { get; private set; } = HookSet.Unknown;
        public GigHead  GigHead  { get; private set; } = GigHead.Unknown;
        public BiteType BiteType { get; private set; } = BiteType.Unknown;
    }
}
