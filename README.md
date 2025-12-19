# Engine Of Apocalypse

**Engine Of Apocalypse** is a minimalist, atmosphere-driven game engine focused on building worlds at the edge of collapse.

Designed for developers who value control, mood, and systems over unnecessary complexity, the engine provides a clean foundation for dark sci-fi, post-apocalyptic, cyberpunk, and narrative-driven games.

---

## Philosophy

> The world is breaking.  
> The engine gives you control over how it ends — or survives.

Engine Of Apocalypse is built around the idea that games are not just visuals and mechanics, but **systems under pressure**.

The engine emphasizes:
- atmosphere over spectacle  
- systems over presets  
- control over abstraction  

---

## Core Concepts

The engine architecture is inspired by the **Four Forces of Apocalypse**, represented in the logo by four outward-facing horse skulls aligned to the cardinal directions.

Each force corresponds to a major system group:

- **WAR** — combat, physics, conflict simulation  
- **FAMINE** — resources, economy, survival mechanics  
- **DEATH** — damage, AI behavior, decay systems  
- **CONTROL** — scripting, logic, world rules  

These systems are independent, modular, and designed to interact organically.

---

## Features (Planned)

### Engine Core
- Scene-based architecture
- Entity–Component system
- Deterministic update loop
- Modular subsystems

### Atmosphere & World
- Dynamic lighting and shadows
- Fog, dust, ash, rain systems
- World decay over time
- Environmental storytelling tools

### Gameplay Systems
- Combat and damage framework
- Resource scarcity mechanics
- Faction and morality tracking
- Event-driven chaos system

### Developer Experience
- Minimal editor UI
- Scriptable logic layer
- Mod-friendly design
- Clean, documented API

---

## Target Projects

Engine Of Apocalypse is suited for:
- Post-apocalyptic games
- Cyberpunk and dystopian worlds
- Psychological thrillers
- Narrative-driven experiences
- Experimental indie and AA projects

---

## Design Goals

- Clean and minimal core
- High performance and low overhead
- Strong atmosphere support
- Full developer control
- No forced workflows

---

## Project Status

🚧 **Early Development / Concept Stage**

The engine is currently under active design and prototyping.
Expect breaking changes until the first public release.

---

## Roadmap

- [ ] Core engine architecture
- [ ] Basic rendering pipeline
- [ ] Entity–Component system
- [ ] Scripting layer
- [ ] Minimal editor
- [ ] Documentation and examples

---

## Logo & Identity

The logo represents the **Four Horsemen of Apocalypse** as four outward-facing horse skulls aligned to north, south, west, and east.

This symbolizes:
- total system coverage
- world pressure from all directions
- controlled collapse

---

## Contributing

Contributions are welcome.

If you believe in building engines that prioritize **systems, atmosphere, and control**, feel free to:
- open issues
- submit pull requests
- propose architecture ideas

Please keep contributions clean, minimal, and well-documented.

---

## License

License information will be added soon.

---

## Final Note

Engine Of Apocalypse is not about destruction for spectacle.

It is about **building worlds that break — and systems that decide how**.

---

# 🚀 ПОЛНОЕ РУКОВОДСТВО: ОТ УСТАНОВКИ ДО ИГРЫ (для этого репозитория)

## ЧАСТЬ 1: УСТАНОВКА И ЗАПУСК

## Требования

- **.NET SDK 8.0**

Проверка:

```bash
dotnet --version
```

## Запуск

Команды выполняются **в корне репозитория** (там где лежит `GameEngine.sln`).

```bash
dotnet restore
dotnet build
dotnet run --project GameEngine.Demo/GameEngine.Demo.csproj
```

## Важные примечания по структуре

- В этом репозитории `GameEngine.Demo` ссылается только на `GameEngine.Core`.
- `GameEngine.Core` **агрегирует исходники** из `GameEngine.Renderer`, `GameEngine.Physics`, `GameEngine.Assets`, `GameEngine.Input`, `GameEngine.UI`.

Из-за этого:

- **не нужно** добавлять `ProjectReference` на все проекты в `GameEngine.Demo`.
- если добавить лишние ссылки, появятся ошибки вида `type exists in both assemblies`.

## NuGet версии (важно)

Если ставишь пакеты вручную — версии должны быть согласованы. В этом репозитории используются:

- `Silk.NET.OpenGL` **2.22.0**
- `Silk.NET.Windowing` **2.22.0**
- `Silk.NET.Input` **2.21.0**
- `StbImageSharp` **2.30.15**
- `AssimpNet` **4.1.0**

Если поставить более старые версии (например `Silk.NET 2.21.0` или `StbImageSharp 2.27.14`), можно получить:

- `NU1605: Detected package downgrade`

---

## ЧАСТЬ 2: СОЗДАНИЕ СВОЕЙ ИГРЫ

## 🎮 Пример: своя сцена

1) Создай файл `GameEngine.Demo/MyGameScene.cs` и помести туда сцену (пример ниже).

2) В `GameEngine.Demo/Program.cs` создай `Main()` и загрузи сцену:

```csharp
var scene = new MyGameScene();
engine.SceneManager.AddScene("main", scene);
engine.SceneManager.LoadScene("main");
```

### Пример сцены

```csharp
using GameEngine.Core;
using System;
using System.Numerics;

public class MyGameScene : Scene
{
    private GameObject? player;

    public override void OnLoad()
    {
        Name = "My Game";

        var cameraObj = new GameObject("Camera");
        var camera = cameraObj.AddComponent<Camera>();
        camera.FOV = 75f;
        camera.AspectRatio = 16f / 9f;
        cameraObj.Transform.Position = new Vector3(0, 1.6f, 0);
        cameraObj.AddComponent<FirstPersonController>();
        MainCamera = camera;
        AddGameObject(cameraObj);

        player = cameraObj;

        CreateCube("Floor", new Vector3(0, -0.5f, 0), new Vector3(20, 1, 20), new Vector3(0.5f), isStatic: true);
        CreateCube("Wall", new Vector3(0, 2, -10), new Vector3(20, 4, 1), new Vector3(0.7f, 0.3f, 0.3f), isStatic: true);
    }

    private void CreateCube(string name, Vector3 position, Vector3 scale, Vector3 color, bool isStatic)
    {
        var obj = new GameObject(name);
        obj.Transform.Position = position;
        obj.Transform.Scale = scale;

        var renderer = obj.AddComponent<MeshRenderer>();
        renderer.Mesh = Mesh.CreateCube();
        renderer.Material = new Material { Albedo = color, Metallic = 0.1f, Roughness = 0.7f };

        var body = obj.AddComponent<RigidBody>();
        body.IsKinematic = isStatic;
        body.UseGravity = !isStatic;

        var collider = obj.AddComponent<BoxCollider>();
        collider.Size = Vector3.One;

        AddGameObject(obj);
    }

    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);

        if (Engine.Instance.Input?.IsKeyPressed(KeyCode.Escape) == true)
            Engine.Instance.Stop();
    }
}
```

---

## ЧАСТЬ 3: ASSETS

## Иконка окна

Чтобы иконка окна подхватилась автоматически:

- положи файл `Assets/englogo.png`

При запуске в консоли будет:

- `✓ Window icon loaded: ...`
