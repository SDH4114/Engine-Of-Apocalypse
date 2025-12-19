namespace EngineOfApocalypse.Core;

public sealed class EngineConfig
{
    public int Width { get; set; } = 1280;
    public int Height { get; set; } = 720;
    public string Title { get; set; } = "Engine Of Apocalypse";
    public bool VSync { get; set; } = true;
    public string? IconPath { get; set; }

    public float ClearR { get; set; } = 0.05f;
    public float ClearG { get; set; } = 0.07f;
    public float ClearB { get; set; } = 0.12f;
    public float ClearA { get; set; } = 1.0f;
}
