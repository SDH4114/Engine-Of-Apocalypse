using System.Runtime.CompilerServices;

namespace EoA.Core;

/// <summary>
/// Structured logging system.
/// Thread-safe, minimal allocation design.
/// </summary>
public static class Logger
{
    private static readonly object _lock = new();
    private static LogLevel _minLevel = LogLevel.Info;

    public enum LogLevel
    {
        Trace,
        Debug,
        Info,
        Warning,
        Error,
        Fatal
    }

    public static void SetMinLevel(LogLevel level)
    {
        lock (_lock)
        {
            _minLevel = level;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Trace(string message) => Log(LogLevel.Trace, message);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Debug(string message) => Log(LogLevel.Debug, message);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Info(string message) => Log(LogLevel.Info, message);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Warning(string message) => Log(LogLevel.Warning, message);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Error(string message) => Log(LogLevel.Error, message);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Fatal(string message) => Log(LogLevel.Fatal, message);

    private static void Log(LogLevel level, string message)
    {
        if (level < _minLevel) return;

        lock (_lock)
        {
            var timestamp = DateTime.UtcNow.ToString("HH:mm:ss.fff");
            var levelStr = level switch
            {
                LogLevel.Trace => "TRC",
                LogLevel.Debug => "DBG",
                LogLevel.Info => "INF",
                LogLevel.Warning => "WRN",
                LogLevel.Error => "ERR",
                LogLevel.Fatal => "FTL",
                _ => "???"
            };

            var color = level switch
            {
                LogLevel.Error or LogLevel.Fatal => ConsoleColor.Red,
                LogLevel.Warning => ConsoleColor.Yellow,
                LogLevel.Debug => ConsoleColor.Gray,
                _ => ConsoleColor.White
            };

            Console.ForegroundColor = color;
            Console.WriteLine($"[{timestamp}] [{levelStr}] {message}");
            Console.ResetColor();
        }
    }
}