# Engine Of Apocalypse - User Guide

## Milestone 1: Getting Started

### Prerequisites
- .NET 8 SDK or later
- Windows 10+, macOS 10.15+, or Linux with Vulkan/OpenGL support

### Building and Running

#### Windows (PowerShell)
```powershell
# Setup repository
.\setup-repo.ps1
.\configure-projects.ps1

# Build
dotnet build

# Run sample game
dotnet run --project src/EoA.SampleGame/EoA.SampleGame.csproj
```

#### macOS/Linux (Bash)
```bash
# Make scripts executable
chmod +x setup-repo.sh configure-projects.sh

# Setup repository
./setup-repo.sh
./configure-projects.sh

# Build
dotnet build

# Run sample game
dotnet run --project src/EoA.SampleGame/EoA.SampleGame.csproj
```

### Expected Behavior
- A window opens with title "EoA Sample Game - Milestone 1"
- Window size: 1280x720
- Clear color: Dark blue-gray (#1A1A26)
- Console shows FPS and frame info every 2 seconds
- Close window to exit cleanly

### Configuration
Edit `EngineConfig` in `Program.cs` to customize:
- Window size, title, fullscreen
- VSync on/off
- Verbose logging

### Next Steps
See `docs/roadmap.md` for upcoming milestones.