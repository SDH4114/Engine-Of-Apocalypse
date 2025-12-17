// ============================================================================
// RIGIDBODY
// ============================================================================

using System.Numerics;

namespace GameEngine.Core
{
    // ========================================================================
    // RIGIDBODY
    // ========================================================================
    public class RigidBody : Component
    {
        public float Mass { get; set; } = 1.0f;
        public Vector3 Velocity { get; set; }
        public Vector3 AngularVelocity { get; set; }
        public Vector3 Acceleration { get; set; }
        public Vector3 AngularAcceleration { get; set; }
        
        public float LinearDamping { get; set; } = 0.01f;
        public float AngularDamping { get; set; } = 0.05f;
        public float Restitution { get; set; } = 0.5f; // Упругость
        public float Friction { get; set; } = 0.3f;
        
        public bool IsKinematic { get; set; } = false;
        public bool UseGravity { get; set; } = true;

        private Vector3 _forceAccumulator;
        private Vector3 _torqueAccumulator;

        public override void OnAttach()
        {
            Engine.Instance.Physics.AddBody(this);
        }

        public void AddForce(Vector3 force)
        {
            _forceAccumulator += force;
            Acceleration = _forceAccumulator / Mass;
        }

        public void AddTorque(Vector3 torque)
        {
            _torqueAccumulator += torque;
            // Упрощенная модель без тензора инерции
            AngularAcceleration = _torqueAccumulator / Mass;
        }

        public void AddImpulse(Vector3 impulse)
        {
            Velocity += impulse / Mass;
        }

        public override void Update(float deltaTime)
        {
            _forceAccumulator = Vector3.Zero;
            _torqueAccumulator = Vector3.Zero;
        }
    }
}