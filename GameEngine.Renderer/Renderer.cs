using System;
using System.Numerics;
using Silk.NET.OpenGL;

namespace GameEngine.Core
{
    public sealed class Renderer : IDisposable
    {
        private readonly GraphicsAPI _api;
        private readonly Window _window;
        private GL? _gl;
        private bool _glReady;
        private OpenGLRenderer? _glRenderer;
        private Camera? _activeCamera;

        public Renderer(GraphicsAPI api, Window window)
        {
            _api = api;
            _window = window;
        }

        public void BeginFrame()
        {
            EnsureGL();
            if (!_glReady)
            {
                return;
            }

            _glRenderer.BeginFrame(new Vector4(0.05f, 0.07f, 0.12f, 1.0f));
        }

        public void EndFrame()
        {
            if (_glReady)
            {
            }
        }

        public void SetCamera(Camera camera)
        {
            _activeCamera = camera;
        }

        public void DrawMesh(Mesh mesh, Material material, Matrix4x4 modelMatrix)
        {
            if (mesh == null || material == null)
            {
                return;
            }

            if (!_glReady || _glRenderer == null || _activeCamera == null)
            {
                return;
            }

            _glRenderer.DrawMesh(mesh, material, modelMatrix, _activeCamera.GetViewMatrix(), _activeCamera.GetProjectionMatrix());
        }

        private void EnsureGL()
        {
            if (_glReady)
            {
                return;
            }

            if (_api != GraphicsAPI.OpenGL)
            {
                return;
            }

            _gl = GL.GetApi(_window.NativeWindow);
            _glRenderer = new OpenGLRenderer(_gl);
            _glRenderer.Resize((uint)_window.Size.X, (uint)_window.Size.Y);
            _glReady = true;
        }

        public void Dispose()
        {
        }
    }
}
