$ErrorActionPreference = 'Stop'

$hookProject = Join-Path $PSScriptRoot 'hooks\HookRunner\HookRunner.csproj'
$treasureServer = Join-Path $PSScriptRoot 'treasure-finder\server.cs'

if (-not (Test-Path $hookProject)) {
    throw "Hook project not found: $hookProject"
}

if (-not (Test-Path $treasureServer)) {
    throw "File-based app not found: $treasureServer"
}

Write-Host "Building $hookProject"
dotnet build $hookProject --tl:off --verbosity quiet

Write-Host "Warming $treasureServer"
@() | dotnet run --file $treasureServer --tl:off --verbosity quiet -- --warmup