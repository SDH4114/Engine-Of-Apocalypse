using System.Collections.Generic;

namespace GameEngine.Core
{
    public sealed class Canvas : UIElement
    {
        private readonly List<UIElement> _children = new();

        public void AddChild(UIElement element)
        {
            if (element == null) return;
            _children.Add(element);
        }

        public override void Update(float deltaTime)
        {
            if (!Visible) return;

            foreach (var child in _children)
            {
                child.Update(deltaTime);
            }
        }

        public override void Render(Renderer renderer)
        {
            if (!Visible) return;

            foreach (var child in _children)
            {
                child.Render(renderer);
            }
        }
    }
}
