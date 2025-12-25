#!/bin/bash
# setup-repo.sh

set -e

dotnet new sln -n EngineOfApocalypse --force

# Create directory structure
mkdir -p src/{EoA.Core,EoA.Platform,EoA.Graphics,EoA.Physics2D,EoA.Assets,EoA.Audio,EoA.UI,EoA.Cinematics,EoA.Diagnostics,EoA.Editor,EoA.SampleGame}
mkdir -p tests/EoA.Tests
mkdir -p docs
mkdir -p tools/{asset-importer-cli,build-scripts}

# Create projects
dotnet new classlib -n EoA.Core -o src/EoA.Core -f net8.0
dotnet sln add src/EoA.Core/EoA.Core.csproj

dotnet new classlib -n EoA.Platform -o src/EoA.Platform -f net8.0
dotnet sln add src/EoA.Platform/EoA.Platform.csproj

dotnet new classlib -n EoA.Graphics -o src/EoA.Graphics -f net8.0
dotnet sln add src/EoA.Graphics/EoA.Graphics.csproj

dotnet new console -n EoA.SampleGame -o src/EoA.SampleGame -f net8.0
dotnet sln add src/EoA.SampleGame/EoA.SampleGame.csproj

dotnet new xunit -n EoA.Tests -o tests/EoA.Tests -f net8.0
dotnet sln add tests/EoA.Tests/EoA.Tests.csproj

echo "Repository structure created successfully!"
echo "Next: Run './configure-projects.sh' to add dependencies and configuration"