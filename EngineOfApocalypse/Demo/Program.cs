using EngineOfApocalypse.Core;

var engine = new ApocalypseEngine();
engine.Run(new EngineConfig
{
    Title = "Engine Of Apocalypse",
    Width = 1280,
    Height = 720,
    VSync = true,
    IconPath = Path.Combine("Assets", "Icons", "englogo.png")
});
