using System;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;

namespace EngineOfApocalypse.Renderer;

public sealed class OpenGLRenderSystem
{
    private GL? _gl;

    private uint _vao;
    private uint _vbo;
    private uint _program;

    public void Initialize(IWindow window)
    {
        _gl = window.CreateOpenGL();

        _gl.Enable(EnableCap.DepthTest);

        CreateTriangleResources();
    }

    public void BeginFrame(float r, float g, float b, float a)
    {
        if (_gl is null) return;

        _gl.ClearColor(r, g, b, a);
        _gl.Clear((uint)(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit));

        _gl.UseProgram(_program);
        _gl.BindVertexArray(_vao);
        _gl.DrawArrays(PrimitiveType.Triangles, 0, 3);
        _gl.BindVertexArray(0);
        _gl.UseProgram(0);
    }

    private unsafe void CreateTriangleResources()
    {
        if (_gl is null) return;

        const string vertSrc = "#version 330 core\n" +
                               "layout(location = 0) in vec2 aPos;\n" +
                               "layout(location = 1) in vec3 aColor;\n" +
                               "out vec3 vColor;\n" +
                               "void main() { vColor = aColor; gl_Position = vec4(aPos, 0.0, 1.0); }\n";

        const string fragSrc = "#version 330 core\n" +
                               "in vec3 vColor;\n" +
                               "out vec4 FragColor;\n" +
                               "void main() { FragColor = vec4(vColor, 1.0); }\n";

        var vs = CompileShader(ShaderType.VertexShader, vertSrc);
        var fs = CompileShader(ShaderType.FragmentShader, fragSrc);

        _program = _gl.CreateProgram();
        _gl.AttachShader(_program, vs);
        _gl.AttachShader(_program, fs);
        _gl.LinkProgram(_program);
        _gl.GetProgram(_program, GLEnum.LinkStatus, out var linkStatus);
        if (linkStatus == 0)
        {
            var info = _gl.GetProgramInfoLog(_program);
            throw new InvalidOperationException($"Failed to link shader program: {info}");
        }
        _gl.DetachShader(_program, vs);
        _gl.DetachShader(_program, fs);
        _gl.DeleteShader(vs);
        _gl.DeleteShader(fs);

        // Interleaved: vec2 position, vec3 color
        float[] vertices =
        [
            // x, y,    r, g, b
            0.0f,  0.6f,  1.0f, 0.2f, 0.2f,
            -0.6f, -0.6f, 0.2f, 1.0f, 0.2f,
            0.6f,  -0.6f, 0.2f, 0.2f, 1.0f,
        ];

        _vao = _gl.GenVertexArray();
        _vbo = _gl.GenBuffer();

        _gl.BindVertexArray(_vao);
        _gl.BindBuffer(BufferTargetARB.ArrayBuffer, _vbo);

        fixed (float* p = vertices)
        {
            _gl.BufferData(BufferTargetARB.ArrayBuffer, (nuint)(vertices.Length * sizeof(float)), p, BufferUsageARB.StaticDraw);
        }

        var stride = 5 * sizeof(float);
        _gl.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, (uint)stride, (void*)0);
        _gl.EnableVertexAttribArray(0);
        _gl.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, (uint)stride, (void*)(2 * sizeof(float)));
        _gl.EnableVertexAttribArray(1);

        _gl.BindBuffer(BufferTargetARB.ArrayBuffer, 0);
        _gl.BindVertexArray(0);
    }

    private uint CompileShader(ShaderType type, string source)
    {
        if (_gl is null) throw new InvalidOperationException("GL is not initialized.");

        var shader = _gl.CreateShader(type);
        _gl.ShaderSource(shader, source);
        _gl.CompileShader(shader);
        _gl.GetShader(shader, ShaderParameterName.CompileStatus, out var status);
        if (status == 0)
        {
            var info = _gl.GetShaderInfoLog(shader);
            throw new InvalidOperationException($"Failed to compile {type}: {info}");
        }
        return shader;
    }
}
