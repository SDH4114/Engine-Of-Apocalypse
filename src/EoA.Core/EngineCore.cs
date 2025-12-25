using System;

namespace EoA.Core;

public sealed class EngineConfig
{
    public string WindowTitle { get; init; } = "Engine Of Apocalypse";
    public int WindowWidth { get; init; } = 1280;
    public int WindowHeight { get; init; } = 720;
    public bool VSync { get; init; } = true;
}

public readonly record struct FrameTime(double DeltaSeconds, double TotalSeconds);

public readonly record struct Color(float R, float G, float B, float A)
{
    public static readonly Color Black = new(0f, 0f, 0f, 1f);
    public static readonly Color CornflowerBlue = new(0.392f, 0.584f, 0.929f, 1f);

    public static Color FromRgb(byte r, byte g, byte b, byte a = 255)
    {
        const float inv = 1f / 255f;
        return new Color(r * inv, g * inv, b * inv, a * inv);
    }
}

public interface IGraphicsDeviceContext : IDisposable
{
    uint Width { get; }
    uint Height { get; }
    void Clear(Color color);
    void Present();
    void Resize(uint width, uint height);
}

public sealed class EngineContext
{
    public EngineContext(EngineConfig config, IGraphicsDeviceContext graphics)
    {
        Config = config ?? throw new ArgumentNullException(nameof(config));
        Graphics = graphics ?? throw new ArgumentNullException(nameof(graphics));
    }

    public EngineConfig Config { get; }
    public IGraphicsDeviceContext Graphics { get; }
}

public interface IGame
{
    void Initialize(EngineContext context);
    void Update(FrameTime frameTime);
    void Render(FrameTime frameTime);
    void Shutdown();
}
