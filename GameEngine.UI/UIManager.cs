using System;

namespace GameEngine.Core
{
    public sealed class UIManager
    {
        private readonly Canvas _rootCanvas = new();

        public UIManager()
        {
            Console.WriteLine("✓ UI Manager initialized");
        }

        public Canvas GetRootCanvas() => _rootCanvas;

        public void Update(float deltaTime)
        {
            _rootCanvas.Update(deltaTime);
        }

        public void Render(Renderer renderer)
        {
            _rootCanvas.Render(renderer);
        }
    }
}
