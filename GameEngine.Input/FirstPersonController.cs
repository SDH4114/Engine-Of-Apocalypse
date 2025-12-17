// ============================================================================
// FIRST PERSON CONTROLLER
// ============================================================================

using System;
using System.Numerics;

namespace GameEngine.Core
{
    // ========================================================================
    // FIRST PERSON CONTROLLER
    // ========================================================================
    public class FirstPersonController : Component
    {
        public float MoveSpeed { get; set; } = 5.0f;
        public float SprintSpeed { get; set; } = 10.0f;
        public float LookSensitivity { get; set; } = 0.1f;
        public float MaxLookAngle { get; set; } = 89f;

        private float _pitch;
        private float _yaw;

        public override void Update(float deltaTime)
        {
            var input = Engine.Instance.Input;

            // Движение
            var movement = input.GetMovementInput();
            var speed = input.IsKeyDown(KeyCode.Shift) ? SprintSpeed : MoveSpeed;

            var forward = GameObject.Transform.Forward;
            var right = GameObject.Transform.Right;

            var moveDirection = forward * movement.Y + right * movement.X;
            if (moveDirection.LengthSquared() > 0)
            {
                GameObject.Transform.Translate(Vector3.Normalize(moveDirection) * speed * deltaTime);
            }

            // Взгляд
            var look = input.GetLookInput();
            _yaw += look.X * LookSensitivity;
            _pitch -= look.Y * LookSensitivity;
            _pitch = Math.Clamp(_pitch, -MaxLookAngle, MaxLookAngle);

            GameObject.Transform.Rotation = Quaternion.CreateFromYawPitchRoll(
                _yaw * (float)Math.PI / 180f,
                _pitch * (float)Math.PI / 180f,
                0f);
        }
    }
}
