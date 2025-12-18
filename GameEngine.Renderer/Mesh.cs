using System;
using System.Numerics;
using Silk.NET.OpenGL;

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

        public uint Vao { get; private set; }
        private uint _vbo;
        private uint _ebo;
        private bool _uploaded;

        public void Upload()
        {
        }

        public void Upload(GL gl)
        {
            if (_uploaded)
            {
                return;
            }

            Vao = gl.GenVertexArray();
            _vbo = gl.GenBuffer();
            _ebo = gl.GenBuffer();

            gl.BindVertexArray(Vao);

            gl.BindBuffer(BufferTargetARB.ArrayBuffer, _vbo);
            var vertexData = BuildInterleavedVertexData();
            unsafe
            {
                fixed (float* vertexPtr = vertexData)
                {
                    gl.BufferData(GLEnum.ArrayBuffer, (nuint)(vertexData.Length * sizeof(float)), vertexPtr, GLEnum.StaticDraw);
                }
            }

            gl.BindBuffer(BufferTargetARB.ElementArrayBuffer, _ebo);
            unsafe
            {
                fixed (uint* indexPtr = Indices)
                {
                    gl.BufferData(GLEnum.ElementArrayBuffer, (nuint)(Indices.Length * sizeof(uint)), indexPtr, GLEnum.StaticDraw);
                }
            }

            const uint stride = 8 * sizeof(float);
            gl.EnableVertexAttribArray(0);
            gl.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, stride, 0);

            gl.EnableVertexAttribArray(1);
            gl.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, stride, 3 * sizeof(float));

            gl.EnableVertexAttribArray(2);
            gl.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, stride, 6 * sizeof(float));

            gl.BindVertexArray(0);

            _uploaded = true;
        }

        private float[] BuildInterleavedVertexData()
        {
            var data = new float[Vertices.Length * 8];
            var i = 0;
            foreach (var v in Vertices)
            {
                data[i++] = v.Position.X;
                data[i++] = v.Position.Y;
                data[i++] = v.Position.Z;

                data[i++] = v.Normal.X;
                data[i++] = v.Normal.Y;
                data[i++] = v.Normal.Z;

                data[i++] = v.TexCoord.X;
                data[i++] = v.TexCoord.Y;
            }
            return data;
        }

        public static Mesh CreateCube()
        {
            var vertices = new[]
            {
                // Front
                new Vertex(new Vector3(-0.5f, -0.5f,  0.5f), Vector3.UnitZ, new Vector2(0, 0)),
                new Vertex(new Vector3( 0.5f, -0.5f,  0.5f), Vector3.UnitZ, new Vector2(1, 0)),
                new Vertex(new Vector3( 0.5f,  0.5f,  0.5f), Vector3.UnitZ, new Vector2(1, 1)),
                new Vertex(new Vector3(-0.5f,  0.5f,  0.5f), Vector3.UnitZ, new Vector2(0, 1)),
                // Back
                new Vertex(new Vector3( 0.5f, -0.5f, -0.5f), -Vector3.UnitZ, new Vector2(0, 0)),
                new Vertex(new Vector3(-0.5f, -0.5f, -0.5f), -Vector3.UnitZ, new Vector2(1, 0)),
                new Vertex(new Vector3(-0.5f,  0.5f, -0.5f), -Vector3.UnitZ, new Vector2(1, 1)),
                new Vertex(new Vector3( 0.5f,  0.5f, -0.5f), -Vector3.UnitZ, new Vector2(0, 1)),
                // Left
                new Vertex(new Vector3(-0.5f, -0.5f, -0.5f), -Vector3.UnitX, new Vector2(0, 0)),
                new Vertex(new Vector3(-0.5f, -0.5f,  0.5f), -Vector3.UnitX, new Vector2(1, 0)),
                new Vertex(new Vector3(-0.5f,  0.5f,  0.5f), -Vector3.UnitX, new Vector2(1, 1)),
                new Vertex(new Vector3(-0.5f,  0.5f, -0.5f), -Vector3.UnitX, new Vector2(0, 1)),
                // Right
                new Vertex(new Vector3( 0.5f, -0.5f,  0.5f), Vector3.UnitX, new Vector2(0, 0)),
                new Vertex(new Vector3( 0.5f, -0.5f, -0.5f), Vector3.UnitX, new Vector2(1, 0)),
                new Vertex(new Vector3( 0.5f,  0.5f, -0.5f), Vector3.UnitX, new Vector2(1, 1)),
                new Vertex(new Vector3( 0.5f,  0.5f,  0.5f), Vector3.UnitX, new Vector2(0, 1)),
                // Top
                new Vertex(new Vector3(-0.5f,  0.5f,  0.5f), Vector3.UnitY, new Vector2(0, 0)),
                new Vertex(new Vector3( 0.5f,  0.5f,  0.5f), Vector3.UnitY, new Vector2(1, 0)),
                new Vertex(new Vector3( 0.5f,  0.5f, -0.5f), Vector3.UnitY, new Vector2(1, 1)),
                new Vertex(new Vector3(-0.5f,  0.5f, -0.5f), Vector3.UnitY, new Vector2(0, 1)),
                // Bottom
                new Vertex(new Vector3(-0.5f, -0.5f, -0.5f), -Vector3.UnitY, new Vector2(0, 0)),
                new Vertex(new Vector3( 0.5f, -0.5f, -0.5f), -Vector3.UnitY, new Vector2(1, 0)),
                new Vertex(new Vector3( 0.5f, -0.5f,  0.5f), -Vector3.UnitY, new Vector2(1, 1)),
                new Vertex(new Vector3(-0.5f, -0.5f,  0.5f), -Vector3.UnitY, new Vector2(0, 1)),
            };

            var indices = new uint[]
            {
                0, 1, 2, 2, 3, 0,
                4, 5, 6, 6, 7, 4,
                8, 9, 10, 10, 11, 8,
                12, 13, 14, 14, 15, 12,
                16, 17, 18, 18, 19, 16,
                20, 21, 22, 22, 23, 20
            };

            var mesh = new Mesh
            {
                Vertices = vertices,
                Indices = indices
            };
            return mesh;
        }
    }
}
