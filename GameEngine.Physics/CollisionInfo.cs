// ============================================================================
// COLLISION INFO
// ============================================================================

using System.Numerics;

namespace GameEngine.Core
{
    // ========================================================================
    // COLLISION INFO
    // ========================================================================
    public class CollisionInfo
    {
        public Collider ColliderA { get; set; }
        public Collider ColliderB { get; set; }
        public Vector3 Normal { get; set; }
        public float Penetration { get; set; }
        public Vector3 ContactPoint { get; set; }
    }
}
