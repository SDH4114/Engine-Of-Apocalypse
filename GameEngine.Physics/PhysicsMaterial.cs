// ============================================================================
// PHYSICS MATERIAL
// ============================================================================

namespace GameEngine.Core
{
    // ========================================================================
    // PHYSICS MATERIAL
    // ========================================================================
    public class PhysicsMaterial
    {
        public float StaticFriction { get; set; } = 0.6f;
        public float DynamicFriction { get; set; } = 0.4f;
        public float Restitution { get; set; } = 0.5f;

        public static PhysicsMaterial Default => new PhysicsMaterial();
    }
}