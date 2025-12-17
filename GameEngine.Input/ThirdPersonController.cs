// ============================================================================
// THIRD PERSON CONTROLLER
// ============================================================================

using System;
using System.Numerics;

namespace GameEngine.Core
{
    // ========================================================================
    // THIRD PERSON CONTROLLER
    // ========================================================================
    public class ThirdPersonController : Component
    {
        public GameObject Target { get; set; }
        public float Distance { get; set; } = 5.0f;
        public float Height { get; set; } = 2.0f;
        public float RotationSpeed { get; set; } = 5.0f;
        public float LookSensitivity { get; set; } = 0.1f;

        private float _horizontalAngle;
        private float _verticalAngle = 20f;

        public override void Update(float deltaTime)
        {
            if (Target == null) return;

            var input = Engine.Instance.Input;

            // Вращение камеры
            var look = input.GetLookInput();
            _horizontalAngle += look.X * LookSensitivity;
            _verticalAngle -= look.Y * LookSensitivity;
            _verticalAngle = Math.Clamp(_verticalAngle, -80f, 80f);

            // Позиция камеры
            var targetPos = Target.Transform.Position;
            var rotation = Quaternion.CreateFromYawPitchRoll(
                _horizontalAngle * (float)Math.PI / 180f,
                _verticalAngle * (float)Math.PI / 180f,
                0f);

            var offset = Vector3.Transform(new Vector3(0, Height, -Distance), rotation);
            GameObject.Transform.Position = targetPos + offset;
            GameObject.Transform.LookAt(targetPos + Vector3.UnitY * Height, Vector3.UnitY);
        }
    }
}
