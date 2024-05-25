using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GatherBuddy.AutoGather
{
  public class AutoGatherConfig
  {
    public float MountUpDistance { get; set; } = 15.0f;
    public uint AutoGatherMountId { get; set; } = 1;
    public Dictionary<uint, List<Vector3>> BlacklistedNodesByTerritoryId { get; set; } = new();

    public ActionConfig BYIIConfig { get; set; } = new(true, 100, uint.MaxValue);
    public ActionConfig LuckConfig { get; set; } = new(true, 200, uint.MaxValue);
    public bool DoGathering { get; set; } = true;
    public float NavResetCooldown { get; set; } = 3.0f;
    public float NavResetThreshold { get; set; } = 2.0f;
    public bool ForceWalking { get; set; } = false;

    public class ActionConfig
    {
      public ActionConfig(bool useAction, uint minGP, uint maximumGP)
      {
        UseAction = useAction;
        MinimumGP = minGP;
        MaximumGP = maximumGP;
      }
      public bool UseAction { get; set; }
      public uint MinimumGP { get; set; }
      public uint MaximumGP { get; set; }
    }
  }
}
