# Engine Of Apocalypse - Architecture

## Overview
EoA is a modular C# game engine for 2D games and cinematic workflows.

## Design Principles
1. **Modular**: Core + independent modules (graphics, physics, audio, etc.)
2. **Cross-platform**: Windows, macOS, Linux via .NET 8
3. **Performance-focused**: Minimal allocations, batching, multi-threading
4. **Data-oriented**: Cache-friendly data layouts where beneficial

## Module Structure

### Core (EoA.Core)
- Engine lifecycle management
- Configuration system
- Time management
- Structured logging
- Base interfaces (IEngineModule)

### Platform (EoA.Platform)
- Window management (Silk.NET)
- Input handling
- Cross-platform abstractions

### Graphics (EoA.Graphics)
- Rendering backend (Veldrid)
- Command buffer management
- Material system (future)
- 2D sprite batching (future)
- Lighting pipeline (future)

### Future Modules
- Physics2D: Collision detection, rigid bodies, constraints
- Assets: Import pipeline, asset database, hot reload
- Audio: Sound playback, mixing, spatial audio
- UI: Editor UI (ImGui.NET) + in-game UI system
- Cinematics: Camera rigs, timeline, keyframe animation
- Diagnostics: Profiler, debug overlay, frame markers

## Threading Model
- **Main thread**: Window events, command submission
- **Worker threads**: Physics, asset loading, render prep (future)

## Current Status: Milestone 1
✅ Repo structure
✅ Core types (Time, Logger, Config)
✅ Platform module (window + events)
✅ Graphics module (device + clear screen)
✅ Sample game (minimal runnable app)