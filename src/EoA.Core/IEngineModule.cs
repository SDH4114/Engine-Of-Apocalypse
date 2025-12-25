namespace EoA.Core;

/// <summary>
/// Base interface for all engine modules (graphics, physics, audio, etc.)
/// Provides lifecycle hooks for initialization, update, and shutdown.
/// </summary>
public interface IEngineModule
{
    /// <summary>Module name for logging and diagnostics</summary>
    string Name { get; }
    
    /// <summary>Initialize the module. Called once at startup.</summary>
    void Initialize();
    
    /// <summary>Update the module. Called every frame.</summary>
    /// <param name="deltaTime">Time since last frame in seconds</param>
    void Update(float deltaTime);
    
    /// <summary>Shutdown and cleanup. Called once at exit.</summary>
    void Shutdown();
}