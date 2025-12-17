// ============================================================================
// АУДИО КЛИП
// ============================================================================

namespace GameEngine.Core
{
    // ========================================================================
    // АУДИО КЛИП
    // ========================================================================
    public class AudioClip
    {
        public string Name { get; set; }
        public float Duration { get; set; }
        public int SampleRate { get; set; }
        public int Channels { get; set; }
        internal byte[] Data { get; set; }
    }
}
