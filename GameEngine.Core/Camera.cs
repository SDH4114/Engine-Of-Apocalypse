// ============================================================================
// КАМЕРА
// ============================================================================

using System;
using System.Numerics;

namespace GameEngine.Core
{
    // ========================================================================
    // КАМЕРА
    // ========================================================================
    public class Camera : Component
    {
        public float FOV { get; set; } = 60f;
        public float Near { get; set; } = 0.1f;
        public float Far { get; set; } = 1000f;
        public float AspectRatio { get; set; } = 16f / 9f;

        public Matrix4x4 GetViewMatrix()
        {
            return Matrix4x4.CreateLookAt(
                GameObject.Transform.Position,
                GameObject.Transform.Position + GameObject.Transform.Forward,
                GameObject.Transform.Up);
        }

        public Matrix4x4 GetProjectionMatrix()
        {
            return Matrix4x4.CreatePerspectiveFieldOfView(
                FOV * (float)Math.PI / 180f, AspectRatio, Near, Far);
        }
    }
}