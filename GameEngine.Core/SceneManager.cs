// ============================================================================
// МЕНЕДЖЕР СЦЕН
// ============================================================================

using System.Collections.Generic;

namespace GameEngine.Core
{
    // ========================================================================
    // МЕНЕДЖЕР СЦЕН
    // ========================================================================
    public class SceneManager
    {
        private Scene? _activeScene;
        private Dictionary<string, Scene> _scenes = new Dictionary<string, Scene>();

        public void AddScene(string name, Scene scene)
        {
            _scenes[name] = scene;
        }

        public void LoadScene(string name)
        {
            if (_scenes.TryGetValue(name, out var scene))
            {
                _activeScene?.OnUnload();
                _activeScene = scene;
                _activeScene.OnLoad();
            }
        }

        public void Update(float deltaTime)
        {
            _activeScene?.Update(deltaTime);
        }

        public void Render(Renderer renderer)
        {
            _activeScene?.Render(renderer);
        }
    }
}