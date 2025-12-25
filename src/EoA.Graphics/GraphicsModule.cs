using EoA.Core;
using EoA.Platform;
using Veldrid;
using Veldrid.OpenGL;

namespace EoA.Graphics;

/// <summary>
/// Графический модуль: управляет устройством рендеринга и отправкой команд.
/// Построен на Veldrid для кроссплатформенного рендеринга.
/// </summary>
public sealed class GraphicsModule : IEngineModule
{
    private GraphicsDevice? _device;
    private CommandList? _commandList;
    private readonly PlatformModule _platform;
    private RgbaFloat _clearColor = new(0.1f, 0.1f, 0.15f, 1.0f);

    public string Name => "Graphics";
    public GraphicsDevice Device => _device ?? throw new InvalidOperationException("Graphics device not initialized");
    public CommandList CommandList => _commandList ?? throw new InvalidOperationException("Command list not initialized");

    public GraphicsModule(PlatformModule platform)
    {
        _platform = platform;
    }

    public void Initialize()
    {
        Logger.Info($"[{Name}] Initializing graphics module...");

        var window = _platform.Window;
        
        var options = new GraphicsDeviceOptions
        {
            PreferStandardClipSpaceYDirection = true,
            PreferDepthRangeZeroToOne = true,
            Debug = false,
            SyncToVerticalBlank = true,
            SwapchainDepthFormat = PixelFormat.R16_UNorm
        };

        // Создаем устройство на основе платформы
        _device = CreateGraphicsDevice(window, options);
        _commandList = _device.ResourceFactory.CreateCommandList();

        Logger.Info($"[{Name}] Graphics device created: {_device.BackendType}");
    }

    private static GraphicsDevice CreateGraphicsDevice(Silk.NET.Windowing.IWindow window, GraphicsDeviceOptions options)
    {
        var width = (uint)window.Size.X;
        var height = (uint)window.Size.Y;

        // Пробуем создать устройство в зависимости от платформы
        try
        {
            if (OperatingSystem.IsMacOS())
            {
                Logger.Info("Attempting to create Metal device...");
                var swapchainSource = GetSwapchainSource(window);
                var swapchainDescription = new SwapchainDescription(
                    swapchainSource, width, height, PixelFormat.R32_Float, true);
                return GraphicsDevice.CreateMetal(options, swapchainDescription);
            }
            
            // Для Windows и Linux пробуем Vulkan
            Logger.Info("Attempting to create Vulkan device...");
            var swapchainSourceVk = GetSwapchainSource(window);
            var swapchainDescriptionVk = new SwapchainDescription(
                swapchainSourceVk, width, height, PixelFormat.R32_Float, true);
            return GraphicsDevice.CreateVulkan(options, swapchainDescriptionVk);
        }
        catch (Exception ex)
        {
            Logger.Warning($"Failed to create preferred backend: {ex.Message}");
            Logger.Info("Falling back to OpenGL...");
            
            // Fallback на OpenGL если Vulkan/Metal не работает
            try
            {
                var platformInfo = new OpenGLPlatformInfo(
                    openGLContextHandle: window.GLContext!.Handle,
                    getProcAddress: name => { window.GLContext.TryGetProcAddress(name, out var addr); return addr; },
                    makeCurrent: _ => window.GLContext.MakeCurrent(),
                    getCurrentContext: () => window.GLContext.Handle,
                    clearCurrentContext: window.GLContext.Clear,
                    deleteContext: _ => window.GLContext.Dispose(),
                    swapBuffers: window.GLContext.SwapBuffers,
                    setSyncToVerticalBlank: vsync => window.GLContext.SwapInterval(vsync ? 1 : 0),
                    setSwapchainFramebuffer: () => { },
                    resizeSwapchain: (w, h) => { });

                return GraphicsDevice.CreateOpenGL(options, platformInfo, width, height);
            }
            catch (Exception glEx)
            {
                Logger.Error($"Failed to create OpenGL device: {glEx.Message}");
                throw new Exception("Could not create graphics device with any backend!", glEx);
            }
        }
    }

    private static SwapchainSource GetSwapchainSource(Silk.NET.Windowing.IWindow window)
    {
        // Windows
        var win32View = window.Native?.Win32;
        if (win32View.HasValue)
        {
            return SwapchainSource.CreateWin32(win32View.Value.Hwnd, win32View.Value.HInstance);
        }

        // Linux X11
        var x11View = window.Native?.X11;
        if (x11View.HasValue)
        {
            return SwapchainSource.CreateXlib(
                (nint)x11View.Value.Display,
                (nint)x11View.Value.Window);
        }

        // Linux Wayland
        var waylandView = window.Native?.Wayland;
        if (waylandView.HasValue)
        {
            return SwapchainSource.CreateWayland(
                (nint)waylandView.Value.Display,
                (nint)waylandView.Value.Surface);
        }

        // macOS
        var cocoaView = window.Native?.Cocoa;
        if (cocoaView.HasValue)
        {
            return SwapchainSource.CreateNSWindow((nint)cocoaView.Value);
        }

        throw new PlatformNotSupportedException("Cannot create swapchain for this platform!");
    }

    public void SetClearColor(float r, float g, float b, float a = 1.0f)
    {
        _clearColor = new RgbaFloat(r, g, b, a);
    }

    public void BeginFrame()
    {
        if (_device == null || _commandList == null) return;

        _commandList.Begin();
        _commandList.SetFramebuffer(_device.SwapchainFramebuffer);
        _commandList.ClearColorTarget(0, _clearColor);
    }

    public void EndFrame()
    {
        if (_device == null || _commandList == null) return;

        _commandList.End();
        _device.SubmitCommands(_commandList);
        _device.SwapBuffers();
    }

    public void Update(float deltaTime)
    {
        // Обновление графики происходит в BeginFrame/EndFrame
    }

    public void Shutdown()
    {
        Logger.Info($"[{Name}] Shutting down graphics module...");
        
        _commandList?.Dispose();
        _device?.Dispose();
        
        _commandList = null;
        _device = null;
    }
}