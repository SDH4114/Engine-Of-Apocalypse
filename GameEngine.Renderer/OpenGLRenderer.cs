using System;
using System.Numerics;
using Silk.NET.OpenGL;

namespace GameEngine.Core
{
    public sealed class OpenGLRenderer : IDisposable
    {
        private readonly GL _gl;

        public OpenGLRenderer(GL gl)
        {
            _gl = gl;
            _gl.Enable(GLEnum.DepthTest);
            _gl.Enable(GLEnum.CullFace);
            _gl.CullFace(GLEnum.Back);
        }

        public void Resize(uint width, uint height)
        {
            if (width == 0 || height == 0)
            {
                return;
            }

            _gl.Viewport(0, 0, width, height);
        }

        public void BeginFrame(Vector4 clearColor)
        {
            _gl.ClearColor(clearColor.X, clearColor.Y, clearColor.Z, clearColor.W);
            _gl.Clear((uint)(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit));
        }

        public void DrawMesh(Mesh mesh, Material material, Matrix4x4 model, Matrix4x4 view, Matrix4x4 projection)
        {
            if (mesh == null || material == null || material.Shader == null)
            {
                return;
            }

            mesh.Upload(_gl);

            material.Shader.Use(_gl);
            material.Shader.SetMatrix4(_gl, "uModel", model);
            material.Shader.SetMatrix4(_gl, "uView", view);
            material.Shader.SetMatrix4(_gl, "uProjection", projection);
            material.Shader.SetVector3(_gl, "uAlbedo", material.Albedo);

            _gl.BindVertexArray(mesh.Vao);
            _gl.DrawElements(PrimitiveType.Triangles, (uint)mesh.Indices.Length, DrawElementsType.UnsignedInt, 0);
            _gl.BindVertexArray(0);
        }

        public void Dispose()
        {
        }
    }
}
