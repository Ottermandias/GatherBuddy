using GatherBuddy.Utility;
using Lumina.Excel.Sheets;

namespace GatherBuddy.Classes;

public class CosmicMission
{
    public ushort Id
        => (ushort)Data.RowId;

    public WKSMissionUnit Data;
    public string         Name;

    public CosmicMission(WKSMissionUnit data)
    {
        Data = data;
        Name = MultiString.ParseSeStringLumina(Data.Unknown0);
    }
}
