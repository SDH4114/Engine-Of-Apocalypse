// ============================================================================
// COLLISION RESOLVER
// ============================================================================

using System;
using System.Numerics;

namespace GameEngine.Core
{
    // ========================================================================
    // COLLISION RESOLVER
    // ========================================================================
    public class CollisionResolver
    {
        public void Resolve(CollisionInfo collision)
        {
            var bodyA = collision.ColliderA.GameObject.GetComponent<RigidBody>();
            var bodyB = collision.ColliderB.GameObject.GetComponent<RigidBody>();

            if (bodyA == null || bodyB == null) return;
            if (bodyA.IsKinematic && bodyB.IsKinematic) return;

            // Разделение объектов
            var correction = collision.Normal * collision.Penetration * 0.5f;
            
            if (!bodyA.IsKinematic)
                bodyA.GameObject.Transform.Position -= correction;
            if (!bodyB.IsKinematic)
                bodyB.GameObject.Transform.Position += correction;

            // Импульсное разрешение
            var relativeVelocity = bodyB.Velocity - bodyA.Velocity;
            var velocityAlongNormal = Vector3.Dot(relativeVelocity, collision.Normal);

            if (velocityAlongNormal > 0) return;

            var restitution = Math.Min(bodyA.Restitution, bodyB.Restitution);
            var impulseScalar = -(1 + restitution) * velocityAlongNormal;
            impulseScalar /= (1 / bodyA.Mass) + (1 / bodyB.Mass);

            var impulse = collision.Normal * impulseScalar;

            if (!bodyA.IsKinematic)
                bodyA.Velocity -= impulse / bodyA.Mass;
            if (!bodyB.IsKinematic)
                bodyB.Velocity += impulse / bodyB.Mass;

            // Трение
            ApplyFriction(bodyA, bodyB, collision, relativeVelocity);
        }

        private void ApplyFriction(RigidBody bodyA, RigidBody bodyB, CollisionInfo collision, Vector3 relativeVelocity)
        {
            var tangent = relativeVelocity - collision.Normal * Vector3.Dot(relativeVelocity, collision.Normal);
            if (tangent.LengthSquared() > 0.0001f)
                tangent = Vector3.Normalize(tangent);

            var friction = (bodyA.Friction + bodyB.Friction) * 0.5f;
            var frictionImpulse = tangent * friction * 0.1f;

            if (!bodyA.IsKinematic)
                bodyA.Velocity -= frictionImpulse;
            if (!bodyB.IsKinematic)
                bodyB.Velocity += frictionImpulse;
        }
    }
}