using System.Diagnostics;

namespace EoA.Core;

/// <summary>
/// High-precision timing utilities.
/// Tracks delta time, elapsed time, and frame count.
/// </summary>
public sealed class Time
{
    private readonly Stopwatch _stopwatch = Stopwatch.StartNew();
    private long _lastFrameTicks;
    private double _deltaTime;
    private double _elapsedTime;
    private ulong _frameCount;

    public double DeltaTime => _deltaTime;
    public double ElapsedTime => _elapsedTime;
    public ulong FrameCount => _frameCount;
    public double FPS => _deltaTime > 0 ? 1.0 / _deltaTime : 0.0;

    public void Tick()
    {
        var currentTicks = _stopwatch.ElapsedTicks;
        var deltaTicks = currentTicks - _lastFrameTicks;
        _lastFrameTicks = currentTicks;

        _deltaTime = deltaTicks / (double)Stopwatch.Frequency;
        _elapsedTime = currentTicks / (double)Stopwatch.Frequency;
        _frameCount++;
    }

    public void Reset()
    {
        _stopwatch.Restart();
        _lastFrameTicks = 0;
        _deltaTime = 0;
        _elapsedTime = 0;
        _frameCount = 0;
    }
}