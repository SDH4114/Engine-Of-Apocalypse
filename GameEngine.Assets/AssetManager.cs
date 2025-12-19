// ============================================================================
// СИСТЕМА УПРАВЛЕНИЯ АССЕТАМИ
// ============================================================================

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameEngine.Core
{
    // ========================================================================
    // МЕНЕДЖЕР АССЕТОВ
    // ========================================================================
    public class AssetManager : IDisposable
    {
        private Dictionary<string, Mesh> _meshes = new Dictionary<string, Mesh>();
        private Dictionary<string, Texture> _textures = new Dictionary<string, Texture>();
        private Dictionary<string, Shader> _shaders = new Dictionary<string, Shader>();
        private Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();
        private Dictionary<string, Material> _materials = new Dictionary<string, Material>();

        private ModelLoader _modelLoader;
        private TextureLoader _textureLoader;
        private AudioLoader _audioLoader;

        public AssetManager()
        {
            _modelLoader = new ModelLoader();
            _textureLoader = new TextureLoader();
            _audioLoader = new AudioLoader();
            
            Console.WriteLine("✓ Asset Manager initialized");
        }

        // ====================================================================
        // ЗАГРУЗКА МОДЕЛЕЙ
        // ====================================================================
        public async Task<Mesh> LoadMeshAsync(string path)
        {
            if (_meshes.TryGetValue(path, out var cached))
                return cached;

            var mesh = await _modelLoader.LoadAsync(path);
            _meshes[path] = mesh;
            Console.WriteLine($"✓ Loaded mesh: {path}");
            return mesh;
        }

        public Mesh LoadMesh(string path)
        {
            if (_meshes.TryGetValue(path, out var cached))
                return cached;

            var mesh = _modelLoader.Load(path);
            _meshes[path] = mesh;
            Console.WriteLine($"✓ Loaded mesh: {path}");
            return mesh;
        }

        // ====================================================================
        // ЗАГРУЗКА ТЕКСТУР
        // ====================================================================
        public async Task<Texture> LoadTextureAsync(string path, TextureSettings? settings = null)
        {
            if (_textures.TryGetValue(path, out var cached))
                return cached;

            var texture = await _textureLoader.LoadAsync(path, settings ?? TextureSettings.Default);
            _textures[path] = texture;
            Console.WriteLine($"✓ Loaded texture: {path}");
            return texture;
        }

        public Texture LoadTexture(string path, TextureSettings? settings = null)
        {
            if (_textures.TryGetValue(path, out var cached))
                return cached;

            var texture = _textureLoader.Load(path, settings ?? TextureSettings.Default);
            _textures[path] = texture;
            Console.WriteLine($"✓ Loaded texture: {path}");
            return texture;
        }

        // ====================================================================
        // ЗАГРУЗКА ШЕЙДЕРОВ
        // ====================================================================
        public Shader LoadShader(string vertexPath, string fragmentPath, string? name = null)
        {
            name ??= $"{vertexPath}_{fragmentPath}";
            
            if (_shaders.TryGetValue(name, out var cached))
                return cached;

            var shader = Shader.LoadFromFile(vertexPath, fragmentPath);
            _shaders[name] = shader;
            Console.WriteLine($"✓ Loaded shader: {name}");
            return shader;
        }

        // ====================================================================
        // ЗАГРУЗКА АУДИО
        // ====================================================================
        public async Task<AudioClip> LoadAudioAsync(string path)
        {
            if (_audioClips.TryGetValue(path, out var cached))
                return cached;

            var clip = await _audioLoader.LoadAsync(path);
            _audioClips[path] = clip;
            Console.WriteLine($"✓ Loaded audio: {path}");
            return clip;
        }

        // ====================================================================
        // СОЗДАНИЕ МАТЕРИАЛОВ
        // ====================================================================
        public Material CreateMaterial(string name, Shader shader)
        {
            var material = new Material { Shader = shader };
            _materials[name] = material;
            return material;
        }

        public Material? GetMaterial(string name)
        {
            return _materials.TryGetValue(name, out var mat) ? mat : null;
        }

        // ====================================================================
        // ОЧИСТКА
        // ====================================================================
        public void Dispose()
        {
            _meshes.Clear();
            _textures.Clear();
            _shaders.Clear();
            _audioClips.Clear();
            _materials.Clear();
        }
    }
}