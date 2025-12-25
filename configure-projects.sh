#!/bin/bash
set -e

dotnet add src/EoA.Platform/EoA.Platform.csproj package Silk.NET.Windowing
dotnet add src/EoA.Platform/EoA.Platform.csproj package Silk.NET.Input

dotnet add src/EoA.Graphics/EoA.Graphics.csproj package Veldrid
dotnet add src/EoA.Graphics/EoA.Graphics.csproj package Veldrid.StartupUtilities

dotnet add src/EoA.Platform/EoA.Platform.csproj reference src/EoA.Core/EoA.Core.csproj
dotnet add src/EoA.Graphics/EoA.Graphics.csproj reference src/EoA.Core/EoA.Core.csproj
dotnet add src/EoA.Graphics/EoA.Graphics.csproj reference src/EoA.Platform/EoA.Platform.csproj
dotnet add src/EoA.SampleGame/EoA.SampleGame.csproj reference src/EoA.Core/EoA.Core.csproj
dotnet add src/EoA.SampleGame/EoA.SampleGame.csproj reference src/EoA.Platform/EoA.Platform.csproj
dotnet add src/EoA.SampleGame/EoA.SampleGame.csproj reference src/EoA.Graphics/EoA.Graphics.csproj

echo "Project configuration complete!"