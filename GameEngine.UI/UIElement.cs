using System.Numerics;

namespace GameEngine.Core
{
    public abstract class UIElement
    {
        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; }
        public bool Visible { get; set; } = true;

        protected UIElement()
        {
        }

        protected UIElement(Vector2 position, Vector2 size)
        {
            Position = position;
            Size = size;
        }

        public virtual void Update(float deltaTime)
        {
        }

        public virtual void Render(Renderer renderer)
        {
        }
    }
}
