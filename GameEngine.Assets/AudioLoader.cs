// ============================================================================
// ЗАГРУЗЧИК АУДИО
// ============================================================================

using System;
using System.IO;
using System.Threading.Tasks;

namespace GameEngine.Core
{
    // ========================================================================
    // ЗАГРУЗЧИК АУДИО
    // ========================================================================
    public class AudioLoader
    {
        public async Task<AudioClip> LoadAsync(string path)
        {
            return await Task.Run(() => Load(path));
        }

        public AudioClip Load(string path)
        {
            var extension = Path.GetExtension(path).ToLower();

            return extension switch
            {
                ".wav" => LoadWAV(path),
                ".mp3" => LoadMP3(path),
                ".ogg" => LoadOGG(path),
                _ => throw new NotSupportedException($"Audio format {extension} not supported")
            };
        }

        private AudioClip LoadWAV(string path)
        {
            // Используйте NAudio или NVorbis для загрузки аудио
            return new AudioClip { Name = Path.GetFileName(path) };
        }

        private AudioClip LoadMP3(string path)
        {
            Console.WriteLine("MP3 loading requires NAudio library");
            return new AudioClip { Name = Path.GetFileName(path) };
        }

        private AudioClip LoadOGG(string path)
        {
            Console.WriteLine("OGG loading requires NVorbis library");
            return new AudioClip { Name = Path.GetFileName(path) };
        }
    }
}

