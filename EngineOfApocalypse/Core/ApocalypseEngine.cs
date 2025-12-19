using EngineOfApocalypse.Input;
using EngineOfApocalypse.Renderer;
using Silk.NET.Core;
using Silk.NET.Maths;
using Silk.NET.Windowing;
using StbImageSharp;

namespace EngineOfApocalypse.Core;

public sealed class ApocalypseEngine
{
    private IWindow? _window;
    private EngineConfig? _config;

    private readonly InputSystem _input = new();
    private readonly OpenGLRenderSystem _renderer = new();

    public void Run(EngineConfig config)
    {
        _config = config;

        var options = WindowOptions.Default;
        options.Title = config.Title;
        options.Size = new Vector2D<int>(config.Width, config.Height);
        options.VSync = config.VSync;
        options.API = new GraphicsAPI(
            ContextAPI.OpenGL,
            ContextProfile.Core,
            ContextFlags.ForwardCompatible,
            new APIVersion(3, 3));

        _window = Window.Create(options);
        
        // Установка иконки окна, если указан путь
        if (!string.IsNullOrEmpty(config.IconPath) && File.Exists(config.IconPath))
        {
            try
            {
                var imageBytes = File.ReadAllBytes(config.IconPath);
                var image = ImageResult.FromMemory(imageBytes, ColorComponents.RedGreenBlueAlpha);
                
                if (image.Width > 0 && image.Height > 0)
                {
                    var rawImage = new RawImage(
                        image.Width,
                        image.Height,
                        image.Data
                    );
                    
                    _window.SetWindowIcon(new ReadOnlySpan<RawImage>(new[] { rawImage }));
                    Console.WriteLine($"[EOA] Window icon loaded: {config.IconPath} ({image.Width}x{image.Height})");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[EOA] Warning: Failed to load window icon from {config.IconPath}: {ex.Message}");
            }
        }
        
        _window.Load += OnLoad;
        _window.Update += OnUpdate;
        _window.Render += OnRender;

        Console.WriteLine($"[EOA] Starting window: {options.Title} {options.Size.X}x{options.Size.Y}");
        try
        {
            _window.Run();
        }
        catch (Exception ex)
        {
            Console.WriteLine("[EOA] Fatal error while running the window loop:");
            Console.WriteLine(ex);
            throw;
        }
        finally
        {
            Console.WriteLine("[EOA] Window loop finished.");
        }
    }

    private void OnLoad()
    {
        if (_window is null) return;

        Console.WriteLine("[EOA] Window loaded. Initializing renderer + input...");
        try
        {
            _renderer.Initialize(_window);
            _input.Initialize(_window);
        }
        catch (Exception ex)
        {
            Console.WriteLine("[EOA] Initialization failed:");
            Console.WriteLine(ex);
            _window.Close();
        }
    }

    private void OnUpdate(double dt)
    {
        if (_window is null) return;

        _input.Update();

        if (_input.IsEscapePressed())
        {
            _window.Close();
        }
    }

    private void OnRender(double dt)
    {
        if (_config is null) return;

        _renderer.BeginFrame(_config.ClearR, _config.ClearG, _config.ClearB, _config.ClearA);
    }
}
