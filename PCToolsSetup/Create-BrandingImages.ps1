# Generate WiX Installer Branding Images
# This script creates Banner.bmp and Dialog.bmp from the 3rdRock logo

param(
    [string]$LogoPath = "$PSScriptRoot\..\SSDToolsWPF.UI\Resource\3rdRockLogo256.png",
    [string]$OutputFolder = $PSScriptRoot
)

Write-Host "`n=== Creating WiX Installer Branding Images ===" -ForegroundColor Cyan

# Load System.Drawing
Add-Type -AssemblyName System.Drawing

# Check if logo exists
if (-not (Test-Path $LogoPath)) {
    Write-Host "? Logo not found at: $LogoPath" -ForegroundColor Red
    exit 1
}

Write-Host "? Logo found: $LogoPath" -ForegroundColor Green

try {
    # Load the logo
    $logo = [System.Drawing.Image]::FromFile($LogoPath)
    Write-Host "  Logo size: $($logo.Width)x$($logo.Height)" -ForegroundColor Gray

    # === Create Banner (493 x 58) ===
    Write-Host "`nCreating Banner.bmp (493x58)..." -ForegroundColor Yellow
    
    $bannerWidth = 493
    $bannerHeight = 58
    $banner = New-Object System.Drawing.Bitmap($bannerWidth, $bannerHeight)
    $graphics = [System.Drawing.Graphics]::FromImage($banner)
    
    # Set high quality rendering
    $graphics.InterpolationMode = [System.Drawing.Drawing2D.InterpolationMode]::HighQualityBicubic
    $graphics.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::HighQuality
    $graphics.PixelOffsetMode = [System.Drawing.Drawing2D.PixelOffsetMode]::HighQuality
    
    # Fill background with white
    $graphics.Clear([System.Drawing.Color]::White)
    
    # Draw a subtle gradient background - MUCH LIGHTER for better text contrast
    $gradientBrush = New-Object System.Drawing.Drawing2D.LinearGradientBrush(
        [System.Drawing.Point]::new(0, 0),
        [System.Drawing.Point]::new($bannerWidth, 0),
        [System.Drawing.Color]::FromArgb(250, 250, 252),
        [System.Drawing.Color]::FromArgb(255, 255, 255)
    )
    $graphics.FillRectangle($gradientBrush, 0, 0, $bannerWidth, $bannerHeight)
    
    # Calculate logo dimensions (fit within 40px height for banner)
    $maxLogoHeight = 40
    $logoScale = [Math]::Min($maxLogoHeight / $logo.Height, 1.0)
    $scaledLogoWidth = [int]($logo.Width * $logoScale)
    $scaledLogoHeight = [int]($logo.Height * $logoScale)
    
    # Draw logo on the RIGHT side (changed from left)
    $logoX = $bannerWidth - $scaledLogoWidth - 10  # 10px margin from right edge
    $logoY = ($bannerHeight - $scaledLogoHeight) / 2
    $graphics.DrawImage($logo, $logoX, $logoY, $scaledLogoWidth, $scaledLogoHeight)
    
    # Remove product name text from banner - just logo on right
    # (Text is already shown in the dialog title)
    
    # Save banner
    $bannerPath = Join-Path $OutputFolder "Banner.bmp"
    $banner.Save($bannerPath, [System.Drawing.Imaging.ImageFormat]::Bmp)
    Write-Host "? Banner created: $bannerPath" -ForegroundColor Green
    
    # Cleanup banner resources
    $graphics.Dispose()
    $banner.Dispose()
    $gradientBrush.Dispose()

    # === Create Dialog (493 x 312) ===
    Write-Host "`nCreating Dialog.bmp (493x312)..." -ForegroundColor Yellow
    
    $dialogWidth = 493
    $dialogHeight = 312
    $dialog = New-Object System.Drawing.Bitmap($dialogWidth, $dialogHeight)
    $graphics2 = [System.Drawing.Graphics]::FromImage($dialog)
    
    # Set high quality rendering
    $graphics2.InterpolationMode = [System.Drawing.Drawing2D.InterpolationMode]::HighQualityBicubic
    $graphics2.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::HighQuality
    $graphics2.PixelOffsetMode = [System.Drawing.Drawing2D.PixelOffsetMode]::HighQuality
    
    # Create gradient background - MUCH LIGHTER (almost white) for better text contrast
    $gradientBrush2 = New-Object System.Drawing.Drawing2D.LinearGradientBrush(
        [System.Drawing.Point]::new(0, 0),
        [System.Drawing.Point]::new(0, $dialogHeight),
        [System.Drawing.Color]::FromArgb(248, 252, 255),
        [System.Drawing.Color]::FromArgb(255, 255, 255)
    )
    $graphics2.FillRectangle($gradientBrush2, 0, 0, $dialogWidth, $dialogHeight)
    
    # Calculate logo size (90px for left-aligned layout)
    $maxDialogLogoSize = 90
    $dialogLogoScale = [Math]::Min($maxDialogLogoSize / $logo.Width, $maxDialogLogoSize / $logo.Height)
    $dialogLogoWidth = [int]($logo.Width * $dialogLogoScale)
    $dialogLogoHeight = [int]($logo.Height * $dialogLogoScale)
    
    # Position logo on the left side, upper area
    $dialogLogoX = 30
    $dialogLogoY = 40
    $graphics2.DrawImage($logo, $dialogLogoX, $dialogLogoY, $dialogLogoWidth, $dialogLogoHeight)
    
    # Add product name to the right of or below the logo
    $dialogFont = New-Object System.Drawing.Font("Segoe UI", 20, [System.Drawing.FontStyle]::Bold)
    $dialogTextBrush = New-Object System.Drawing.SolidBrush([System.Drawing.Color]::FromArgb(0, 100, 180))
    $productText = "SSD One-Stop Tool"
    $textSize = $graphics2.MeasureString($productText, $dialogFont)
    
    # Position text below the logo on the left side
    $dialogTextX = $dialogLogoX
    $dialogTextY = $dialogLogoY + $dialogLogoHeight + 25
    $graphics2.DrawString($productText, $dialogFont, $dialogTextBrush, $dialogTextX, $dialogTextY)
    
    # Add tagline below product name
    $taglineFont = New-Object System.Drawing.Font("Segoe UI", 11, [System.Drawing.FontStyle]::Regular)
    $taglineText = "SSD Maintenance and Optimization"
    $taglineSize = $graphics2.MeasureString($taglineText, $taglineFont)
    $taglineX = $dialogLogoX
    $taglineY = $dialogTextY + 35
    $graphics2.DrawString($taglineText, $taglineFont, $dialogTextBrush, $taglineX, $taglineY)
    
    # Save dialog
    $dialogPath = Join-Path $OutputFolder "Dialog.bmp"
    $dialog.Save($dialogPath, [System.Drawing.Imaging.ImageFormat]::Bmp)
    Write-Host "? Dialog created: $dialogPath" -ForegroundColor Green
    
    # Cleanup dialog resources
    $graphics2.Dispose()
    $dialog.Dispose()
    $dialogFont.Dispose()
    $taglineFont.Dispose()
    $dialogTextBrush.Dispose()
    $gradientBrush2.Dispose()
    
    # Cleanup logo
    $logo.Dispose()
    
    Write-Host "`n=== Success! ===" -ForegroundColor Green
    Write-Host "Branding images created in: $OutputFolder" -ForegroundColor White
    Write-Host "  - Banner.bmp (493x58) - Logo: ${maxLogoHeight}px (right-aligned, no text)" -ForegroundColor Gray
    Write-Host "  - Dialog.bmp (493x312) - Logo: ${maxDialogLogoSize}px (left-aligned with text)" -ForegroundColor Gray
    Write-Host "`nNext step: Rebuild the installer to see the changes" -ForegroundColor Yellow
    Write-Host "Note: Light theme applied for better text contrast" -ForegroundColor Cyan
    
} catch {
    Write-Host "`n? Error creating images: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host $_.ScriptStackTrace -ForegroundColor Gray
    exit 1
}
