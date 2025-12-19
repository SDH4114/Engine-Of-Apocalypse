// ============================================================================
// ORBIT CAMERA CONTROLLER
// ============================================================================

using System;
using System.Numerics;

namespace GameEngine.Core
{
    // ========================================================================
    // ORBIT CAMERA CONTROLLER
    // ========================================================================
    public class OrbitCameraController : Component
    {
        public Vector3 Target { get; set; }
        public float Distance { get; set; } = 10f;
        public float MinDistance { get; set; } = 2f;
        public float MaxDistance { get; set; } = 50f;
        public float RotationSpeed { get; set; } = 0.2f;
        public float ZoomSpeed { get; set; } = 1f;

        private float _theta; // Горизонтальный угол
        private float _phi = 45f; // Вертикальный угол

        public override void Update(float deltaTime)
        {
            if (GameObject == null) return;
            
            var input = Engine.Instance.Input;
            if (input == null) return;

            // Вращение при зажатой правой кнопке мыши
            if (input.IsMouseButtonDown(MouseButton.Right))
            {
                var delta = input.GetMouseDelta();
                _theta += delta.X * RotationSpeed;
                _phi -= delta.Y * RotationSpeed;
                _phi = Math.Clamp(_phi, 1f, 179f);
            }

            // Зум колесиком мыши
            Distance -= input.GetScrollDelta() * ZoomSpeed;
            Distance = Math.Clamp(Distance, MinDistance, MaxDistance);

            // Вычисление позиции камеры
            var thetaRad = _theta * (float)Math.PI / 180f;
            var phiRad = _phi * (float)Math.PI / 180f;

            var x = Distance * MathF.Sin(phiRad) * MathF.Cos(thetaRad);
            var y = Distance * MathF.Cos(phiRad);
            var z = Distance * MathF.Sin(phiRad) * MathF.Sin(thetaRad);

            GameObject.Transform.Position = Target + new Vector3(x, y, z);
            GameObject.Transform.LookAt(Target, Vector3.UnitY);
        }
    }
}

