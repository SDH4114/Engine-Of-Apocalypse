// ============================================================================
// BOX COLLIDER
// ============================================================================

using System;
using System.Numerics;

namespace GameEngine.Core
{
    // ========================================================================
    // BOX COLLIDER
    // ========================================================================
    public class BoxCollider : Collider
    {
        public Vector3 Size { get; set; } = Vector3.One;
        public Vector3 Center { get; set; }

        public Vector3 WorldCenter => GameObject!.Transform.Position + Center;
        public Vector3 WorldSize => Size * GameObject!.Transform.Scale;

        public override bool CheckCollision(Collider other, out CollisionInfo info)
        {
            info = null;
            
            if (other is BoxCollider box)
            {
                return CheckBoxBox(this, box, out info);
            }
            else if (other is SphereCollider sphere)
            {
                return CheckBoxSphere(this, sphere, out info);
            }
            
            return false;
        }

        private bool CheckBoxBox(BoxCollider a, BoxCollider b, out CollisionInfo? info)
        {
            info = null;
            var aMin = a.WorldCenter - a.WorldSize * 0.5f;
            var aMax = a.WorldCenter + a.WorldSize * 0.5f;
            var bMin = b.WorldCenter - b.WorldSize * 0.5f;
            var bMax = b.WorldCenter + b.WorldSize * 0.5f;

            bool overlap = aMin.X <= bMax.X && aMax.X >= bMin.X &&
                          aMin.Y <= bMax.Y && aMax.Y >= bMin.Y &&
                          aMin.Z <= bMax.Z && aMax.Z >= bMin.Z;

            if (overlap)
            {
                var penetration = new Vector3(
                    Math.Min(aMax.X - bMin.X, bMax.X - aMin.X),
                    Math.Min(aMax.Y - bMin.Y, bMax.Y - aMin.Y),
                    Math.Min(aMax.Z - bMin.Z, bMax.Z - aMin.Z));

                float minPenetration = Math.Min(penetration.X, Math.Min(penetration.Y, penetration.Z));
                Vector3 normal;

                if (minPenetration == penetration.X)
                    normal = new Vector3(aMax.X < bMax.X ? -1 : 1, 0, 0);
                else if (minPenetration == penetration.Y)
                    normal = new Vector3(0, aMax.Y < bMax.Y ? -1 : 1, 0);
                else
                    normal = new Vector3(0, 0, aMax.Z < bMax.Z ? -1 : 1);

                info = new CollisionInfo
                {
                    ColliderA = a,
                    ColliderB = b,
                    Normal = normal,
                    Penetration = minPenetration,
                    ContactPoint = (a.WorldCenter + b.WorldCenter) * 0.5f
                };
            }

            return overlap;
        }

        private bool CheckBoxSphere(BoxCollider box, SphereCollider sphere, out CollisionInfo? info)
        {
            info = null;
            var boxMin = box.WorldCenter - box.WorldSize * 0.5f;
            var boxMax = box.WorldCenter + box.WorldSize * 0.5f;
            var spherePos = sphere.WorldCenter;

            var closestPoint = Vector3.Clamp(spherePos, boxMin, boxMax);
            var distance = Vector3.Distance(closestPoint, spherePos);

            if (distance < sphere.WorldRadius)
            {
                var normal = Vector3.Normalize(spherePos - closestPoint);
                
                info = new CollisionInfo
                {
                    ColliderA = box,
                    ColliderB = sphere,
                    Normal = normal,
                    Penetration = sphere.WorldRadius - distance,
                    ContactPoint = closestPoint
                };
                return true;
            }

            return false;
        }

        public override bool Raycast(Vector3 origin, Vector3 direction, out float distance)
        {
            distance = float.MaxValue;
            var min = WorldCenter - WorldSize * 0.5f;
            var max = WorldCenter + WorldSize * 0.5f;

            var tMin = 0f;
            var tMax = float.MaxValue;

            const float eps = 1e-6f;

            if (!IntersectSlab(origin.X, direction.X, min.X, max.X, ref tMin, ref tMax, eps) ||
                !IntersectSlab(origin.Y, direction.Y, min.Y, max.Y, ref tMin, ref tMax, eps) ||
                !IntersectSlab(origin.Z, direction.Z, min.Z, max.Z, ref tMin, ref tMax, eps))
            {
                return false;
            }

            distance = tMin;
            return distance >= 0 && distance <= tMax;
        }

        private static bool IntersectSlab(float origin, float direction, float min, float max, ref float tMin, ref float tMax, float eps)
        {
            if (MathF.Abs(direction) < eps)
            {
                return origin >= min && origin <= max;
            }

            var invD = 1f / direction;
            var t1 = (min - origin) * invD;
            var t2 = (max - origin) * invD;

            if (t1 > t2)
            {
                (t1, t2) = (t2, t1);
            }

            if (t1 > tMin)
            {
                tMin = t1;
            }

            if (t2 < tMax)
            {
                tMax = t2;
            }

            return tMin <= tMax;
        }
    }
}

