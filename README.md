# Engine Of Apocalypse

Engine Of Apocalypse is a cross-platform .NET 8 engine for building 2D games and cinematic pipelines (timeline, camera rigs, recording). It targets fast iteration, deterministic simulation options, and a clean separation between the runtime and future editor tooling.

## Technology choices
- Window/Input: **SDL2** via `Veldrid.StartupUtilities` — lightweight, proven across desktop platforms, and matches the renderer’s lifetime model.
- Rendering: **Veldrid** — modern, backend-agnostic (Vulkan/Metal/D3D11/OpenGL), minimal allocations, and a good fit for a job-friendly render prep pipeline.

## Repository layout
- `EngineOfApocalypse.sln` — solution file
- `src/` — engine modules (`EoA.Core`, `EoA.Platform`, `EoA.Graphics`, `EoA.Physics2D`, `EoA.Assets`, `EoA.Audio`, `EoA.UI`, `EoA.Cinematics`, `EoA.Diagnostics`, `EoA.Editor`, `EoA.SampleGame`)
- `tests/` — automated tests (`EoA.Tests`)
- `docs/` — documentation (`architecture.md`, `user-guide.md`, `roadmap.md`)
- `tools/` — CLI utilities and build scripts (placeholders for now)

## Status (Milestone 1: bootstrap)
- Solution and project layout created per module plan.
- Engine host spins up an SDL2 window, runs a basic loop, clears the screen via Veldrid, and exits cleanly.
- Documentation scaffolded for onboarding and architecture notes.

## Build & run
Prerequisites: .NET SDK 8.0+ and a GPU/driver capable of one of Veldrid’s backends (Vulkan/Metal/D3D11/OpenGL). Veldrid ships the needed SDL2/native bits; no separate install is required on supported platforms.

```bash
dotnet restore
dotnet build EngineOfApocalypse.sln
dotnet run --project src/EoA.SampleGame/EoA.SampleGame.csproj
```

See `docs/user-guide.md` for step-by-step usage and `docs/architecture.md` for module-level design notes.
