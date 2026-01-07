# Creating WiX Installer Branding Images

Since we need specific sizes for WiX, I'll create instructions for generating the banner and dialog images.

## Required Image Sizes

### 1. Banner Image (WixUIBannerBmp)
- **Size**: 493 × 58 pixels
- **Format**: BMP or PNG (BMP preferred)
- **Location**: Top of installer dialogs
- **Content**: Company logo + product name

### 2. Dialog Image (WixUIDialogBmp)  
- **Size**: 493 × 312 pixels
- **Format**: BMP or PNG (BMP preferred)
- **Location**: Left side of main dialogs
- **Content**: Product branding, logo, visual

## Option 1: Quick PNG to BMP Conversion (PowerShell)

```powershell
# Install required module
# Add-Type -AssemblyName System.Drawing

# Load the logo
$logoPath = "D:\PCTools\src\SSDToolsWPF\SSDToolsWPF.UI\Resource\3rdRockLogo256.png"
$outputFolder = "D:\PCTools\src\SSDToolsWPF\PCToolsSetup"

# Create banner (493x58)
$logo = [System.Drawing.Image]::FromFile($logoPath)
$banner = New-Object System.Drawing.Bitmap(493, 58)
$graphics = [System.Drawing.Graphics]::FromImage($banner)
$graphics.Clear([System.Drawing.Color]::White)

# Draw logo on left side (scaled to fit)
$logoHeight = 50
$logoWidth = [int]($logo.Width * ($logoHeight / $logo.Height))
$graphics.DrawImage($logo, 5, 4, $logoWidth, $logoHeight)

# Add text
$font = New-Object System.Drawing.Font("Segoe UI", 16, [System.Drawing.FontStyle]::Bold)
$brush = New-Object System.Drawing.SolidBrush([System.Drawing.Color]::FromArgb(0, 0, 0))
$graphics.DrawString("SSD One-Stop Tool", $font, $brush, ($logoWidth + 15), 18)

$banner.Save("$outputFolder\Banner.bmp", [System.Drawing.Imaging.ImageFormat]::Bmp)

# Create dialog (493x312)
$dialog = New-Object System.Drawing.Bitmap(493, 312)
$graphics2 = [System.Drawing.Graphics]::FromImage($dialog)
$graphics2.Clear([System.Drawing.Color]::FromArgb(240, 240, 240))

# Draw logo centered
$logoHeight2 = 256
$logoWidth2 = [int]($logo.Width * ($logoHeight2 / $logo.Height))
$x = (493 - $logoWidth2) / 2
$y = 30
$graphics2.DrawImage($logo, $x, $y, $logoWidth2, $logoHeight2)

$dialog.Save("$outputFolder\Dialog.bmp", [System.Drawing.Imaging.ImageFormat]::Bmp)

Write-Host "Branding images created successfully!" -ForegroundColor Green
```

## Option 2: Manual Creation (Recommended)

Use an image editor (Paint.NET, GIMP, Photoshop) to create:

### Banner.bmp (493 × 58)
1. Create new image: 493 × 58 pixels
2. Fill with white or brand color
3. Add 3rdRock logo on the left (scaled to ~50px height)
4. Add "SSD One-Stop Tool" text on the right
5. Save as Banner.bmp in PCToolsSetup folder

### Dialog.bmp (493 × 312)
1. Create new image: 493 × 312 pixels
2. Fill with light gray or gradient
3. Center the 3rdRock logo
4. Optionally add product name at top/bottom
5. Save as Dialog.bmp in PCToolsSetup folder

## Option 3: Simple Solid Color Banners

If you just want to use the logo without creating custom images:

```powershell
# This creates simple solid color backgrounds with the logo
# Banner (493x58)
$banner = New-Object System.Drawing.Bitmap(493, 58)
$graphics = [System.Drawing.Graphics]::FromImage($banner)
$graphics.Clear([System.Drawing.Color]::FromArgb(0, 120, 215))  # Windows blue
$banner.Save("$outputFolder\Banner.bmp")

# Dialog (493x312)  
$dialog = New-Object System.Drawing.Bitmap(493, 312)
$graphics2 = [System.Drawing.Graphics]::FromImage($dialog)
$graphics2.Clear([System.Drawing.Color]::White)
$dialog.Save("$outputFolder\Dialog.bmp")
```

## Testing

After creating the images, update Package.wxs to reference them, then rebuild and test the installer.
