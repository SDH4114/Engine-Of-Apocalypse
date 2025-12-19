using System.Numerics;

namespace GameEngine.Core
{
    public sealed class Material
    {
        public Shader? Shader { get; set; }
        public Vector3 Albedo { get; set; } = new Vector3(1, 1, 1);
        public float Metallic { get; set; }
        public float Roughness { get; set; } = 1f;
    }
}
