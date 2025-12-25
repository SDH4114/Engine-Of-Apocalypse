using EoA.Core;
using EoA.Platform;
using EoA.Graphics;

namespace EoA.SampleGame;

/// <summary>
/// Пример игры, демонстрирующий использование Engine Of Apocalypse.
/// Milestone 1: Открывает окно, очищает экран, запускает игровой цикл.
/// </summary>
internal class Program
{
    private static PlatformModule? _platform;
    private static GraphicsModule? _graphics;
    private static Time? _time;

    static void Main(string[] args)
    {
        Logger.Info("=== Engine Of Apocalypse - Sample Game ===");
        Logger.Info("Milestone 1: Bootstrap + Window + Clear Screen");
        Logger.Info("");

        var config = new EngineConfig
        {
            WindowTitle = "EoA Sample Game - Milestone 1",
            WindowWidth = 1280,
            WindowHeight = 720,
            VSync = true,
            VerboseLogging = true
        };

        if (config.VerboseLogging)
        {
            Logger.SetMinLevel(Logger.LogLevel.Debug);
        }

        try
        {
            Initialize(config);
            Run();
            Shutdown();
        }
        catch (Exception ex)
        {
            Logger.Fatal($"Unhandled exception: {ex.Message}");
            Logger.Fatal(ex.StackTrace ?? "No stack trace");
            Environment.Exit(1);
        }

        Logger.Info("Engine terminated successfully.");
    }

    private static void Initialize(EngineConfig config)
    {
        Logger.Info("Initializing engine modules...");

        _time = new Time();
        _platform = new PlatformModule();
        _platform.Initialize();
        _platform.CreateWindow(config);

        _graphics = new GraphicsModule(_platform);
        
        // Подключаем события окна
        _platform.OnLoad += OnLoad;
        _platform.OnUpdate += OnUpdate;
        _platform.OnRender += OnRender;
        _platform.OnClosing += OnClosing;

        Logger.Info("Engine modules initialized.");
    }

    private static void OnLoad()
    {
        Logger.Info("OnLoad: Initializing graphics...");
        _graphics?.Initialize();
        
        // Устанавливаем приятный темно-синий цвет фона
        _graphics?.SetClearColor(0.1f, 0.1f, 0.15f, 1.0f);
    }

    private static void OnUpdate(double deltaTime)
    {
        _time?.Tick();
        
        // Логируем FPS каждые 2 секунды
        if (_time != null && _time.FrameCount % 120 == 0)
        {
            Logger.Debug($"FPS: {_time.FPS:F1} | Frame: {_time.FrameCount} | Time: {_time.ElapsedTime:F2}s");
        }

        // Обновляем модули
        _platform?.Update((float)deltaTime);
        _graphics?.Update((float)deltaTime);
    }

    private static void OnRender(double deltaTime)
    {
        _graphics?.BeginFrame();
        // TODO: Рендеринг спрайтов, UI и т.д. будет в следующих milestone
        _graphics?.EndFrame();
    }

    private static void OnClosing()
    {
        Logger.Info("Window close requested.");
    }

    private static void Run()
    {
        Logger.Info("Starting main loop...");
        _platform?.Run();
    }

    private static void Shutdown()
    {
        Logger.Info("Shutting down engine modules...");
        
        _graphics?.Shutdown();
        _platform?.Shutdown();
        
        _graphics = null;
        _platform = null;
        _time = null;
    }
}