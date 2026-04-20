# Read version from Plugin.cs
$pluginContent = Get-Content "$PSScriptRoot\Plugin.cs" -Raw
$versionMatch = [regex]::Match($pluginContent, 'PluginVersion = "([^"]+)"')

if (-not $versionMatch.Success) {
    Write-Error "Could not find PluginVersion in Plugin.cs"
    exit 1
}

$version = $versionMatch.Groups[1].Value
Write-Host "Detected version: $version"

# Read manifest template
$manifestTemplate = Get-Content "$PSScriptRoot\manifest.json" -Raw

# Replace version placeholder
$manifest = $manifestTemplate -replace '__VERSION__', $version

# Determine output paths based on configuration
$debugOutput = "$PSScriptRoot\..\..\..\debug_builds\NowYouFloat\manifest.json"
$releaseOutput = "$PSScriptRoot\..\..\..\release\NowYouFloat\manifest.json"

# Write to both output directories
$paths = @($debugOutput, $releaseOutput)

foreach ($outputPath in $paths) {
    $outputDir = Split-Path $outputPath -Parent
    if (-not (Test-Path $outputDir)) {
        New-Item -ItemType Directory -Force -Path $outputDir | Out-Null
    }

    Set-Content -Path $outputPath -Value $manifest -NoNewline
    Write-Host "Generated manifest at: $outputPath"
}

Write-Host "Manifest generation complete with version $version" -ForegroundColor Green
