using System;
using System.IO;
using Silk.NET.Core;
using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.Windowing;
using StbImageSharp;

namespace GameEngine.Core
{
    public sealed class Window : IDisposable
    {
        private readonly IWindow _window;
        private IInputContext? _input;

        public Window(int width, int height, string title)
        {
            var options = WindowOptions.Default;
            options.Size = new Vector2D<int>(width, height);
            options.Title = title;
            options.API = new Silk.NET.Windowing.GraphicsAPI(
                ContextAPI.OpenGL,
                ContextProfile.Core,
                ContextFlags.ForwardCompatible,
                new APIVersion(3, 3));

            _window = Silk.NET.Windowing.Window.Create(options);

            _window.Load += () =>
            {
                TrySetIcon("Assets/englogo.png");
                SetupInput();
            };
        }

        public IWindow NativeWindow => _window;

        public Vector2D<int> Size => _window.Size;

        public bool IsOpen => !_window.IsClosing;

        public void PollEvents()
        {
            _window.DoEvents();
        }

        public void Run(Action<float>? onUpdate, Action<float>? onRender, Action? onClose = null)
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

        private void TrySetIcon(string relativePath)
        {
            try
            {
                // Пробуем несколько возможных путей
                string[] possiblePaths = new[]
                {
                    Path.Combine(AppContext.BaseDirectory, relativePath),
                    Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", relativePath),
                    Path.Combine(Directory.GetCurrentDirectory(), relativePath),
                    Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "..", relativePath),
                    relativePath
                };

                string? foundPath = null;
                foreach (var path in possiblePaths)
                {
                    var fullPath = Path.GetFullPath(path);
                    if (File.Exists(fullPath))
                    {
                        foundPath = fullPath;
                        break;
                    }
                }

                if (foundPath == null)
                {
                    return;
                }

                var bytes = File.ReadAllBytes(foundPath);
                var image = ImageResult.FromMemory(bytes, ColorComponents.RedGreenBlueAlpha);
                var raw = new Silk.NET.Core.RawImage(image.Width, image.Height, image.Data);
                _window.SetWindowIcon(new Silk.NET.Core.RawImage[] { raw });
                Console.WriteLine($"✓ Window icon loaded: {foundPath}");
            }
            catch (Exception ex)
            {
                // Иконка не критична, просто логируем
                Console.WriteLine($"Could not load window icon: {ex.Message}");
            }
        }

        private void SetupInput()
        {
            _input = _window.CreateInput();

            if (_input.Keyboards.Count > 0)
            {
                var keyboard = _input.Keyboards[0];
                keyboard.KeyDown += (_, key, _) =>
                {
                    if (TryMapKey(key, out var code))
                    {
                        Engine.Instance.Input?.SetKeyState(code, true);
                    }
                };

                keyboard.KeyUp += (_, key, _) =>
                {
                    if (TryMapKey(key, out var code))
                    {
                        Engine.Instance.Input?.SetKeyState(code, false);
                    }
                };
            }

            if (_input.Mice.Count > 0)
            {
                var mouse = _input.Mice[0];

                mouse.MouseMove += (_, pos) =>
                {
                    Engine.Instance.Input?.SetMousePosition(new System.Numerics.Vector2(pos.X, pos.Y));
                };

                mouse.Scroll += (_, wheel) =>
                {
                    Engine.Instance.Input?.SetScrollDelta(wheel.Y);
                };

                mouse.MouseDown += (_, button) =>
                {
                    if (TryMapMouseButton(button, out var b))
                    {
                        Engine.Instance.Input?.SetMouseButtonState(b, true);
                    }
                };

                mouse.MouseUp += (_, button) =>
                {
                    if (TryMapMouseButton(button, out var b))
                    {
                        Engine.Instance.Input?.SetMouseButtonState(b, false);
                    }
                };
            }
        }

