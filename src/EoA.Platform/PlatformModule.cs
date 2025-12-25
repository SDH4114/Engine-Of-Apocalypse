using EoA.Core;
using Silk.NET.Windowing;
using Silk.NET.Maths;

namespace EoA.Platform;

/// <summary>
/// Platform module: manages window lifecycle and input.
/// Built on Silk.NET for cross-platform support.
/// </summary>
public sealed class PlatformModule : IEngineModule
{
    private IWindow? _window;
    private bool _shouldClose;

    public string Name => "Platform";
    public IWindow Window => _window ?? throw new InvalidOperationException("Window not initialized");
    public bool ShouldClose => _shouldClose;

    public event Action? OnLoad;
    public event Action<double>? OnUpdate;
    public event Action<double>? OnRender;
    public event Action? OnClosing;

    public void Initialize()
    {
        Logger.Info($"[{Name}] Initializing platform module...");
        // Window creation is deferred to CreateWindow()
    }

    public void CreateWindow(EngineConfig config)
    {
            var options = WindowOptions.Default with
            {
                Size = new Vector2D<int>(config.WindowWidth, config.WindowHeight),
                Title = config.WindowTitle,
                VSync = config.VSync,
                WindowState = config.Fullscreen ? WindowState.Fullscreen : WindowState.Normal,
                API = GraphicsAPI.Default
            };

        _window = Silk.NET.Windowing.Window.Create(options);
        
        _window.Load += () =>
        {
            Logger.Info($"[{Name}] Window loaded: {_window.Size.X}x{_window.Size.Y}");
            OnLoad?.Invoke();
        };

        _window.Update += (deltaTime) =>
        {
            OnUpdate?.Invoke(deltaTime);
        };

        _window.Render += (deltaTime) =>
        {
            OnRender?.Invoke(deltaTime);
        };

        _window.Closing += () =>
        {
            Logger.Info($"[{Name}] Window closing...");
            _shouldClose = true;
            OnClosing?.Invoke();
        };

        Logger.Info($"[{Name}] Window created: {config.WindowTitle}");
    }

    public void Run()
    {
        if (_window == null)
            throw new InvalidOperationException("Window not created. Call CreateWindow() first.");

        Logger.Info($"[{Name}] Starting window main loop...");
        _window.Run();
    }

    public void Update(float deltaTime)
    {
        // Main update is driven by window events
        _window?.DoEvents();
    }

    public void Shutdown()
    {
        Logger.Info($"[{Name}] Shutting down platform module...");
        _window?.Dispose();
        _window = null;
    }
}