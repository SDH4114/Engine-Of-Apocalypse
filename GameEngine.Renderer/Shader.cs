using System;
using System.IO;

namespace GameEngine.Core
{
    public sealed class Shader
    {
        public string VertexPath { get; }
        public string FragmentPath { get; }

        private Shader(string vertexPath, string fragmentPath)
        {
            VertexPath = vertexPath;
            FragmentPath = fragmentPath;
        }

        public static Shader LoadFromFile(string vertexPath, string fragmentPath)
        {
            vertexPath = ResolvePath(vertexPath);
            fragmentPath = ResolvePath(fragmentPath);

            if (!File.Exists(vertexPath))
            {
                throw new FileNotFoundException($"Shader not found: {vertexPath}");
            }

            if (!File.Exists(fragmentPath))
            {
                throw new FileNotFoundException($"Shader not found: {fragmentPath}");
            }

            return new Shader(vertexPath, fragmentPath);
        }

        private static string ResolvePath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return path;
            }

            if (Path.IsPathRooted(path))
            {
                return path;
            }

            // 1) Try relative to output folder (where the app runs)
            var baseDirCandidate = Path.Combine(AppContext.BaseDirectory, path);
            if (File.Exists(baseDirCandidate))
            {
                return baseDirCandidate;
            }

            // 2) Try relative to current working directory
            var cwdCandidate = Path.GetFullPath(path);
            return cwdCandidate;
        }
    }
}
