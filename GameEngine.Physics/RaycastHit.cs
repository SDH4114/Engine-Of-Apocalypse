// ============================================================================
// RAYCAST HIT
// ============================================================================

using System.Numerics;

namespace GameEngine.Core
{
    // ========================================================================
    // RAYCAST HIT
    // ========================================================================
    public struct RaycastHit
    {
        public float Distance;
        public Vector3 Point;
        public Vector3 Normal;
        public Collider Collider;
    }
}
