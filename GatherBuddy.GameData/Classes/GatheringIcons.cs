using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using Dalamud.Plugin.Services;
using Lumina.Excel.GeneratedSheets;

namespace GatherBuddy.Classes;

public class GatheringIcons
{
    private readonly IReadOnlyDictionary<Enums.GatheringType, (uint, uint)> _icons;

    public GatheringIcons(IDataManager gameData)
    {
        var sheet = gameData.GetExcelSheet<GatheringType>()!;
        var icons = new Dictionary<Enums.GatheringType, (uint, uint)>(Enum.GetValues<Enums.GatheringType>().Length - 2)
        {
            [Enums.GatheringType.Mining]       = ((uint)sheet.GetRow(0)!.IconMain, (uint)sheet.GetRow(0)!.IconOff),
            [Enums.GatheringType.Quarrying]    = ((uint)sheet.GetRow(1)!.IconMain, (uint)sheet.GetRow(1)!.IconOff),
            [Enums.GatheringType.Logging]      = ((uint)sheet.GetRow(2)!.IconMain, (uint)sheet.GetRow(2)!.IconOff),
            [Enums.GatheringType.Harvesting]   = ((uint)sheet.GetRow(3)!.IconMain, (uint)sheet.GetRow(3)!.IconOff),
            [Enums.GatheringType.Spearfishing] = ((uint)sheet.GetRow(4)!.IconMain, (uint)sheet.GetRow(4)!.IconOff),
            [Enums.GatheringType.Fisher]       = (60465, 60466),
        };
        icons[Enums.GatheringType.Miner]    = icons[Enums.GatheringType.Mining];
        icons[Enums.GatheringType.Botanist] = icons[Enums.GatheringType.Logging];
        _icons                              = icons.ToFrozenDictionary();
    }

    public (uint, uint) this[Enums.GatheringType val]
        => _icons[val];
}
