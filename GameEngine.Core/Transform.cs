// ============================================================================
// TRANSFORM
// ============================================================================

using System.Numerics;

namespace GameEngine.Core
{
    // ========================================================================
    // TRANSFORM
    // ========================================================================
    public class Transform
    {
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; } = Quaternion.Identity;
        public Vector3 Scale { get; set; } = Vector3.One;

        public Vector3 Forward => Vector3.Transform(-Vector3.UnitZ, Rotation);
        public Vector3 Right => Vector3.Transform(Vector3.UnitX, Rotation);
        public Vector3 Up => Vector3.Transform(Vector3.UnitY, Rotation);

        public Matrix4x4 GetModelMatrix()
        {
            return Matrix4x4.CreateScale(Scale) *
                   Matrix4x4.CreateFromQuaternion(Rotation) *
                   Matrix4x4.CreateTranslation(Position);
        }

        public void Translate(Vector3 translation)
        {
            Position += translation;
        }

        public void Rotate(Vector3 axis, float angle)
        {
            Rotation *= Quaternion.CreateFromAxisAngle(axis, angle);
        }

        public void LookAt(Vector3 target, Vector3 up)
        {
            var direction = Vector3.Normalize(target - Position);
            Rotation = Quaternion.CreateFromRotationMatrix(
                Matrix4x4.CreateLookAt(Vector3.Zero, direction, up));
        }
    }
}