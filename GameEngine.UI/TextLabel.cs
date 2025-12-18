using System.Numerics;

namespace GameEngine.Core
{
    public sealed class TextLabel : UIElement
    {
        public string Text { get; set; }
        public float FontSize { get; set; } = 16f;

        public TextLabel(string text, Vector2 position)
        {
            Text = text;
            Position = position;
        }
    }
}
