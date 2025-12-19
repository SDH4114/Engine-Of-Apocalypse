// ============================================================================
// COLLISION DETECTION
// ============================================================================

namespace GameEngine.Core
{
    // ========================================================================
    // COLLISION DETECTION
    // ========================================================================
    public class CollisionDetection
    {
        public CollisionInfo? CheckCollision(Collider a, Collider b)
        {
            if (a.CheckCollision(b, out var info))
            {
                return info;
            }
            return null;
        }
    }
}