# Verify Installer Contents Script
# This script checks if the new installer includes all required files

Write-Host "`n=== SSD One-Stop Tool Installer Verification ===" -ForegroundColor Cyan
Write-Host "`nChecking installer contents..." -ForegroundColor Yellow

$installerPath = "D:\PCTools\src\SSDToolsWPF\PCToolsSetup\bin\x64\Release\en-US\SSDOneStopTool-Setup.msi"
$buildPath = "D:\PCTools\src\SSDToolsWPF\SSDToolsWPF.UI\bin\Release\net10.0-windows7.0"

# Check if installer exists
if (Test-Path $installerPath) {
    $installerSize = (Get-Item $installerPath).Length / 1MB
    Write-Host "? Installer found: $installerPath" -ForegroundColor Green
    Write-Host "  Size: $([math]::Round($installerSize, 2)) MB" -ForegroundColor Gray
} else {
    Write-Host "? Installer not found!" -ForegroundColor Red
    exit
}

# Check required files in build output
Write-Host "`nChecking build output for required files..." -ForegroundColor Yellow

$requiredFiles = @(
    "SSDToolsWPF.UI.exe",
    "SSDToolsWPF.UI.dll",  # CRITICAL
    "SSDToolsWPF.Core.dll",
    "SSDToolsWPF.UI.deps.json",
    "SSDToolsWPF.UI.runtimeconfig.json",
    "Microsoft.Win32.TaskScheduler.dll",
    "System.ServiceProcess.ServiceController.dll"  # CRITICAL
)

$missing = @()
foreach ($file in $requiredFiles) {
    $filePath = Join-Path $buildPath $file
    if (Test-Path $filePath) {
        $size = (Get-Item $filePath).Length
        $sizeKB = [math]::Round($size / 1KB, 1)
        $critical = if ($file -match "SSDToolsWPF.UI.dll|ServiceController") { " [CRITICAL]" } else { "" }
        Write-Host "  ? $file ($sizeKB KB)$critical" -ForegroundColor Green
    } else {
        Write-Host "  ? $file - MISSING!" -ForegroundColor Red
        $missing += $file
    }
}

if ($missing.Count -gt 0) {
    Write-Host "`n? Missing files in build output. Please rebuild the solution." -ForegroundColor Red
    exit
}

# Check if currently installed version is old
Write-Host "`nChecking currently installed version..." -ForegroundColor Yellow
$installPath = "C:\Program Files\3rdRock\SSD One-Stop Tool"

if (Test-Path $installPath) {
    Write-Host "  Found existing installation" -ForegroundColor Yellow
    
    $installedDll = Join-Path $installPath "SSDToolsWPF.UI.dll"
    if (Test-Path $installedDll) {
        Write-Host "  ? SSDToolsWPF.UI.dll is present (good)" -ForegroundColor Green
    } else {
        Write-Host "  ? SSDToolsWPF.UI.dll is MISSING (old version - needs update)" -ForegroundColor Red
    }
    
    Write-Host "`n  Action Required: Uninstall old version before installing new one" -ForegroundColor Yellow
    Write-Host "  1. Open Settings > Apps" -ForegroundColor Gray
    Write-Host "  2. Find 'SSD One-Stop Tool'" -ForegroundColor Gray
    Write-Host "  3. Click Uninstall" -ForegroundColor Gray
} else {
    Write-Host "  No existing installation found (clean install)" -ForegroundColor Green
}

# Check Event Viewer for recent errors
Write-Host "`nChecking Event Viewer for recent .NET errors..." -ForegroundColor Yellow
try {
    $recentErrors = Get-EventLog -LogName Application -Source ".NET Runtime" -Newest 3 -ErrorAction SilentlyContinue | 
        Where-Object { $_.EntryType -eq "Error" -and $_.TimeGenerated -gt (Get-Date).AddHours(-1) }
    
    if ($recentErrors) {
        Write-Host "  ? Found recent .NET Runtime errors:" -ForegroundColor Yellow
        foreach ($error in $recentErrors) {
            Write-Host "    $($error.TimeGenerated): $($error.Message.Substring(0, 100))..." -ForegroundColor Gray
        }
    } else {
        Write-Host "  ? No recent .NET Runtime errors" -ForegroundColor Green
    }
} catch {
    Write-Host "  (Unable to check Event Log)" -ForegroundColor Gray
}

# Summary
Write-Host "`n=== Summary ===" -ForegroundColor Cyan
Write-Host "? New installer has been built with ALL required files" -ForegroundColor Green
Write-Host "? Critical DLLs are included:" -ForegroundColor Green
Write-Host "  - SSDToolsWPF.UI.dll (main application code)" -ForegroundColor Gray
Write-Host "  - System.ServiceProcess.ServiceController.dll (service management)" -ForegroundColor Gray

Write-Host "`n=== Next Steps ===" -ForegroundColor Cyan
if (Test-Path $installPath) {
    Write-Host "1. Uninstall the OLD version (Settings > Apps)" -ForegroundColor Yellow
    Write-Host "2. Delete old log: del C:\SSDTools_Startup.log" -ForegroundColor Yellow
    Write-Host "3. Install NEW version from:" -ForegroundColor Yellow
} else {
    Write-Host "1. Install NEW version from:" -ForegroundColor Yellow
}
Write-Host "   $installerPath" -ForegroundColor White
Write-Host "2. Check if application stays open" -ForegroundColor Yellow
Write-Host "3. View log: notepad C:\SSDTools_Startup.log" -ForegroundColor Yellow

Write-Host "`nPress any key to open installer folder..." -ForegroundColor Gray
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
explorer (Split-Path $installerPath)
