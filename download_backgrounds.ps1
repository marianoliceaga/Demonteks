# Background Asset Download Tracker

$basePath = "Assets\backgrounds"
$levels = @("jungle", "dungeon", "castle")

$expectedFiles = @{
    jungle = @(
        @{name = "sky_1920x1080.png"; desc = "Layer 1: Far Sky (0.2x scroll)"},
        @{name = "trees_3840x1080.png"; desc = "Layer 2: Distant Trees (0.5x scroll)"},
        @{name = "foliage_3840x1080.png"; desc = "Layer 3: Mid-ground Foliage (0.7x scroll)"},
        @{name = "foreground_5760x1080.png"; desc = "Layer 4: Foreground Details (1.2x scroll)"}
    )
    dungeon = @(
        @{name = "wall_1920x1080.png"; desc = "Layer 1: Back Wall (0.3x scroll)"},
        @{name = "pillars_3840x1080.png"; desc = "Layer 2: Pillars and Chains (0.6x scroll)"},
        @{name = "chains_3840x1080.png"; desc = "Layer 3: Foreground Chains (1.3x scroll)"}
    )
    castle = @(
        @{name = "walls_1920x1080.png"; desc = "Layer 1: Distant Walls (0.2x scroll)"},
        @{name = "interior_3840x1080.png"; desc = "Layer 2: Interior Hallway (0.5x scroll)"},
        @{name = "pillars_3840x1080.png"; desc = "Layer 3: Ornate Pillars (1.1x scroll)"}
    )
}

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  DEMONTEKS - BACKGROUND ASSET TRACKER  " -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

Write-Host "Creating directory structure..." -ForegroundColor Yellow
foreach ($level in $levels) {
    $levelPath = Join-Path $basePath $level
    if (-not (Test-Path $levelPath)) {
        New-Item -ItemType Directory -Path $levelPath -Force | Out-Null
        Write-Host "  Created: $levelPath" -ForegroundColor Green
    } else {
        Write-Host "  Exists:  $levelPath" -ForegroundColor Gray
    }
}
Write-Host ""

$totalFiles = 0
$completeFiles = 0

foreach ($level in $levels) {
    Write-Host "--- $($level.ToUpper()) LEVEL ---" -ForegroundColor Magenta
    $levelPath = Join-Path $basePath $level

    foreach ($file in $expectedFiles[$level]) {
        $totalFiles++
        $filePath = Join-Path $levelPath $file.name

        if (Test-Path $filePath) {
            Write-Host "[X] $($file.name)" -ForegroundColor Green
            Write-Host "    $($file.desc)" -ForegroundColor Gray
            $completeFiles++
        } else {
            Write-Host "[ ] $($file.name)" -ForegroundColor Red
            Write-Host "    $($file.desc)" -ForegroundColor Gray
        }
    }
    Write-Host ""
}

Write-Host "========================================" -ForegroundColor Cyan
$color = if ($completeFiles -eq $totalFiles) { "Green" } else { "Yellow" }
Write-Host "PROGRESS: $completeFiles / $totalFiles files complete" -ForegroundColor $color
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

if ($completeFiles -lt $totalFiles) {
    Write-Host "Next Steps:" -ForegroundColor Yellow
    Write-Host "1. Open AI_BACKGROUND_PROMPTS.md" -ForegroundColor White
    Write-Host "2. Copy prompts for missing files" -ForegroundColor White
    Write-Host "3. Generate using DALL-E 3, Midjourney, or other AI tools" -ForegroundColor White
    Write-Host "4. Download files to appropriate directories" -ForegroundColor White
    Write-Host "5. Run this script again to verify" -ForegroundColor White
    Write-Host ""
    Write-Host "Quick download paths:" -ForegroundColor Cyan
    foreach ($level in $levels) {
        $levelPath = Join-Path $basePath $level
        $fullPath = Join-Path $PWD $levelPath
        Write-Host "  $level : $fullPath" -ForegroundColor White
    }
} else {
    Write-Host "All backgrounds downloaded!" -ForegroundColor Green
    Write-Host "Ready to import into Godot." -ForegroundColor Green
}
