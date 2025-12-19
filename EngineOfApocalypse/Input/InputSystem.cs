using System.Collections.Generic;
using Silk.NET.Input;
using Silk.NET.Windowing;

namespace EngineOfApocalypse.Input;

public sealed class InputSystem
{
    private IInputContext? _input;
    private readonly HashSet<Key> _keysDown = new();

    public void Initialize(IWindow window)
    {
        _input = window.CreateInput();

        foreach (var keyboard in _input.Keyboards)
        {
            keyboard.KeyDown += (_, key, _) => _keysDown.Add(key);
            keyboard.KeyUp += (_, key, _) => _keysDown.Remove(key);
        }
    }

    public void Update()
    {
        // Event-driven; nothing to do per-frame for now.
    }

    public bool IsKeyDown(Key key) => _keysDown.Contains(key);

    public bool IsEscapePressed() => IsKeyDown(Key.Escape);
}
