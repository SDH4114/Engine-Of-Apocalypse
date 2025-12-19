using System;
using System.Numerics;

namespace GameEngine.Core
{
    public sealed class Button : UIElement
    {
        public string Text { get; set; }
        public event Action? OnClickEvent;

        public Button(string text, Vector2 position, Vector2 size)
        {
            Text = text;
            Position = position;
            Size = size;
        }

        public void Click()
        {
            OnClickEvent?.Invoke();
        }
    }
}
