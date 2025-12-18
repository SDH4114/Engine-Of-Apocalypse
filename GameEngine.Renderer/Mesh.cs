using System;
using System.Numerics;

namespace GameEngine.Core
{
    public struct Vertex
    {
        public Vector3 Position;
        public Vector3 Normal;
        public Vector2 TexCoord;

        public Vertex(Vector3 position, Vector3 normal, Vector2 texCoord)
        {
            Position = position;
            Normal = normal;
            TexCoord = texCoord;
        }
    }

    public sealed class Mesh
    {
        public Vertex[] Vertices { get; set; } = Array.Empty<Vertex>();
        public uint[] Indices { get; set; } = Array.Empty<uint>();

        public void Upload()
        {
        }

        public static Mesh CreateCube()
        {
            var vertices = new[]
            {
                new Vertex(new Vector3(-0.5f, -0.5f,  0.5f), Vector3.UnitZ, new Vector2(0, 0)),
                new Vertex(new Vector3( 0.5f, -0.5f,  0.5f), Vector3.UnitZ, new Vector2(1, 0)),
                new Vertex(new Vector3( 0.5f,  0.5f,  0.5f), Vector3.UnitZ, new Vector2(1, 1)),
                new Vertex(new Vector3(-0.5f,  0.5f,  0.5f), Vector3.UnitZ, new Vector2(0, 1)),
            };

            var indices = new uint[] { 0, 1, 2, 2, 3, 0 };

            var mesh = new Mesh
            {
                Vertices = vertices,
                Indices = indices
            };
            mesh.Upload();
            return mesh;
        }
    }
}
