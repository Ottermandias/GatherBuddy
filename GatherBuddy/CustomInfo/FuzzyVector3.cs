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
        public static Vector3 Fuzz(this Vector3 value, float fuzziness = 0.2f)
        {
            var random = new Random();
            var vector = new Vector3(
                value.X + (float)random.NextDouble() * fuzziness,
                value.Y + (float)random.NextDouble() * fuzziness,
                value.Z + (float)random.NextDouble() * fuzziness
            );
            GatherBuddy.Log.Verbose($"Fuzzed vector {value} to {vector}");
            return vector;
        }

        public static Vector3 CorrectForMesh(this Vector3 vector, float distance)
        {
            try
            {
                try
                {
                    var nearestPoint = VNavmesh_IPCSubscriber.Query_Mesh_PointOnFloor(vector, false, distance);
                    if (!nearestPoint.SanityCheck())
                        return vector;

                    GatherBuddy.Log.Verbose($"Corrected vector {vector} to floor point {nearestPoint}");
                    return nearestPoint;
                }
                catch (Exception)
                {
                    var nearestPoint = VNavmesh_IPCSubscriber.Query_Mesh_NearestPoint(vector, distance, distance);
                    if (!nearestPoint.SanityCheck())
                        return vector;

                    GatherBuddy.Log.Verbose($"Corrected vector {vector} to {nearestPoint}");
                    return nearestPoint;
                }
            }
            catch (Exception)
            {
                GatherBuddy.Log.Debug("Failed to correct for mesh, returning original vector.");
                return vector;
            }
        }

        public static bool SanityCheck(this Vector3 vector)
        {
            if (vector.X == 0)
                return false;
            if (vector.Y == 0)
                return false;
            if (vector.Z == 0)
                return false;

            return true;
        }

        public static float DistanceToPlayer(this Vector3 vector)
        {
            var distance = Vector3.Distance(vector, Player.Object.Position);
            return distance;
        }
    }
}
