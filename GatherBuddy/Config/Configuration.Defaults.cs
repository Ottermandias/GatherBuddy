using System.Collections.Generic;
using System.Linq;
using ImGuiOtter;
using Lumina.Excel.GeneratedSheets;

namespace GatherBuddy.Config;

public partial class Configuration
{
    public const string DefaultIdentifiedItemFormat = "Identified {Name} for \"{Input}\".";
    public const string DefaultIdentifiedFishFormat = "Identified {Name} for \"{Input}\".";
    public const string DefaultNodeAlarmFormat = "[GatherBuddy][Alarm {Name}]: The gathering node for {AllItems} {DelayString} at {Location}.";

    public const string DefaultFishAlarmFormat =
        "[GatherBuddy][Alarm {Name}]: The fish {FishName} at {FishingSpotName} {DelayString}. Catch with {BaitName}.";

    public static readonly Dictionary<int, uint> ForegroundColors = Dalamud.GameData.GetExcelSheet<UIColor>()!
        .Where(c => (c.UIForeground & 0xFF) > 0)
        .ToDictionary(c => (int)c.RowId, c => ImGuiUtil.ReorderColor(c.UIForeground));

    public const int DefaultSeColorNames     = 504;
    public const int DefaultSeColorCommands  = 31;
    public const int DefaultSeColorArguments = 546;
}
