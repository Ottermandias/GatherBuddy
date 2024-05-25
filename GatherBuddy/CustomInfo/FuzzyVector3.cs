using GatherBuddy.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

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
        public static Vector3 CorrectForMesh(this Vector3 vector)
        {
            try
            {
                try
                {
                    var nearestPoint = VNavmesh_IPCSubscriber.Query_Mesh_PointOnFloor(vector, 0.5f, 0.5f);
                    GatherBuddy.Log.Verbose($"Corrected vector {vector} to floor point {nearestPoint}");
                    return nearestPoint;
                }
                catch (Exception)
                {
                    var nearestPoint = VNavmesh_IPCSubscriber.Query_Mesh_NearestPoint(vector, 0.5f, 0.5f);
                    GatherBuddy.Log.Verbose($"Corrected vector {vector} to {nearestPoint}");
                    return nearestPoint;
                }
            }
            catch (Exception)
            {
                GatherBuddy.Log.Debug("Failed to correct for mesh, returning original vector.");
                return vector.Fuzz();
            }
        }
    }
}
