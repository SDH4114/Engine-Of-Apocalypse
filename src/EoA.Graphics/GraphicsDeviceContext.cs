using System;
using EoA.Core;
using Veldrid;

namespace EoA.Graphics;

public sealed class GraphicsDeviceContext : IGraphicsDeviceContext
{
    private readonly GraphicsDevice _graphicsDevice;
    private readonly CommandList _commandList;

    public GraphicsDeviceContext(GraphicsDevice graphicsDevice)
    {
        _graphicsDevice = graphicsDevice ?? throw new ArgumentNullException(nameof(graphicsDevice));
        _commandList = _graphicsDevice.ResourceFactory.CreateCommandList();

        Width = _graphicsDevice.SwapchainFramebuffer.Width;
        Height = _graphicsDevice.SwapchainFramebuffer.Height;
    }

    public uint Width { get; private set; }
    public uint Height { get; private set; }

    public void Clear(Color color)
    {
        _commandList.Begin();
        _commandList.SetFramebuffer(_graphicsDevice.SwapchainFramebuffer);
        _commandList.ClearColorTarget(0, ToRgbaFloat(color));
        _commandList.ClearDepthStencil(1f);
        _commandList.End();

        _graphicsDevice.SubmitCommands(_commandList);
    }

    public void Present() => _graphicsDevice.SwapBuffers();

    public void Resize(uint width, uint height)
    {
        Width = width;
        Height = height;
    }

    public void Dispose()
    {
        _commandList.Dispose();
        _graphicsDevice.Dispose();
    }

    private static RgbaFloat ToRgbaFloat(Color color) => new(color.R, color.G, color.B, color.A);
}
