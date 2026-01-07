# SSD One-Stop Tool - Installation Package

This directory contains the WiX Toolset installer project for the SSD One-Stop Tool application.

## About SSD One-Stop Tool

SSD One-Stop Tool is a Windows utility that helps maintain solid-state drives (SSDs) for optimal performance and longevity.

**Key Features:**
- Repairs the Windows "Optimize Drives" service
- Runs TRIM commands on SSDs
- Sets up automatic monthly maintenance tasks
- Monitors drive status and optimization progress

**Version:** 1.0.0  
**Developer:** Jim Barber for 3rdRock  
**License:** MIT

## Interactive Installation Wizard

The installer now includes a **full-featured interactive wizard** that guides users through the installation process:

### Installation Screens:
1. **Welcome** - Introduction to the application
2. **License Agreement** - MIT License acceptance
3. **Setup Type** - Complete or Custom installation
4. **Custom Setup** - Select features to install (if Custom selected)
5. **Installation Folder** - Choose installation directory
6. **Ready to Install** - Review choices before installing
7. **Installing** - Progress bar with real-time status
8. **Complete** - Success message with launch option

### Selectable Features:
- ? **SSD One-Stop Tool** (Required) - Main application files
- ? **Start Menu Shortcuts** (Optional) - Shortcuts in Start Menu
- ? **Desktop Shortcut** (Optional) - Desktop shortcut icon

?? **See [INSTALLATION-GUIDE.md](INSTALLATION-GUIDE.md) for complete details on using the interactive installer.**

## Building the Installer

### Prerequisites
1. **WiX Toolset 6.0.2** or later
2. **.NET 10 SDK** or later
3. **Visual Studio 2022** or later (recommended)

### Build Steps

#### From Visual Studio:
1. Open the solution file in Visual Studio
2. Set build configuration to **Release**
3. Set platform to **x64**
4. Build the **PCToolsSetup** project
5. The installer will be generated in `PCToolsSetup\bin\x64\Release\en-US\`

#### From Command Line:
```powershell
# Navigate to the solution directory
cd D:\PCTools\src\SSDToolsWPF

# Build the installer
dotnet build PCToolsSetup\PCToolsSetup.wixproj -c Release

# Output location:
# PCToolsSetup\bin\x64\Release\en-US\SSDOneStopTool-Setup.msi
```

## Running the Installer

### Interactive Installation:
```powershell
# Double-click the MSI file, or run:
.\SSDOneStopTool-Setup.msi

# With detailed logging:
msiexec /i SSDOneStopTool-Setup.msi /l*v install.log
```

### Silent Installation (No UI):
```powershell
# Install all features silently
msiexec /i SSDOneStopTool-Setup.msi /quiet /norestart

# Install with specific features only
msiexec /i SSDOneStopTool-Setup.msi /quiet /norestart ADDLOCAL=MainApplication,StartMenuShortcuts

# Install to custom location
msiexec /i SSDOneStopTool-Setup.msi /quiet /norestart INSTALLFOLDER="D:\CustomPath\SSDTools"
```

### Uninstallation:
```powershell
# Interactive uninstall
msiexec /x SSDOneStopTool-Setup.msi

# Silent uninstall
msiexec /x SSDOneStopTool-Setup.msi /quiet /norestart

# Or use Windows Settings > Apps > Installed apps
```

## Installer Features

The installation package provides:

### User Experience
- ?? **Interactive wizard** with 8 step-by-step screens
- ?? **License agreement** display and acceptance
- ?? **Custom feature selection** (choose what to install)
- ?? **Installation directory selection** with browse dialog
- ?? **Real-time progress** tracking during installation
- ?? **Launch application** option after installation
- ? **Professional UI** consistent with Windows standards

### Technical Features
- ? **Upgrade support** - Automatically upgrades previous versions
- ? **Proper uninstaller** - Via Add/Remove Programs
- ? **Silent installation** - For enterprise deployment
- ? **Detailed logging** - For troubleshooting
- ? **Custom actions** - Launch app after install (optional)
- ? **Feature selection** - Modular installation
- ? **Rollback support** - Automatic rollback on failure

### Installed Components
- **Main Application** - Core executable and libraries
- **Resources** - Icons, images, and assets
- **Start Menu Shortcuts** - Easy access from Start Menu
- **Desktop Shortcut** - Quick launch icon (optional)
- **Registry Entries** - Application settings and tracking

## Installation Directory Structure

Default installation path: `C:\Program Files\3rdRock\SSD One-Stop Tool\`

```
SSD One-Stop Tool\
??? SSDToolsWPF.UI.exe              # Main application
??? SSDToolsWPF.Core.dll            # Core library
??? Microsoft.Win32.TaskScheduler.dll
??? SSDToolsWPF.UI.deps.json       # Dependencies manifest
??? SSDToolsWPF.UI.runtimeconfig.json
??? Resource\
    ??? favicon.ico
    ??? ssd_drive.png
    ??? 3rdRockLogo256.png
    ??? drive-image.png
