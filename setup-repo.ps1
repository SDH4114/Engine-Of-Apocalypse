# setup-repo.ps1
# Creates the complete repo structure for Engine Of Apocalypse

# Create solution
dotnet new sln -n EngineOfApocalypse --force

# Create directory structure
$dirs = @(
    "src/EoA.Core",
    "src/EoA.Platform",
    "src/EoA.Graphics",
    "src/EoA.Physics2D",
    "src/EoA.Assets",
    "src/EoA.Audio",
    "src/EoA.UI",
    "src/EoA.Cinematics",
    "src/EoA.Diagnostics",
    "src/EoA.Editor",
    "src/EoA.SampleGame",
    "tests/EoA.Tests",
    "docs",
    "tools/asset-importer-cli",
    "tools/build-scripts"
)

foreach ($dir in $dirs) {
    New-Item -ItemType Directory -Force -Path $dir | Out-Null
}

# Create Core library
dotnet new classlib -n EoA.Core -o src/EoA.Core -f net8.0
dotnet sln add src/EoA.Core/EoA.Core.csproj

# Create Platform library
dotnet new classlib -n EoA.Platform -o src/EoA.Platform -f net8.0
dotnet sln add src/EoA.Platform/EoA.Platform.csproj

# Create Graphics library
dotnet new classlib -n EoA.Graphics -o src/EoA.Graphics -f net8.0
dotnet sln add src/EoA.Graphics/EoA.Graphics.csproj

# Create SampleGame executable
dotnet new console -n EoA.SampleGame -o src/EoA.SampleGame -f net8.0
dotnet sln add src/EoA.SampleGame/EoA.SampleGame.csproj

# Create test project
dotnet new xunit -n EoA.Tests -o tests/EoA.Tests -f net8.0
dotnet sln add tests/EoA.Tests/EoA.Tests.csproj

Write-Host "Repository structure created successfully!"
Write-Host "Next: Run './configure-projects.ps1' to add dependencies and configuration"
