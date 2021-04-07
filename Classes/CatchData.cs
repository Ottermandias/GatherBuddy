using System;
using System.Collections.Generic;
using GatherBuddy.Managers;

namespace GatherBuddy.Classes
{
    public class CatchData
    {
        public CatchData(byte patchMajor, byte patchMinor, uint startHour = 0, uint endHour = 24)
        {
            PatchMajor = patchMajor;
            PatchMinor = patchMinor;
            SetHours(startHour, endHour);
        }

        public List<uint> PreviousWeather { get; set; } = new();
        public List<uint> CurrentWeather  { get; set; } = new();
        public List<uint> BaitOrder       { get; set; } = new();

        public (uint, int) Predator { get; set; } = (0, 0);

        public uint   Folklore { get; set; }         = 0;
        public Uptime Hours    { get; private set; } = Uptime.AllHours;

        public void SetHours(uint startHour, uint endHour)
            => Hours = Uptime.FromHours(startHour, endHour);

        public byte PatchMajor { get; set; } = 2;
        public byte PatchMinor { get; set; } = 0;
        public bool Snagging   { get; set; } = false;
    }
}
