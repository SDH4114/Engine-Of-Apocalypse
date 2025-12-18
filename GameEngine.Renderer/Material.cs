using System.Numerics;

namespace GameEngine.Core
{
    public sealed class Material
    {
        public Shader Shader { get; set; }
        public Vector3 Albedo { get; set; } = new Vector3(1, 1, 1);
    }
}
