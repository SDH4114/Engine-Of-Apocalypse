# Engine Of Apocalypse – Руководство пользователя

Это руководство проведет нового разработчика от клонирования репозитория до создания исполняемого файла. Мы используем подход CLI-first (командная строка) и поддерживаем кроссплатформенность (Windows/macOS/Linux) на базе .NET 10.

## Предварительные требования
- .NET SDK 10.0 (`dotnet --version`)
- Драйвер видеокарты с поддержкой Vulkan, Metal, Direct3D 11 или OpenGL.
- Клиент Git.
- **Для macOS**: Установленный Homebrew для загрузки нативных библиотек.

## Клонирование, настройка и запуск

1. **Клонируйте репозиторий:**
   ```bash
   git clone <url-вашего-форка-или-оригинала> Engine-Of-Apocalypse
   cd Engine-Of-Apocalypse
   ```

2. **Восстановите зависимости:**
   ```bash
   dotnet restore
   ```

3. **Соберите решение:**
   ```bash
   dotnet build
   ```

4. **Запустите пример игры (SampleGame):**

   **Windows / Linux:**
   ```bash
   dotnet run --project src/EoA.SampleGame/EoA.SampleGame.csproj
   ```

   **macOS:**
   Для корректной работы библиотеки SDL2 на macOS необходимо указать путь к ней. Используйте подготовленный скрипт:
   ```bash
   chmod +x run_game.sh  # (только первый раз)
   ./run_game.sh
   ```
   *Если скрипта нет, вы можете запустить вручную:*
   ```bash
   export DYLD_LIBRARY_PATH=/opt/homebrew/lib:$DYLD_LIBRARY_PATH
   dotnet run --project src/EoA.SampleGame/EoA.SampleGame.csproj
   ```

   При успешном запуске откроется окно с заливкой цветом `CornflowerBlue`.

## Структура проекта
- `EoA.Core` — Ядро: конфигурация, интерфейсы (`IGame`, `IGraphicsDeviceContext`), структуры данных (`Color`, `FrameTime`). Зависит только от стандартной библиотеки.
- `EoA.Platform` — Платформа: создание окна, инициализация SDL2/Veldrid, игровой цикл (`EngineHost`).
- `EoA.Graphics` — Графика: реализация `IGraphicsDeviceContext` на базе Veldrid.
- `EoA.SampleGame` — Пример использования движка.

## Создание собственного игрового проекта

1. **Создайте консольное приложение:**
   ```bash
   dotnet new console -n MyGame -o src/MyGame
   ```

2. **Добавьте ссылки на проекты движка:**
   ```bash
   dotnet add src/MyGame/MyGame.csproj reference src/EoA.Core/EoA.Core.csproj src/EoA.Platform/EoA.Platform.csproj
   ```

3. **Напишите код игры (Program.cs):**
   ```csharp
   using EoA.Core;
   using EoA.Platform;

   // 1. Конфигурация
   var config = new EngineConfig 
   { 
       WindowTitle = "Моя Игра", 
       WindowWidth = 1280, 
       WindowHeight = 720,
       VSync = true 
   };

   // 2. Создание экземпляра игры
   var game = new MyGameImplementation();

   // 3. Запуск хоста
   using var host = new EngineHost(config, game);
   host.Run();

   // Реализация интерфейса IGame
   internal sealed class MyGameImplementation : IGame
   {
       private EngineContext _ctx;

       public void Initialize(EngineContext context) 
       {
           _ctx = context;
           // Здесь загружайте ресурсы
       }

       public void Update(FrameTime frameTime) 
       {
           // Логика игры (вызывается каждый кадр)
           // frameTime.DeltaSeconds - время с прошлого кадра
       }

       public void Render(FrameTime frameTime)
       {
           // Рендеринг
           _ctx.Graphics.Clear(Color.Black); // Очистка экрана
           // ... отрисовка ...
           _ctx.Graphics.Present(); // Отображение кадра
       }

       public void Shutdown() 
       {
           // Освобождение ресурсов
       }
   }
   ```

4. **Добавьте проект в решение (опционально):**
   ```bash
   dotnet sln add src/MyGame/MyGame.csproj
   ```

## Публикация (Сборка исполняемого файла)

Для создания готовой к распространению версии:

```bash
# Пример для macOS (Apple Silicon)
dotnet publish src/EoA.SampleGame/EoA.SampleGame.csproj -c Release -r osx-arm64 --self-contained true

# Пример для Windows (x64)
dotnet publish src/EoA.SampleGame/EoA.SampleGame.csproj -c Release -r win-x64 --self-contained true
```
Артефакты будут находиться в `src/EoA.SampleGame/bin/Release/net10.0/<runtime>/publish/`.

## Решение проблем

*   **Ошибка `DllNotFoundException` или `Unable to load shared library 'libSDL2'`**:
    *   Убедитесь, что SDL2 установлен (`brew install sdl2` на macOS).
    *   Используйте `run_game.sh` на macOS.
*   **Ошибка `Metal validation error` или крэш при старте**:
    *   Убедитесь, что используется актуальная версия кода, где настроен корректный формат буфера глубины (`PixelFormat.D32_Float_S8_UInt`).
*   **Предупреждения безопасности (NuGet)**:
    *   Мы обновили `Newtonsoft.Json` до версии 13.0.3. Если ошибка повторяется, выполните `dotnet restore`.
