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
        }

        public IWindow NativeWindow => _window;

        public Vector2D<int> Size => _window.Size;

        public bool IsOpen => !_window.IsClosing;

        public void PollEvents()
        {
            _window.DoEvents();
        }

        public void Run(Action<float> onUpdate, Action<float> onRender, Action onClose = null)
        {
            if (onUpdate != null)
            {
                _window.Update += dt => onUpdate((float)dt);
            }

            if (onRender != null)
            {
                _window.Render += dt => onRender((float)dt);
            }

            if (onClose != null)
            {
                _window.Closing += onClose;
            }

            _window.Run();
        }

        public void Close()
        {
            _window.Close();
        }

        public void SwapBuffers()
        {
            _window.SwapBuffers();
        }

        public void Dispose()
        {
            _window.Dispose();
        }
    }
}
