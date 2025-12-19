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
        public bool Orthographic { get; set; }
        public float OrthoSize { get; set; } = 5f;

        public Matrix4x4 GetViewMatrix()
        {
            if (GameObject == null)
                return Matrix4x4.Identity;
                
            return Matrix4x4.CreateLookAt(
                GameObject.Transform.Position,
                GameObject.Transform.Position + GameObject.Transform.Forward,
                GameObject.Transform.Up);
        }

        public Matrix4x4 GetProjectionMatrix()
        {
            if (Orthographic)
            {
                var halfHeight = OrthoSize;
                var halfWidth = OrthoSize * AspectRatio;
                return Matrix4x4.CreateOrthographic(halfWidth * 2f, halfHeight * 2f, Near, Far);
            }

            // OpenGL clip space expects Z in [-1; 1].
            // System.Numerics helpers may use different conventions depending on runtime,
            // so we build a projection matrix explicitly.
            var fovRad = FOV * (float)Math.PI / 180f;
            var f = 1.0f / (float)Math.Tan(fovRad * 0.5f);
            var rangeInv = 1.0f / (Near - Far);

            return new Matrix4x4(
                f / AspectRatio, 0f, 0f, 0f,
                0f, f, 0f, 0f,
                0f, 0f, (Far + Near) * rangeInv, -1f,
                0f, 0f, (2f * Far * Near) * rangeInv, 0f);
        }
    }
}