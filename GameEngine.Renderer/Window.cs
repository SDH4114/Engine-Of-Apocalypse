using System;
using Silk.NET.Maths;
using Silk.NET.Windowing;

namespace GameEngine.Core
{
    public sealed class Window : IDisposable
    {
        private readonly IWindow _window;

        public Window(int width, int height, string title)
        {
            var options = WindowOptions.Default;
            options.Size = new Vector2D<int>(width, height);
            options.Title = title;

            _window = Silk.NET.Windowing.Window.Create(options);
            _window.Initialize();
        }

        public bool IsOpen => !_window.IsClosing;

        public void PollEvents()
        {
            _window.DoEvents();
        }

        public void Dispose()
        {
            _window.Dispose();
        }
    }
}
