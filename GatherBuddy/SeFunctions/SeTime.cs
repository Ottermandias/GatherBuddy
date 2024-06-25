using System;
using Dalamud.Plugin.Services;
using FFXIVClientStructs.FFXIV.Client.System.Framework;
using GatherBuddy.Time;
using Action = System.Action;

namespace GatherBuddy.SeFunctions;

public class SeTime
{
    private static TimeStamp GetServerTime()
        => new(Framework.GetServerTime() * 1000);

    public TimeStamp ServerTime         { get; private set; }
    public TimeStamp EorzeaTime         { get; private set; }
    public long      EorzeaTotalMinute  { get; private set; }
    public long      EorzeaTotalHour    { get; private set; }
    public short     EorzeaMinuteOfDay  { get; private set; }
    public byte      EorzeaHourOfDay    { get; private set; }
    public byte      EorzeaMinuteOfHour { get; private set; }

    public event Action? Updated;
    public event Action? HourChanged;
    public event Action? WeatherChanged;

    public SeTime()
    {
        Update(null!);
        Dalamud.Framework.Update += Update;
    }

    public void Dispose()
        => Dalamud.Framework.Update -= Update;

    private unsafe TimeStamp GetEorzeaTime()
    {
        var framework = Framework.Instance();
        if (framework == null)
            return ServerTime.ConvertToEorzea();

        return Math.Abs(new TimeStamp(framework->UtcTime.Timestamp * 1000) - ServerTime) < 5000
            ? new TimeStamp(framework->ClientTime.EorzeaTime * 1000)
            : ServerTime.ConvertToEorzea();
    }

    private void Update(IFramework _)
    {
        ServerTime = GetServerTime();
        EorzeaTime = GetEorzeaTime();
        var minute = EorzeaTime.TotalMinutes;
        if (minute != EorzeaTotalMinute)
        {
            EorzeaTotalMinute  = minute;
            EorzeaMinuteOfDay  = (short)(EorzeaTotalMinute % RealTime.MinutesPerDay);
            EorzeaMinuteOfHour = (byte)(EorzeaMinuteOfDay % RealTime.MinutesPerHour);
        }

        var hour = EorzeaTotalMinute / RealTime.MinutesPerHour;
        if (hour != EorzeaTotalHour)
        {
            // Sometimes the Eorzea time gets seemingly rounded up and triggers before the ServerTime.
            //ServerTime      = ServerTime.AddEorzeaMinutes(30).SyncToEorzeaHour();
            EorzeaTotalHour = hour;
            EorzeaHourOfDay = (byte)(EorzeaMinuteOfDay / RealTime.MinutesPerHour);
            HourChanged?.Invoke();
            if ((EorzeaHourOfDay & 0b111) == 0)
            {
                GatherBuddy.Log.Verbose($"Eorzea Hour and Weather Change triggered. {(long)ServerTime} {(long)EorzeaTime}");
                WeatherChanged?.Invoke();
            }
            else
            {
                GatherBuddy.Log.Verbose($"Eorzea Hour Change triggered. {(long)ServerTime} {(long)EorzeaTime}");
            }
        }

        Updated?.Invoke();
    }
}
