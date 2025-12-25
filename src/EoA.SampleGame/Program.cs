using EoA.Core;
using EoA.Platform;

var config = new EngineConfig
{
    WindowTitle = "Engine Of Apocalypse - SampleGame",
    WindowWidth = 1280,
    WindowHeight = 720,
    VSync = true
};

var game = new SampleGame();
using var host = new EngineHost(config, game);
host.Run();

internal sealed class SampleGame : IGame
{
    private EngineContext _context = null!;

    public void Initialize(EngineContext context)
    {
        _context = context;
    }

    public void Update(FrameTime frameTime)
    {
        // Game update loop will live here; left empty for bootstrap milestone.
    }

    public void Render(FrameTime frameTime)
    {
        _context.Graphics.Clear(Color.CornflowerBlue);
        _context.Graphics.Present();
    }

    public void Shutdown()
    {
        // Nothing to tear down yet.
    }
}
