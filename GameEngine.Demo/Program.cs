using GameEngine.Core;
using System.Numerics;

class Program
{
    static void Main()
    {
        // Конфигурация движка
        var config = new EngineConfig
        {
            Width = 1920,
            Height = 1080,
            Title = "My Game Engine",
            GraphicsAPI = GraphicsAPI.OpenGL,
            VSync = true,
            MSAA = 4
        };

        // Инициализация
        var engine = Engine.Instance;
        engine.Initialize(config);

        // Создание сцены
        var scene = new GameScene();
        engine.SceneManager.AddScene("main", scene);
        engine.SceneManager.LoadScene("main");

        // Запуск
        engine.Run();
    }
}

// Пользовательская сцена
class GameScene : Scene
{
    private GameObject player;
    private GameObject light;
    private float _rotation;

    public override void OnLoad()
    {
        Name = "Main Scene";

        // Камера
        var cameraObj = new GameObject("Camera");
        var camera = cameraObj.AddComponent<Camera>();
        camera.FOV = 60f;
        camera.AspectRatio = 16f / 9f;
        cameraObj.AddComponent<FirstPersonController>();
        cameraObj.Transform.Position = new Vector3(0, 2, -5);
        MainCamera = camera;
        AddGameObject(cameraObj);

        // Игрок
        player = new GameObject("Player");
        player.Transform.Position = new Vector3(0, 1, 0);
        
        var meshRenderer = player.AddComponent<MeshRenderer>();
        meshRenderer.Mesh = Mesh.CreateCube();
        
        var material = new Material
        {
            Shader = Engine.Instance.Assets.LoadShader(
                "shaders/standard.vert",
                "shaders/standard.frag"),
            Albedo = new Vector3(0.8f, 0.2f, 0.2f)
        };
        meshRenderer.Material = material;
        
        var rigidBody = player.AddComponent<RigidBody>();
        rigidBody.Mass = 10f;
        rigidBody.UseGravity = true;
        
        var collider = player.AddComponent<BoxCollider>();
        collider.Size = Vector3.One;
        
        AddGameObject(player);

        // Пол
        var floor = new GameObject("Floor");
        floor.Transform.Position = new Vector3(0, -0.5f, 0);
        floor.Transform.Scale = new Vector3(20, 0.1f, 20);
        
        var floorRenderer = floor.AddComponent<MeshRenderer>();
        floorRenderer.Mesh = Mesh.CreateCube();
        floorRenderer.Material = new Material
        {
            Shader = material.Shader,
            Albedo = new Vector3(0.3f, 0.3f, 0.3f)
        };
        
        var floorBody = floor.AddComponent<RigidBody>();
        floorBody.IsKinematic = true;
        
        var floorCollider = floor.AddComponent<BoxCollider>();
        floorCollider.Size = Vector3.One;
        
        AddGameObject(floor);

        // Свет
        light = new GameObject("Light");
        light.Transform.Position = new Vector3(0, 5, 0);
        var lightComponent = light.AddComponent<Light>();
        lightComponent.Type = LightType.Point;
        lightComponent.Color = new Vector3(1, 1, 1);
        lightComponent.Intensity = 10f;
        AddGameObject(light);

        // UI
        CreateUI();
    }

    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);

        if (player != null)
        {
            _rotation += deltaTime;
            player.Transform.Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitY, _rotation);
        }
    }

    private void CreateUI()
    {
        var canvas = Engine.Instance.UI.GetRootCanvas();

        // FPS Counter
        var fpsLabel = new TextLabel("FPS: 60", new Vector2(10, 10));
        fpsLabel.FontSize = 20f;
        canvas.AddChild(fpsLabel);

        // Pause button
        var pauseButton = new Button("Pause", new Vector2(10, 50), new Vector2(100, 40));
        pauseButton.OnClickEvent += () => {
            Console.WriteLine("Paused!");
        };
        canvas.AddChild(pauseButton);
    }
}