```

## System Requirements

- **Operating System:** Windows 10 or later (Windows 11 recommended)
- **.NET Runtime:** .NET 10.0 Desktop Runtime
- **Architecture:** x64 (64-bit)
- **Disk Space:** ~50 MB
- **Administrator Rights:** Required for SSD maintenance operations

## Distribution

The generated MSI installer is ready for distribution via:
- ?? **Direct download** - Share the MSI file
- ?? **Web hosting** - Host on internal/external websites
- ?? **USB drives** - Portable distribution
- ?? **Enterprise deployment** - Group Policy, SCCM, Intune
- ?? **Email** - Send to users (if organization allows)

The installer includes:
- All necessary application files
- Professional interactive wizard
- Desktop and Start Menu shortcuts
- Proper uninstall support
- Automatic upgrade capability

## Customization

### Changing Product Information
Edit `Package.wxs` to modify:
- Product name and description
- Version number (update for each release)
- Manufacturer information
- UpgradeCode (?? only change if starting completely new product)

### Modifying Installed Files
Edit `ExampleComponents.wxs` to:
- Add or remove files
- Update file paths
- Change component GUIDs (use new GUIDs for new components)

### Adjusting UI Behavior
Edit `Package.wxs` UI section to:
- Change default feature selections
- Modify installation flow
- Add custom dialogs
- Change launch behavior

### Localization
- Edit `Package.en-us.wxl` for English text
- Create new `.wxl` files for other languages (e.g., `Package.fr-fr.wxl`)

## Project Files

| File | Purpose |
|------|---------|
| `Package.wxs` | Main installer configuration and UI |
| `ExampleComponents.wxs` | File components and shortcuts |
| `Folders.wxs` | Directory structure (currently unused) |
| `Package.en-us.wxl` | English localization strings |
| `License.rtf` | MIT License for display in installer |
| `README.md` | This file - project documentation |
| `INSTALLATION-GUIDE.md` | Detailed user installation guide |
| `PCToolsSetup.wixproj` | WiX project configuration |

## Troubleshooting

### Build Errors

**"File not found" errors:**
- Ensure SSDToolsWPF.UI project is built in Release configuration first
- Verify paths in ExampleComponents.wxs match your build output
- Check that all resource files exist in the specified locations

**"Multiple entry sections" errors:**
- Verify `EnableDefaultCompileItems` is set to `false` in .wixproj
- Check that only Package.wxs and ExampleComponents.wxs are compiled

**"ICE validation" warnings:**
- These are Windows Installer best practices warnings
- Most can be safely ignored for internal distribution
- Review each warning to understand its impact

### Installation Errors

**"Another version is already installed":**
- Uninstall existing version via Control Panel
- Or the installer should auto-upgrade (check UpgradeCode)

**"Insufficient privileges":**
- Right-click installer and select "Run as Administrator"
- Default location requires admin rights

**".NET Runtime not found":**
- Install .NET 10 Desktop Runtime from https://dotnet.microsoft.com/
- Restart installer after runtime installation

### Testing Tips

```powershell
# Test complete installation cycle with logging
msiexec /i SSDOneStopTool-Setup.msi /l*v install-full.log

# Verify installation
Test-Path "C:\Program Files\3rdRock\SSD One-Stop Tool\SSDToolsWPF.UI.exe"
Get-ItemProperty "HKCU:\Software\3rdRock\SSDOneStopTool"

# Test uninstallation
msiexec /x SSDOneStopTool-Setup.msi /l*v uninstall-full.log

# Verify clean uninstall
Test-Path "C:\Program Files\3rdRock\SSD One-Stop Tool"
```

## Support & Resources

- **GitHub Repository:** https://github.com/3rdrock-dev/PC-Drive-Tools
- **Installation Guide:** See [INSTALLATION-GUIDE.md](INSTALLATION-GUIDE.md)
- **WiX Documentation:** https://wixtoolset.org/docs/
- **Issue Reporting:** GitHub Issues

---

**Copyright © 2026 3rdRock**  
Developed by Jim Barber  
Installer built with WiX Toolset v6
