using System;
using System.IO;
using System.Numerics;
using Silk.NET.OpenGL;

namespace GameEngine.Core
{
    public sealed class Shader
    {
        public string VertexPath { get; }
        public string FragmentPath { get; }

        private readonly string _vertexSource;
        private readonly string _fragmentSource;

        private uint _program;
        private bool _compiled;

        private Shader(string vertexPath, string fragmentPath, string vertexSource, string fragmentSource)
        {
            VertexPath = vertexPath;
            FragmentPath = fragmentPath;
            _vertexSource = vertexSource;
            _fragmentSource = fragmentSource;
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

            var vs = File.ReadAllText(vertexPath);
            var fs = File.ReadAllText(fragmentPath);
            return new Shader(vertexPath, fragmentPath, vs, fs);
        }

        public void Use(GL gl)
        {
            EnsureCompiled(gl);
            gl.UseProgram(_program);
        }

        public void SetMatrix4(GL gl, string name, Matrix4x4 value)
        {
            EnsureCompiled(gl);
            var location = gl.GetUniformLocation(_program, name);
            if (location < 0) return;

            var m = Matrix4x4.Transpose(value);
            gl.UniformMatrix4(location, 1, false, new[]
            {
                m.M11, m.M12, m.M13, m.M14,
                m.M21, m.M22, m.M23, m.M24,
                m.M31, m.M32, m.M33, m.M34,
                m.M41, m.M42, m.M43, m.M44
            });
        }

        public void SetVector3(GL gl, string name, Vector3 value)
        {
            EnsureCompiled(gl);
            var location = gl.GetUniformLocation(_program, name);
            if (location < 0) return;
            gl.Uniform3(location, value.X, value.Y, value.Z);
        }

        private void EnsureCompiled(GL gl)
        {
            if (_compiled)
            {
                return;
            }

            var vertexShader = gl.CreateShader(ShaderType.VertexShader);
            gl.ShaderSource(vertexShader, _vertexSource);
            gl.CompileShader(vertexShader);
            gl.GetShader(vertexShader, ShaderParameterName.CompileStatus, out var vsStatus);
            if (vsStatus == 0)
            {
                var log = gl.GetShaderInfoLog(vertexShader);
                gl.DeleteShader(vertexShader);
                throw new InvalidOperationException($"Vertex shader compilation failed: {log}");
            }

            var fragmentShader = gl.CreateShader(ShaderType.FragmentShader);
            gl.ShaderSource(fragmentShader, _fragmentSource);
            gl.CompileShader(fragmentShader);
            gl.GetShader(fragmentShader, ShaderParameterName.CompileStatus, out var fsStatus);
            if (fsStatus == 0)
            {
                var log = gl.GetShaderInfoLog(fragmentShader);
                gl.DeleteShader(vertexShader);
                gl.DeleteShader(fragmentShader);
                throw new InvalidOperationException($"Fragment shader compilation failed: {log}");
            }

            _program = gl.CreateProgram();
            gl.AttachShader(_program, vertexShader);
            gl.AttachShader(_program, fragmentShader);
            gl.LinkProgram(_program);

            gl.GetProgram(_program, ProgramPropertyARB.LinkStatus, out var linkStatus);
            if (linkStatus == 0)
            {
                var log = gl.GetProgramInfoLog(_program);
                gl.DeleteShader(vertexShader);
                gl.DeleteShader(fragmentShader);
                gl.DeleteProgram(_program);
                _program = 0;
                throw new InvalidOperationException($"Shader link failed: {log}");
            }

            gl.DetachShader(_program, vertexShader);
            gl.DetachShader(_program, fragmentShader);
            gl.DeleteShader(vertexShader);
            gl.DeleteShader(fragmentShader);

            _compiled = true;
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
