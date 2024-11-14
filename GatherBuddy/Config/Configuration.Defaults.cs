using System.Collections.Generic;
using System.Linq;
using Dalamud.Game.ClientState.Keys;
using OtterGui;
using Lumina.Excel.Sheets;

namespace GatherBuddy.Config;

public partial class Configuration
{
    public static readonly VirtualKey[] ValidKeys = Dalamud.Keys.GetValidVirtualKeys().Prepend(VirtualKey.NO_KEY).ToArray();

    public const string DefaultIdentifiedGatherableFormat = "Identified {Item} for \"{Input}\".";
    public const string DefaultAlarmFormat                = "{Alarm} {Item} {DelayString} at {Location}.";

    public static readonly Dictionary<int, uint> ForegroundColors = Dalamud.GameData.GetExcelSheet<UIColor>()!
        .Where(c => (c.UIForeground & 0xFF) > 0)
        .ToDictionary(c => (int)c.RowId, c => Functions.ReorderColor(c.UIForeground));

    public const int DefaultSeColorNames     = 504;
    public const int DefaultSeColorCommands  = 31;
    public const int DefaultSeColorArguments = 546;
    public const int DefaultSeColorAlarm     = 518;
}
