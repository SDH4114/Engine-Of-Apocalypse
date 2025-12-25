namespace EoA.Core;

/// <summary>
/// Global configuration for the engine.
/// Immutable after initialization.
/// </summary>
public sealed class EngineConfig
{
    public string WindowTitle { get; init; } = "Engine Of Apocalypse";
    public int WindowWidth { get; init; } = 1280;
    public int WindowHeight { get; init; } = 720;
    public bool VSync { get; init; } = true;
    public bool Fullscreen { get; init; } = false;
    public int TargetFrameRate { get; init; } = 60;
    
    /// <summary>Enable detailed logging for diagnostics</summary>
    public bool VerboseLogging { get; init; } = false;
    
    public static EngineConfig Default => new();
}