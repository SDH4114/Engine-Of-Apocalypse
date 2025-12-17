// ============================================================================
// СЦЕНА
// ============================================================================

using System.Collections.Generic;

namespace GameEngine.Core
{
    // ========================================================================
    // СЦЕНА
    // ========================================================================
    public class Scene
    {
        public string Name { get; set; }
        private List<GameObject> _gameObjects = new List<GameObject>();
        public Camera MainCamera { get; set; }

        public void AddGameObject(GameObject obj)
        {
            _gameObjects.Add(obj);
        }

        public virtual void OnLoad() { }
        public virtual void OnUnload() { }

        public void Update(float deltaTime)
        {
            foreach (var obj in _gameObjects)
                obj.Update(deltaTime);
        }

        public void Render(Renderer renderer)
        {
            if (MainCamera == null) return;
            
            renderer.SetCamera(MainCamera);
            
            foreach (var obj in _gameObjects)
            {
                var meshRenderer = obj.GetComponent<MeshRenderer>();
                meshRenderer?.Render(renderer);
            }
        }
    }
}
