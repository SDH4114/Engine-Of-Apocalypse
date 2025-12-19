// ============================================================================
// ЗАГРУЗЧИК ТЕКСТУР
// ============================================================================

using System;
using System.IO;
using System.Threading.Tasks;

namespace GameEngine.Core
{
    // ========================================================================
    // ЗАГРУЗЧИК ТЕКСТУР
    // ========================================================================
    public class TextureLoader
    {
        public async Task<Texture> LoadAsync(string path, TextureSettings settings)
        {
            return await Task.Run(() => Load(path, settings));
        }

        public Texture Load(string path, TextureSettings settings)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException($"Texture not found: {path}");

            // Используйте StbImageSharp или ImageSharp
            // using var stream = File.OpenRead(path);
            // var image = StbImage.LoadFromStream(stream, StbImage.STBI_rgb_alpha);
            
            var texture = new Texture
            {
                Width = 512,  // Заглушка
                Height = 512,
                // Загрузка в GPU с настройками фильтрации и mipmap
            };

            ApplyTextureSettings(texture, settings);
            return texture;
        }

        private void ApplyTextureSettings(Texture texture, TextureSettings settings)
        {
            // GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)settings.WrapMode);
            // GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)settings.FilterMode);
            
            if (settings.GenerateMipmaps)
            {
                // GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            }
        }
    }

    // ========================================================================
    // НАСТРОЙКИ ТЕКСТУРЫ
    // ========================================================================
    public class TextureSettings
    {
        public TextureWrapMode WrapMode { get; set; } = TextureWrapMode.Repeat;
        public TextureFilterMode FilterMode { get; set; } = TextureFilterMode.Linear;
        public bool GenerateMipmaps { get; set; } = true;
        public bool SRGB { get; set; } = true;

        public static TextureSettings Default => new TextureSettings();
    }

    public enum TextureWrapMode
    {
        Repeat,
        Clamp,
        Mirror
    }

    public enum TextureFilterMode
    {
        Nearest,
        Linear,
        Trilinear
    }
}

