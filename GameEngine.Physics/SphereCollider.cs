// ============================================================================
// SPHERE COLLIDER
// ============================================================================

using System;
using System.Numerics;

namespace GameEngine.Core
{
    // ========================================================================
    // SPHERE COLLIDER
    // ========================================================================
    public class SphereCollider : Collider
    {
        public float Radius { get; set; } = 0.5f;
        public Vector3 Center { get; set; }

        public Vector3 WorldCenter => GameObject.Transform.Position + Center;
        public float WorldRadius => Radius * Math.Max(Math.Max(
            GameObject.Transform.Scale.X,
            GameObject.Transform.Scale.Y),
            GameObject.Transform.Scale.Z);

        public override bool CheckCollision(Collider other, out CollisionInfo info)
        {
            info = null;
            
            if (other is SphereCollider sphere)
            {
                var distance = Vector3.Distance(WorldCenter, sphere.WorldCenter);
                var radiusSum = WorldRadius + sphere.WorldRadius;

                if (distance < radiusSum)
                {
                    var normal = Vector3.Normalize(sphere.WorldCenter - WorldCenter);
                    info = new CollisionInfo
                    {
                        ColliderA = this,
                        ColliderB = sphere,
                        Normal = normal,
                        Penetration = radiusSum - distance,
                        ContactPoint = WorldCenter + normal * WorldRadius
                    };
                    return true;
                }
            }

            return false;
        }

        public override bool Raycast(Vector3 origin, Vector3 direction, out float distance)
        {
            var oc = origin - WorldCenter;
            var a = Vector3.Dot(direction, direction);
            var b = 2.0f * Vector3.Dot(oc, direction);
            var c = Vector3.Dot(oc, oc) - WorldRadius * WorldRadius;
            var discriminant = b * b - 4 * a * c;

            if (discriminant < 0)
            {
                distance = float.MaxValue;
                return false;
            }

            distance = (-b - MathF.Sqrt(discriminant)) / (2.0f * a);
            return distance >= 0;
        }
    }
}
