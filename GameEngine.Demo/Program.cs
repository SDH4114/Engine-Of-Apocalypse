using GameEngine.Core;
using GameEngine.Demo.Game.Scenes;
using System;
using System.Numerics;

public class MyGameScene : Scene
{
    private GameObject? player;
    private float moveSpeed = 5f;

    public override void OnLoad()
    {
        Name = "My Game";

        // Камера от первого лица
        var camera = new GameObject("Camera");
        var cam = camera.AddComponent<Camera>();
        cam.FOV = 75f;
        cam.AspectRatio = 16f / 9f;
        
        camera.Transform.Position = new Vector3(0, 1.6f, 0);
        camera.AddComponent<FirstPersonController>();
        
        MainCamera = cam;
        AddGameObject(camera);

        // Игрок (невидимый коллайдер)
        player = camera;
        var playerBody = player.AddComponent<RigidBody>();
        playerBody.Mass = 70f;
        playerBody.UseGravity = true;
        
        var playerCollider = player.AddComponent<BoxCollider>();
        playerCollider.Size = new Vector3(0.6f, 1.8f, 0.6f);

        // Создаем уровень
        CreateLevel();
        
        // Свет
        CreateLight(new Vector3(5, 10, 5), new Vector3(1, 0.9f, 0.8f), 50f);
    }

    private void CreateLevel()
    {
        // Пол
        CreateCube("Floor", new Vector3(0, -0.5f, 0), new Vector3(20, 1, 20), 
            new Vector3(0.5f, 0.5f, 0.5f), true);

        // Стены
        CreateCube("Wall1", new Vector3(0, 2, -10), new Vector3(20, 4, 1), 
            new Vector3(0.7f, 0.3f, 0.3f), true);
        CreateCube("Wall2", new Vector3(0, 2, 10), new Vector3(20, 4, 1), 
            new Vector3(0.7f, 0.3f, 0.3f), true);
        CreateCube("Wall3", new Vector3(-10, 2, 0), new Vector3(1, 4, 20), 
            new Vector3(0.7f, 0.3f, 0.3f), true);
        CreateCube("Wall4", new Vector3(10, 2, 0), new Vector3(1, 4, 20), 
            new Vector3(0.7f, 0.3f, 0.3f), true);

        // Препятствия
        for (int i = 0; i < 10; i++)
        {
            var random = new Random(i);
            var pos = new Vector3(
                random.Next(-8, 8),
                1,
                random.Next(-8, 8)
            );
            var color = new Vector3(
                (float)random.NextDouble(),
                (float)random.NextDouble(),
                (float)random.NextDouble()
            );
            CreateCube($"Obstacle{i}", pos, Vector3.One, color, false);
        }
    }

    private void CreateCube(string name, Vector3 position, Vector3 scale, 
        Vector3 color, bool isStatic)
    {
        var obj = new GameObject(name);
        obj.Transform.Position = position;
        obj.Transform.Scale = scale;

        var renderer = obj.AddComponent<MeshRenderer>();
        renderer.Mesh = Mesh.CreateCube();
        renderer.Material = new Material
        {
            Albedo = color,
            Metallic = 0.1f,
            Roughness = 0.7f
        };

        var body = obj.AddComponent<RigidBody>();
        body.IsKinematic = isStatic;
        body.UseGravity = !isStatic;

        var collider = obj.AddComponent<BoxCollider>();
        collider.Size = Vector3.One;

        AddGameObject(obj);
    }

    private void CreateLight(Vector3 position, Vector3 color, float intensity)
    {
        var light = new GameObject("Light");
        light.Transform.Position = position;
        
        var lightComp = light.AddComponent<Light>();
        lightComp.Type = LightType.Point;
        lightComp.Color = color;
        lightComp.Intensity = intensity;
        
        AddGameObject(light);
    }

    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);

        // Выход по ESC
        if (Engine.Instance.Input?.IsKeyPressed(KeyCode.Escape) == true)
        {
            Engine.Instance.Stop();
        }

        // Стрельба по пробелу
        if (Engine.Instance.Input?.IsKeyPressed(KeyCode.Space) == true)
        {
            ShootCube();
        }
    }

    private void ShootCube()
    {
        if (player == null)
        {
            return;
        }

        var forward = player.Transform.Forward;
        var spawnPos = player.Transform.Position + forward * 2f;

        var projectile = new GameObject("Projectile");
        projectile.Transform.Position = spawnPos;
        projectile.Transform.Scale = new Vector3(0.3f, 0.3f, 0.3f);

        var renderer = projectile.AddComponent<MeshRenderer>();
        renderer.Mesh = Mesh.CreateCube();
        renderer.Material = new Material
        {
            Albedo = new Vector3(1, 1, 0), // Желтый
            Metallic = 0.8f,
            Roughness = 0.2f
        };

        var body = projectile.AddComponent<RigidBody>();
        body.Mass = 1f;
        body.Velocity = forward * 20f; // Скорость снаряда

        var collider = projectile.AddComponent<SphereCollider>();
        collider.Radius = 0.15f;

        AddGameObject(projectile);
    }
}

public static class Program
{
    public static void Main()
    {
        var config = new EngineConfig
        {
            Width = 1920,
            Height = 1080,
            Title = "Engine Of Apocalypse",
            GraphicsAPI = GraphicsAPI.OpenGL,
            VSync = true,
            MSAA = 4
        };

        var engine = Engine.Instance;
        engine.Initialize(config);

        var scene = new MarioScene();
        engine.SceneManager!.AddScene("main", scene);
        engine.SceneManager.LoadScene("main");

        engine.Run();
    }
}