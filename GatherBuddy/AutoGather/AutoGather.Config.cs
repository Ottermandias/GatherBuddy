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
  }
}
