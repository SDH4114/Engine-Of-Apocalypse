// ============================================================================
// MESH RENDERER
// ============================================================================

namespace GameEngine.Core
{
    // ========================================================================
    // MESH RENDERER
    // ========================================================================
    public class MeshRenderer : Component
    {
        public Mesh? Mesh { get; set; }
        public Material? Material { get; set; }

        public void Render(Renderer renderer)
        {
            if (Mesh == null || Material == null || GameObject == null) return;
            
            renderer.DrawMesh(Mesh, Material, GameObject.Transform.GetModelMatrix());
        }
    }
}