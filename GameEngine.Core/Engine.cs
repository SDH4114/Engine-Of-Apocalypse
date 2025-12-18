// ============================================================================
// ЯДРО ИГРОВОГО ДВИЖКА
// ============================================================================

using System;
using System.Diagnostics;

namespace GameEngine.Core
{
    // ========================================================================
    // ГЛАВНЫЙ КЛАСС ДВИЖКА
    // ========================================================================
    public class Engine
    {
        private static Engine _instance;
        public static Engine Instance => _instance ??= new Engine();

        public Window Window { get; private set; }
        public Renderer Renderer { get; private set; }
        public PhysicsWorld Physics { get; private set; }
        public SceneManager SceneManager { get; private set; }
        public InputManager Input { get; private set; }
        public AudioManager Audio { get; private set; }
        public AssetManager Assets { get; private set; }
        public UIManager UI { get; private set; }
        public Profiler Profiler { get; private set; }

        private bool _isRunning;
        private double _targetFPS = 60.0;
        private double _fixedTimeStep = 1.0 / 60.0;

        public void Initialize(EngineConfig config)
        {
            Profiler = new Profiler();
            
            Window = new Window(config.Width, config.Height, config.Title);
            Renderer = new Renderer(config.GraphicsAPI, Window);
            Physics = new PhysicsWorld();
            SceneManager = new SceneManager();
            Input = new InputManager();
            Audio = new AudioManager();
            Assets = new AssetManager();
            UI = new UIManager();

            Console.WriteLine("✓ Engine initialized successfully");
        }

        public void Run()
        {
            _isRunning = true;
            double accumulator = 0.0;

            var shouldShutdown = false;

            Window.Run(
                onUpdate: dt =>
                {
                    if (!_isRunning) return;

                    accumulator += dt;

                    Profiler.BeginSection("Events");
                    Input.Update();
                    Profiler.EndSection();

                    Profiler.BeginSection("Physics");
                    while (accumulator >= _fixedTimeStep)
                    {
                        Physics.Step((float)_fixedTimeStep);
                        accumulator -= _fixedTimeStep;
                    }
                    Profiler.EndSection();

                    Profiler.BeginSection("Update");
                    SceneManager.Update(dt);
                    UI.Update(dt);
                    Profiler.EndSection();
                },
                onRender: dt =>
                {
                    if (!_isRunning) return;

                    Profiler.BeginFrame();

                    Profiler.BeginSection("Render");
                    Renderer.BeginFrame();
                    SceneManager.Render(Renderer);
                    UI.Render(Renderer);
                    Renderer.EndFrame();
                    Profiler.EndSection();

                    Profiler.EndFrame();
                    Profiler.MaybePrintStats();
                },
                onClose: () =>
                {
                    _isRunning = false;
                    shouldShutdown = true;
                });

            if (shouldShutdown)
            {
                Shutdown();
            }
        }

        private void Shutdown()
        {
            Assets.Dispose();
            Audio.Dispose();
            Renderer.Dispose();
            Window.Dispose();
            Console.WriteLine("✓ Engine shutdown complete");
        }

        private double GetTime() => Stopwatch.GetTimestamp() / (double)Stopwatch.Frequency;

        public void Stop() => _isRunning = false;
    }

    // ========================================================================
    // КОНФИГУРАЦИЯ ДВИЖКА
    // ========================================================================
    public class EngineConfig
    {
        public int Width { get; set; } = 1920;
        public int Height { get; set; } = 1080;
        public string Title { get; set; } = "Game Engine";
        public GraphicsAPI GraphicsAPI { get; set; } = GraphicsAPI.OpenGL;
        public bool VSync { get; set; } = true;
        public int MSAA { get; set; } = 4;
    }

    public enum GraphicsAPI
    {
        OpenGL,
        Vulkan,
        DirectX11
    }
}