using System.Numerics;

namespace GameEngine.Core
{
    public enum LightType
    {
        Directional,
        Point,
        Spot
    }

    public sealed class Light : Component
    {
        public LightType Type { get; set; } = LightType.Point;
        public Vector3 Color { get; set; } = new Vector3(1, 1, 1);
        public float Intensity { get; set; } = 1.0f;
    }
}