        private static bool TryMapMouseButton(Silk.NET.Input.MouseButton button, out MouseButton mapped)
        {
            mapped = default;
            switch (button)
            {
                case Silk.NET.Input.MouseButton.Left:
                    mapped = MouseButton.Left;
                    return true;
                case Silk.NET.Input.MouseButton.Right:
                    mapped = MouseButton.Right;
                    return true;
                case Silk.NET.Input.MouseButton.Middle:
                    mapped = MouseButton.Middle;
                    return true;
                case Silk.NET.Input.MouseButton.Button4:
                    mapped = MouseButton.Button4;
                    return true;
                case Silk.NET.Input.MouseButton.Button5:
                    mapped = MouseButton.Button5;
                    return true;
                default:
                    return false;
            }
        }

        private static bool TryMapKey(Silk.NET.Input.Key key, out KeyCode code)
        {
            code = default;
            switch (key)
            {
                case Silk.NET.Input.Key.A: code = KeyCode.A; return true;
                case Silk.NET.Input.Key.B: code = KeyCode.B; return true;
                case Silk.NET.Input.Key.C: code = KeyCode.C; return true;
                case Silk.NET.Input.Key.D: code = KeyCode.D; return true;
                case Silk.NET.Input.Key.E: code = KeyCode.E; return true;
                case Silk.NET.Input.Key.F: code = KeyCode.F; return true;
                case Silk.NET.Input.Key.G: code = KeyCode.G; return true;
                case Silk.NET.Input.Key.H: code = KeyCode.H; return true;
                case Silk.NET.Input.Key.I: code = KeyCode.I; return true;
                case Silk.NET.Input.Key.J: code = KeyCode.J; return true;
                case Silk.NET.Input.Key.K: code = KeyCode.K; return true;
                case Silk.NET.Input.Key.L: code = KeyCode.L; return true;
                case Silk.NET.Input.Key.M: code = KeyCode.M; return true;
                case Silk.NET.Input.Key.N: code = KeyCode.N; return true;
                case Silk.NET.Input.Key.O: code = KeyCode.O; return true;
                case Silk.NET.Input.Key.P: code = KeyCode.P; return true;
                case Silk.NET.Input.Key.Q: code = KeyCode.Q; return true;
                case Silk.NET.Input.Key.R: code = KeyCode.R; return true;
                case Silk.NET.Input.Key.S: code = KeyCode.S; return true;
                case Silk.NET.Input.Key.T: code = KeyCode.T; return true;
                case Silk.NET.Input.Key.U: code = KeyCode.U; return true;
                case Silk.NET.Input.Key.V: code = KeyCode.V; return true;
                case Silk.NET.Input.Key.W: code = KeyCode.W; return true;
                case Silk.NET.Input.Key.X: code = KeyCode.X; return true;
                case Silk.NET.Input.Key.Y: code = KeyCode.Y; return true;
                case Silk.NET.Input.Key.Z: code = KeyCode.Z; return true;

                case Silk.NET.Input.Key.Number0: code = KeyCode.Num0; return true;
                case Silk.NET.Input.Key.Number1: code = KeyCode.Num1; return true;
                case Silk.NET.Input.Key.Number2: code = KeyCode.Num2; return true;
                case Silk.NET.Input.Key.Number3: code = KeyCode.Num3; return true;
                case Silk.NET.Input.Key.Number4: code = KeyCode.Num4; return true;
                case Silk.NET.Input.Key.Number5: code = KeyCode.Num5; return true;
                case Silk.NET.Input.Key.Number6: code = KeyCode.Num6; return true;
                case Silk.NET.Input.Key.Number7: code = KeyCode.Num7; return true;
                case Silk.NET.Input.Key.Number8: code = KeyCode.Num8; return true;
                case Silk.NET.Input.Key.Number9: code = KeyCode.Num9; return true;

                case Silk.NET.Input.Key.Escape: code = KeyCode.Escape; return true;
                case Silk.NET.Input.Key.Tab: code = KeyCode.Tab; return true;
                case Silk.NET.Input.Key.CapsLock: code = KeyCode.CapsLock; return true;
                case Silk.NET.Input.Key.ShiftLeft:
                case Silk.NET.Input.Key.ShiftRight:
                    code = KeyCode.Shift;
                    return true;
                case Silk.NET.Input.Key.ControlLeft:
                case Silk.NET.Input.Key.ControlRight:
                    code = KeyCode.Control;
                    return true;
                case Silk.NET.Input.Key.AltLeft:
                case Silk.NET.Input.Key.AltRight:
                    code = KeyCode.Alt;
                    return true;
                case Silk.NET.Input.Key.Space: code = KeyCode.Space; return true;
                case Silk.NET.Input.Key.Enter: code = KeyCode.Enter; return true;
                case Silk.NET.Input.Key.Backspace: code = KeyCode.Backspace; return true;

                case Silk.NET.Input.Key.Left: code = KeyCode.Left; return true;
                case Silk.NET.Input.Key.Right: code = KeyCode.Right; return true;
                case Silk.NET.Input.Key.Up: code = KeyCode.Up; return true;
                case Silk.NET.Input.Key.Down: code = KeyCode.Down; return true;

                case Silk.NET.Input.Key.F1: code = KeyCode.F1; return true;
                case Silk.NET.Input.Key.F2: code = KeyCode.F2; return true;
                case Silk.NET.Input.Key.F3: code = KeyCode.F3; return true;
                case Silk.NET.Input.Key.F4: code = KeyCode.F4; return true;
                case Silk.NET.Input.Key.F5: code = KeyCode.F5; return true;
                case Silk.NET.Input.Key.F6: code = KeyCode.F6; return true;
                case Silk.NET.Input.Key.F7: code = KeyCode.F7; return true;
                case Silk.NET.Input.Key.F8: code = KeyCode.F8; return true;
                case Silk.NET.Input.Key.F9: code = KeyCode.F9; return true;
                case Silk.NET.Input.Key.F10: code = KeyCode.F10; return true;
                case Silk.NET.Input.Key.F11: code = KeyCode.F11; return true;
                case Silk.NET.Input.Key.F12: code = KeyCode.F12; return true;

                case Silk.NET.Input.Key.Insert: code = KeyCode.Insert; return true;
                case Silk.NET.Input.Key.Delete: code = KeyCode.Delete; return true;
                case Silk.NET.Input.Key.Home: code = KeyCode.Home; return true;
                case Silk.NET.Input.Key.End: code = KeyCode.End; return true;
                case Silk.NET.Input.Key.PageUp: code = KeyCode.PageUp; return true;
                case Silk.NET.Input.Key.PageDown: code = KeyCode.PageDown; return true;

                case Silk.NET.Input.Key.Minus: code = KeyCode.Minus; return true;
                case Silk.NET.Input.Key.Equal: code = KeyCode.Equals; return true;
                case Silk.NET.Input.Key.LeftBracket: code = KeyCode.LeftBracket; return true;
                case Silk.NET.Input.Key.RightBracket: code = KeyCode.RightBracket; return true;
                case Silk.NET.Input.Key.Semicolon: code = KeyCode.Semicolon; return true;
                case Silk.NET.Input.Key.Apostrophe: code = KeyCode.Quote; return true;
                case Silk.NET.Input.Key.Comma: code = KeyCode.Comma; return true;
                case Silk.NET.Input.Key.Period: code = KeyCode.Period; return true;
                case Silk.NET.Input.Key.Slash: code = KeyCode.Slash; return true;
                case Silk.NET.Input.Key.BackSlash: code = KeyCode.Backslash; return true;
                case Silk.NET.Input.Key.GraveAccent: code = KeyCode.Grave; return true;

                default:
                    return false;
            }
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
