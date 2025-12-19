// ============================================================================
// COLLIDERS (Базовый класс)
// ============================================================================

using System.Numerics;

namespace GameEngine.Core
{
    // ========================================================================
    // COLLIDERS
    // ========================================================================
    public abstract class Collider : Component
    {
        public bool IsTrigger { get; set; } = false;
        public PhysicsMaterial Material { get; set; } = PhysicsMaterial.Default;

        public abstract bool CheckCollision(Collider other, out CollisionInfo? info);
        public abstract bool Raycast(Vector3 origin, Vector3 direction, out float distance);
        
        public override void OnAttach()
        {
            Engine.Instance.Physics.AddCollider(this);
        }
    }
}