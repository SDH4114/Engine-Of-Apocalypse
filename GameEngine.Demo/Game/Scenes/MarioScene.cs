using GameEngine.Core;
using GameEngine.Demo.Game.Components;
using System.Numerics;

namespace GameEngine.Demo.Game.Scenes;

public sealed class MarioScene : Scene
{
    private Shader? _shader;

    public override void OnLoad()
    {
        Name = "Mario (2D)";

        var window = Engine.Instance.Window;
        var aspect = 16f / 9f;
        if (window != null)
        {
            aspect = window.Size.Y == 0 ? aspect : (float)window.Size.X / window.Size.Y;
        }

        var cameraObj = new GameObject("Camera");
        var camera = cameraObj.AddComponent<Camera>();
        camera.Orthographic = true;
        camera.OrthoSize = 6f;
        camera.AspectRatio = aspect;
        cameraObj.Transform.Position = new Vector3(0, 0, 10);
        MainCamera = camera;
        AddGameObject(cameraObj);

        _shader = Engine.Instance.Assets?.LoadShader("shaders/standard.vert", "shaders/standard.frag");

        CreatePlatform("Ground", new Vector2(0f, -3.5f), new Vector2(30f, 1.5f), new Vector3(0.35f, 0.35f, 0.35f));
        CreatePlatform("Block1", new Vector2(-4f, -1f), new Vector2(4f, 1f), new Vector3(0.55f, 0.35f, 0.2f));
        CreatePlatform("Block2", new Vector2(5f, 0f), new Vector2(6f, 1f), new Vector3(0.55f, 0.35f, 0.2f));

        CreatePlayer(new Vector2(-8f, -2f));
    }

    private void CreatePlayer(Vector2 pos)
    {
        var player = new GameObject("Player");
        player.Transform.Position = new Vector3(pos.X, pos.Y, 0f);
        player.Transform.Scale = new Vector3(0.8f, 1.1f, 0.2f);

        var mr = player.AddComponent<MeshRenderer>();
        mr.Mesh = Mesh.CreateCube();
        mr.Material = new Material
        {
            Shader = _shader,
            Albedo = new Vector3(0.9f, 0.1f, 0.1f),
            Metallic = 0f,
            Roughness = 1f
        };

        var body = player.AddComponent<RigidBody>();
        body.Mass = 1f;
        body.UseGravity = true;
        body.LinearDamping = 0.0f;

        var col = player.AddComponent<BoxCollider>();
        col.Size = Vector3.One;

        player.AddComponent<PlayerController2D>();

        AddGameObject(player);
    }

    private void CreatePlatform(string name, Vector2 pos, Vector2 size, Vector3 color)
    {
        var obj = new GameObject(name);
        obj.Transform.Position = new Vector3(pos.X, pos.Y, 0f);
        obj.Transform.Scale = new Vector3(size.X, size.Y, 0.2f);

        var mr = obj.AddComponent<MeshRenderer>();
        mr.Mesh = Mesh.CreateCube();
        mr.Material = new Material
        {
            Shader = _shader,
            Albedo = color,
            Metallic = 0f,
            Roughness = 1f
        };

        var body = obj.AddComponent<RigidBody>();
        body.IsKinematic = true;
        body.UseGravity = false;

        var col = obj.AddComponent<BoxCollider>();
        col.Size = Vector3.One;

        AddGameObject(obj);
    }

    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);

        if (Engine.Instance.Input?.IsKeyPressed(KeyCode.Escape) == true)
        {
            Engine.Instance.Stop();
        }
    }
}
