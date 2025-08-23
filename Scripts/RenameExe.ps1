param(
    [string]$OutputPath,
    [string]$AssemblyName,
    [string]$ProjectDir,
    [string]$Configuration = "Release"
)

Write-Host "Raw parameters received:" -ForegroundColor Yellow
Write-Host "  OutputPath: '$OutputPath'" -ForegroundColor Gray
Write-Host "  AssemblyName: '$AssemblyName'" -ForegroundColor Gray
Write-Host "  ProjectDir: '$ProjectDir'" -ForegroundColor Gray
Write-Host "  Configuration: '$Configuration'" -ForegroundColor Gray

# Skip renaming for Debug builds
if ($Configuration -eq "Debug") {
    Write-Host "⚠ Skipping executable renaming for Debug configuration" -ForegroundColor Yellow
    exit 0
}
# Clean up parameters - remove quotes and fix paths
$OutputPath = $OutputPath.Trim('"').TrimEnd('\')
$AssemblyName = $AssemblyName.Trim('"')
$ProjectDir = $ProjectDir.Trim('"').TrimEnd('\')

# Handle empty ProjectDir by using current location
if ([string]::IsNullOrWhiteSpace($ProjectDir)) {
    $ProjectDir = Get-Location
    Write-Host "ProjectDir was empty, using current location: $ProjectDir" -ForegroundColor Yellow
}

Write-Host "Cleaned parameters:" -ForegroundColor Green
Write-Host "  OutputPath: '$OutputPath'" -ForegroundColor Gray
Write-Host "  AssemblyName: '$AssemblyName'" -ForegroundColor Gray
Write-Host "  ProjectDir: '$ProjectDir'" -ForegroundColor Gray

# Read AppConfig.cs from the correct path
$appConfigPath = Join-Path $ProjectDir "Utils\Lists\AppConfig.cs"
Write-Host "Looking for AppConfig at: $appConfigPath" -ForegroundColor Gray

if (-not (Test-Path $appConfigPath)) {
    Write-Host "AppConfig.cs not found at: $appConfigPath" -ForegroundColor Red
    Write-Host "Trying alternative paths..." -ForegroundColor Yellow

    # Try some alternative paths
    $alternatives = @(
        (Join-Path $ProjectDir "AppConfig.cs"),
        (Join-Path $ProjectDir "Utils\AppConfig.cs"),
        (Join-Path $ProjectDir "Lists\AppConfig.cs")
    )

    foreach ($alt in $alternatives) {
        Write-Host "  Checking: $alt" -ForegroundColor Gray
        if (Test-Path $alt) {
            $appConfigPath = $alt
            Write-Host "  Found AppConfig at: $appConfigPath" -ForegroundColor Green
            break
        }
    }

    if (-not (Test-Path $appConfigPath)) {
        Write-Host "AppConfig.cs not found in any expected location" -ForegroundColor Red
        exit 0
    }
}

$appConfigContent = Get-Content $appConfigPath -Raw
Write-Host "AppConfig.cs content length: $($appConfigContent.Length) characters" -ForegroundColor Gray

# Extract values using robust regex patterns
$nameMatch = [regex]::Match($appConfigContent, 'public\s+static\s+string\s+Name\s*=\s*"([^"]+)"')
$preReleaseMatch = [regex]::Match($appConfigContent, 'public\s+static\s+bool\s+preRelease\s*=\s*(true|false)')
$versionMatch = [regex]::Match($appConfigContent, 'public\s+static\s+string\s+Version\s*=\s*"([^"]+)"')
$serverModeMatch = [regex]::Match($appConfigContent, 'public\s+static\s+int\s+ServerMode\s*=\s*(\d+)')

Write-Host "Regex results:" -ForegroundColor Gray
Write-Host "  Name match: $($nameMatch.Success) - '$($nameMatch.Groups[1].Value)'" -ForegroundColor Gray
Write-Host "  PreRelease match: $($preReleaseMatch.Success) - '$($preReleaseMatch.Groups[1].Value)'" -ForegroundColor Gray
Write-Host "  Version match: $($versionMatch.Success) - '$($versionMatch.Groups[1].Value)'" -ForegroundColor Gray
Write-Host "  ServerMode match: $($serverModeMatch.Success) - '$($serverModeMatch.Groups[1].Value)'" -ForegroundColor Gray

if ($nameMatch.Success -and $preReleaseMatch.Success -and $serverModeMatch.Success) {
    $appName = $nameMatch.Groups[1].Value
    $isPreRelease = $preReleaseMatch.Groups[1].Value -eq "true"
    $version = if ($versionMatch.Success) { $versionMatch.Groups[1].Value } else { "Unknown" }
    $serverMode = [int]$serverModeMatch.Groups[1].Value

    # Determine rate tag based on ServerMode
    $rateTag = switch ($serverMode) {
        0 { "MR" }  # Mid-rate
        1 { "HR" }  # High-rate
        default { "Unknown" }
    }

    # Clean the app name for filename (remove spaces and special characters)
    $cleanAppName = $appName -replace '\s+', '' -replace '[^\w\-_.]', ''

    # Get current date in same format as your code
    $dateStamp = Get-Date -Format "yyyyMMdd"

    # Determine new filename based on preRelease flag
    if ($isPreRelease) {
        $newFileName = "$cleanAppName-$dateStamp-$rateTag.exe"
    } else {
        $newFileName = "$cleanAppName-$rateTag.exe"
    }

    # File paths - use absolute paths to avoid confusion
    $originalPath = Join-Path $ProjectDir (Join-Path $OutputPath "$AssemblyName.exe")
    $newPath = Join-Path $ProjectDir (Join-Path $OutputPath $newFileName)

    Write-Host "File paths:" -ForegroundColor Gray
    Write-Host "  Original: '$originalPath'" -ForegroundColor Gray
    Write-Host "  New: '$newPath'" -ForegroundColor Gray

    # Rename the file if original exists
    if (Test-Path $originalPath) {
        # Only rename if the names are different
        if ($originalPath -ne $newPath) {
            Move-Item $originalPath $newPath -Force
            Write-Host "✓ Renamed executable: $AssemblyName.exe → $newFileName" -ForegroundColor Green
            Write-Host "  App: $appName $version | PreRelease: $isPreRelease | Mode: $rateTag" -ForegroundColor Cyan
        } else {
            Write-Host "✓ Executable name unchanged: $newFileName" -ForegroundColor Gray
        }
    } else {
        Write-Host "✗ Original executable not found: $originalPath" -ForegroundColor Red
    }
} else {
    Write-Host "✗ Could not parse required values from AppConfig.cs" -ForegroundColor Red
    if (-not $nameMatch.Success) { Write-Host "  - Name field not found" -ForegroundColor Yellow }
    if (-not $preReleaseMatch.Success) { Write-Host "  - preRelease field not found" -ForegroundColor Yellow }
    if (-not $serverModeMatch.Success) { Write-Host "  - ServerMode field not found" -ForegroundColor Yellow }
}