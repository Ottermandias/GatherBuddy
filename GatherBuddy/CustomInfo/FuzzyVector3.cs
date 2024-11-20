using GatherBuddy.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ECommons.GameHelpers;

namespace GatherBuddy.CustomInfo
{
    public static class VectorExtensions
    {
        public static float DistanceToPlayer(this Vector3 vector)
        {
            var distance = Vector3.Distance(vector, Player.Object.Position);
            return distance;
        }
    }
}
