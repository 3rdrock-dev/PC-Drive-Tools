# Custom Installer Branding - Complete

## What Was Added

Your SSD One-Stop Tool installer now features **custom branding** using the 3rdRock logo!

## Branding Images Created

### 1. Banner.bmp (493 × 58 pixels)
- **Location**: Top of all installer dialog screens
- **Content**: 
  - 3rdRock logo on the left
  - "SSD One-Stop Tool" text in blue
  - Subtle gradient background
- **File**: `PCToolsSetup\Banner.bmp`

### 2. Dialog.bmp (493 × 312 pixels)
- **Location**: Left side of main installer dialogs
- **Content**:
  - Centered 3rdRock logo
  - "SSD One-Stop Tool" title
  - "SSD Maintenance and Optimization" tagline
  - Gradient background (light blue to white)
- **File**: `PCToolsSetup\Dialog.bmp`

## Where You'll See The Branding

### Screens with Banner (Top)
? Welcome screen  
? License Agreement  
? Setup Type selection  
? Custom Setup (feature selection)  
? Installation Folder  
? Ready to Install  
? Installing (progress)  
? Installation Complete  

### Screens with Dialog Image (Left Side)
? Welcome screen  
? License Agreement  
? Setup Type selection  
? Installation Complete  

## Technical Details

### Image Specifications
```
Banner:
- Size: 493 × 58 pixels
- Format: BMP (24-bit)
- Background: White/light gradient
- Logo: Scaled to 48px height
- Text: Segoe UI Bold 18pt

Dialog:
- Size: 493 × 312 pixels
- Format: BMP (24-bit)
- Background: Light blue gradient
- Logo: Centered, 200px max
- Text: Segoe UI Bold 16pt + 10pt
```

### Configuration in Package.wxs
```xml
<WixVariable Id="WixUIBannerBmp" Value="Banner.bmp" />
<WixVariable Id="WixUIDialogBmp" Value="Dialog.bmp" />
```

### Project Configuration
Added to `PCToolsSetup.wixproj`:
```xml
<Content Include="Banner.bmp" />
<Content Include="Dialog.bmp" />
```

## How to Test

1. **Uninstall old version** (if installed)
2. **Run the new installer**:
   ```
   D:\PCTools\src\SSDToolsWPF\PCToolsSetup\bin\x64\Release\en-US\SSDOneStopTool-Setup.msi
   ```
3. **Observe the branding** on each screen as you step through the wizard

## Customizing the Images

If you want to modify the branding images:

### Option 1: Regenerate with Script
```powershell
cd D:\PCTools\src\SSDToolsWPF\PCToolsSetup
.\Create-BrandingImages.ps1
```

### Option 2: Edit Manually
1. Open `Banner.bmp` and `Dialog.bmp` in an image editor
2. Modify as desired (keep the exact sizes!)
3. Save as BMP format
4. Rebuild the installer

### Option 3: Customize the Script
Edit `Create-BrandingImages.ps1` to change:
- Colors
- Fonts
- Text
- Logo positioning
- Gradients

## Branding Comparison

### Before (Default WiX UI)
- Generic banner with solid color
- Generic dialog with solid color
- No company branding
- Plain appearance

### After (Custom Branding)
- 3rdRock logo prominently displayed
- Product name in branded color scheme
- Professional gradient backgrounds
- Cohesive brand identity throughout installation

## Files Added/Modified

### New Files
- ? `PCToolsSetup\Banner.bmp` (493×58)
- ? `PCToolsSetup\Dialog.bmp` (493×312)
- ? `PCToolsSetup\Create-BrandingImages.ps1` (generator script)
- ? `PCToolsSetup\CREATE-BRANDING-IMAGES.md` (documentation)
- ? `PCToolsSetup\CUSTOM-BRANDING-SUMMARY.md` (this file)

### Modified Files
- ? `PCToolsSetup\Package.wxs` (added WixVariable references)
- ? `PCToolsSetup\PCToolsSetup.wixproj` (added Content items)

## Professional Touch

The installer now provides:
- ? **Branded appearance** throughout installation
- ? **Professional presentation** of your product
- ? **Consistent visual identity** with 3rdRock logo
- ? **Polished user experience** matching your application

## Additional Customization Ideas

Want to enhance further? Consider:

### Color Schemes
- Match your company's brand colors
- Use different gradients
- Add subtle patterns or textures

### Logo Variations
- Use different logo versions for banner vs dialog
- Add logo watermark to backgrounds
- Include product version in banner

### Text Styling
- Add company tagline
- Include copyright notice
- Add support/contact information

### Advanced Features
- Animated backgrounds (not recommended for installers)
- Multiple language versions with localized images
- Seasonal/promotional variants

## Maintenance

When updating the logo:
1. Replace `3rdRockLogo256.png` in `SSDToolsWPF.UI\Resource\`
2. Run `Create-BrandingImages.ps1` to regenerate BMPs
3. Rebuild the installer
4. Test the new branding

## Quality Checklist

Before distributing:
- ? Banner displays correctly on all screens
- ? Dialog image displays correctly
- ? Text is readable and well-positioned
- ? Colors match brand guidelines
- ? Images look professional at 96 DPI
- ? No pixelation or artifacts
- ? Consistent across all dialog screens

## Technical Notes

### BMP Format Required
Windows Installer (MSI) requires BMP format for branding images. PNG is not supported for these specific UI elements.

### Size Requirements
The exact pixel dimensions are required by WiX:
- Banner: 493 × 58 (no flexibility)
- Dialog: 493 × 312 (no flexibility)

Images with different sizes will be rejected or distorted.

### Color Depth
Use 24-bit BMP files for best compatibility and quality.

## Support

If you need to modify the branding:
- Review `CREATE-BRANDING-IMAGES.md` for detailed instructions
- Edit `Create-BrandingImages.ps1` for automated regeneration
- Use an image editor (Paint.NET, GIMP, Photoshop) for manual editing

---

**Status**: ? Complete - Custom branding applied  
**Installer Location**: `PCToolsSetup\bin\x64\Release\en-US\SSDOneStopTool-Setup.msi`  
**Ready to distribute**: Yes - Test first to verify branding appears correctly!
