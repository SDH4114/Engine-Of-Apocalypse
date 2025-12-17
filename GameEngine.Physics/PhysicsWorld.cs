// ============================================================================
// МИР ФИЗИКИ
// ============================================================================

using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace GameEngine.Core
{
    // ========================================================================
    // МИР ФИЗИКИ
    // ========================================================================
    public class PhysicsWorld
    {
        private List<RigidBody> _bodies = new List<RigidBody>();
        private List<Collider> _colliders = new List<Collider>();
        private ConcurrentBag<CollisionInfo> _collisions = new ConcurrentBag<CollisionInfo>();
        
        public Vector3 Gravity { get; set; } = new Vector3(0, -9.81f, 0);
        public int VelocityIterations { get; set; } = 8;
        public int PositionIterations { get; set; } = 3;
        
        private CollisionDetection _collisionDetection;
        private CollisionResolver _collisionResolver;

        public PhysicsWorld()
        {
            _collisionDetection = new CollisionDetection();
            _collisionResolver = new CollisionResolver();
        }

        public void AddBody(RigidBody body)
        {
            _bodies.Add(body);
        }

        public void AddCollider(Collider collider)
        {
            _colliders.Add(collider);
        }

        public void Step(float deltaTime)
        {
            // Многопоточная физика
            Parallel.ForEach(_bodies, body =>
            {
                if (body.IsKinematic) return;
                
                // Применение гравитации
                body.AddForce(Gravity * body.Mass);
                
                // Интеграция скорости
                body.Velocity += body.Acceleration * deltaTime;
                body.AngularVelocity += body.AngularAcceleration * deltaTime;
                
                // Применение демпфирования
                body.Velocity *= (1.0f - body.LinearDamping * deltaTime);
                body.AngularVelocity *= (1.0f - body.AngularDamping * deltaTime);
                
                // Интеграция позиции
                body.GameObject.Transform.Position += body.Velocity * deltaTime;
                
                // Интеграция вращения
                var angularDisplacement = body.AngularVelocity * deltaTime;
                if (angularDisplacement.LengthSquared() > 0)
                {
                    var rotation = Quaternion.CreateFromAxisAngle(
                        Vector3.Normalize(angularDisplacement),
                        angularDisplacement.Length());
                    body.GameObject.Transform.Rotation *= rotation;
                }
                
                // Сброс ускорения
                body.Acceleration = Vector3.Zero;
                body.AngularAcceleration = Vector3.Zero;
            });

            // Обнаружение коллизий (многопоточно)
            _collisions.Clear();
            DetectCollisions();

            // Разрешение коллизий
            ResolveCollisions(deltaTime);
        }

        private void DetectCollisions()
        {
            var pairs = new List<(Collider, Collider)>();
            
            // Broad phase - простая проверка всех пар
            for (int i = 0; i < _colliders.Count; i++)
            {
                for (int j = i + 1; j < _colliders.Count; j++)
                {
                    pairs.Add((_colliders[i], _colliders[j]));
                }
            }

            // Narrow phase - детальная проверка параллельно
            Parallel.ForEach(pairs, pair =>
            {
                var collision = _collisionDetection.CheckCollision(pair.Item1, pair.Item2);
                if (collision != null)
                {
                    _collisions.Add(collision);
                }
            });
        }

        private void ResolveCollisions(float deltaTime)
        {
            foreach (var collision in _collisions)
            {
                _collisionResolver.Resolve(collision);
            }
        }

        public bool Raycast(Vector3 origin, Vector3 direction, float maxDistance, out RaycastHit hit)
        {
            hit = default;
            float closestDistance = maxDistance;
            bool foundHit = false;

            foreach (var collider in _colliders)
            {
                if (collider.Raycast(origin, direction, out var distance))
                {
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        hit = new RaycastHit
                        {
                            Distance = distance,
                            Point = origin + direction * distance,
                            Collider = collider
                        };
                        foundHit = true;
                    }
                }
            }

            return foundHit;
        }
    }
}