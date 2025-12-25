using System;
using System.Diagnostics;
using EoA.Core;
using EoA.Graphics;
using Veldrid;
using Veldrid.StartupUtilities;
using Veldrid.Sdl2;

namespace EoA.Platform;

public sealed class EngineHost : IDisposable
{
    private readonly EngineConfig _config;
    private readonly IGame _game;
    private readonly Sdl2Window _window;
    private readonly GraphicsDevice _graphicsDevice;
    private readonly GraphicsDeviceContext _graphicsContext;
    private readonly EngineContext _context;
    private readonly Stopwatch _clock = Stopwatch.StartNew();
    private bool _disposed;
    private bool _shouldExit;

    public EngineHost(EngineConfig config, IGame game)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));
        _game = game ?? throw new ArgumentNullException(nameof(game));

        CreateWindowAndGraphics(out _window, out _graphicsDevice);
        _graphicsContext = new GraphicsDeviceContext(_graphicsDevice);
        _context = new EngineContext(_config, _graphicsContext);

        _window.Resized += () =>
        {
            _graphicsDevice.ResizeMainWindow((uint)_window.Width, (uint)_window.Height);
            _graphicsContext.Resize((uint)_window.Width, (uint)_window.Height);
        };
        _window.Closed += () => _shouldExit = true;
    }

    public void Run()
    {
        _game.Initialize(_context);
        double previousTimeSeconds = 0d;

        while (_window.Exists && !_shouldExit)
        {
            _window.PumpEvents();

            double totalSeconds = _clock.Elapsed.TotalSeconds;
            double deltaSeconds = totalSeconds - previousTimeSeconds;
            previousTimeSeconds = totalSeconds;

            var frameTime = new FrameTime(deltaSeconds, totalSeconds);
            _game.Update(frameTime);
            _game.Render(frameTime);
        }

        _game.Shutdown();
    }

    public void Exit() => _shouldExit = true;

    private void CreateWindowAndGraphics(out Sdl2Window window, out GraphicsDevice graphicsDevice)
    {
        var windowCI = new WindowCreateInfo(
            x: 100,
            y: 100,
            windowWidth: _config.WindowWidth,
            windowHeight: _config.WindowHeight,
            windowInitialState: WindowState.Normal,
            windowTitle: _config.WindowTitle);

        var options = new GraphicsDeviceOptions(
            debug: false,
            swapchainDepthFormat: PixelFormat.D32_Float_S8_UInt,
            syncToVerticalBlank: _config.VSync,
            resourceBindingModel: ResourceBindingModel.Improved,
            preferStandardClipSpaceYDirection: true,
            preferDepthRangeZeroToOne: true);

        VeldridStartup.CreateWindowAndGraphicsDevice(windowCI, options, out window, out graphicsDevice);
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;

        _graphicsDevice.WaitForIdle();
        _graphicsContext.Dispose();
        _window.Close();
    }
}
