// ============================================================================
// ПРОФАЙЛЕР
// ============================================================================

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace GameEngine.Core
{
    // ========================================================================
    // ПРОФАЙЛЕР
    // ========================================================================
    public class Profiler
    {
        private Dictionary<string, ProfilingData> _sections = new Dictionary<string, ProfilingData>();
        private Stack<string> _sectionStack = new Stack<string>();
        private Stopwatch _frameTimer = new Stopwatch();
        private Stopwatch _printTimer = Stopwatch.StartNew();
        private double _frameTime;
        private int _fps;
        public long FrameCount { get; private set; }

        public void BeginFrame()
        {
            _frameTimer.Restart();
        }

        public void BeginSection(string name)
        {
            if (!_sections.ContainsKey(name))
                _sections[name] = new ProfilingData();
            
            _sections[name].Start();
            _sectionStack.Push(name);
        }

        public void EndSection()
        {
            if (_sectionStack.Count > 0)
            {
                var name = _sectionStack.Pop();
                _sections[name].Stop();
            }
        }

        public void EndFrame()
        {
            _frameTime = _frameTimer.Elapsed.TotalMilliseconds;
            _fps = (int)(1000.0 / _frameTime);
            FrameCount++;
        }

        public void MaybePrintStats(double intervalSeconds = 3.0)
        {
            if (_printTimer.Elapsed.TotalSeconds < intervalSeconds)
            {
                return;
            }

            _printTimer.Restart();
            PrintStats();
        }

        public void PrintStats()
        {
            Console.WriteLine($"FPS: {_fps} | Frame: {_frameTime:F2}ms");
            foreach (var kvp in _sections)
            {
                Console.WriteLine($"  {kvp.Key}: {kvp.Value.AverageTime:F2}ms");
            }
        }
    }

    public class ProfilingData
    {
        private Stopwatch _timer = new Stopwatch();
        private Queue<double> _samples = new Queue<double>(60);
        
        public double AverageTime => _samples.Count > 0 ? _samples.Average() : 0;

        public void Start() => _timer.Restart();

        public void Stop()
        {
            _timer.Stop();
            _samples.Enqueue(_timer.Elapsed.TotalMilliseconds);
            if (_samples.Count > 60) _samples.Dequeue();
        }
    }
}