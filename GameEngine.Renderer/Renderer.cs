using System;
using System.Numerics;

namespace GameEngine.Core
{
    public sealed class Renderer : IDisposable
    {
        private readonly GraphicsAPI _api;
        private Camera _activeCamera;

        public Renderer(GraphicsAPI api)
        {
            _api = api;
        }

        public void BeginFrame()
        {
        }

        public void EndFrame()
        {
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
        }

        public void Dispose()
        {
        }
    }
}
