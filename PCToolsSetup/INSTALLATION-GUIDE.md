# SSD One-Stop Tool - Interactive Installation Guide

## Overview
The installer now includes a **full interactive wizard** that guides users through the installation process with multiple dialog screens.

## Installation Wizard Screens

### 1. **Welcome Screen**
- Displays product name and version
- Brief introduction to the application
- **Action**: Click "Next" to continue

### 2. **License Agreement**
- Shows the MIT License
- **Action**: Read and accept the license agreement by selecting "I accept the terms in the License Agreement"
- Click "Next" to continue

### 3. **Setup Type**
- Choose installation type:
  - **Complete**: Install all features (recommended)
  - **Custom**: Select which features to install
- **Action**: Select your preferred installation type

### 4. **Custom Setup** (if Custom selected)
- Select features to install:
  - ? **SSD One-Stop Tool** (Required - Cannot be deselected)
    - Main application executable
    - Core libraries
    - Resource files
  - ? **Start Menu Shortcuts** (Optional)
    - Creates shortcuts under 3rdRock folder in Start Menu
  - ? **Desktop Shortcut** (Optional)
    - Creates a shortcut on your desktop
- Click on feature icons to:
  - **Install on local hard drive** (default)
  - **Entire feature will be unavailable**
- **Action**: Customize your installation, then click "Next"

### 5. **Installation Folder**
- Default location: `C:\Program Files\3rdRock\SSD One-Stop Tool\`
- **Action**: 
  - Accept default location, OR
  - Click "Change..." to select a different folder
- Click "Next" to continue

### 6. **Ready to Install**
- Review your installation choices
- Summary of selected features and installation location
- **Action**: 
  - Click "Install" to begin installation, OR
  - Click "Back" to change settings

### 7. **Installing Progress**
- Shows real-time installation progress
- Progress bar and status messages
- Installing files, creating shortcuts, updating registry
- **Action**: Wait for installation to complete (typically 10-30 seconds)

### 8. **Installation Complete**
- Confirmation that installation was successful
- Optional: "Launch SSD One-Stop Tool" checkbox (checked by default)
- **Action**: 
  - Check/uncheck "Launch SSD One-Stop Tool"
  - Click "Finish" to exit the wizard

## Command-Line Installation Options

### Silent Installation (No UI)
```powershell
# Install silently with all features
msiexec /i "SSDOneStopTool-Setup.msi" /quiet /norestart

# Install silently with specific features
msiexec /i "SSDOneStopTool-Setup.msi" /quiet /norestart ADDLOCAL=MainApplication,StartMenuShortcuts

# Install silently without desktop shortcut
msiexec /i "SSDOneStopTool-Setup.msi" /quiet /norestart ADDLOCAL=MainApplication,StartMenuShortcuts REMOVE=DesktopShortcut
```

### Installation with Logging
```powershell
# Create detailed installation log
msiexec /i "SSDOneStopTool-Setup.msi" /l*v "C:\Temp\install-log.txt"

# Silent install with logging
msiexec /i "SSDOneStopTool-Setup.msi" /quiet /norestart /l*v "C:\Temp\install-log.txt"
```

### Custom Installation Directory
```powershell
# Install to custom location
msiexec /i "SSDOneStopTool-Setup.msi" INSTALLFOLDER="D:\CustomPath\SSDTools"

# Silent install to custom location
msiexec /i "SSDOneStopTool-Setup.msi" /quiet /norestart INSTALLFOLDER="D:\CustomPath\SSDTools"
```

### Uninstallation
```powershell
# Interactive uninstall
msiexec /x "SSDOneStopTool-Setup.msi"

# Silent uninstall
msiexec /x "SSDOneStopTool-Setup.msi" /quiet /norestart

# Silent uninstall with log
msiexec /x "SSDOneStopTool-Setup.msi" /quiet /norestart /l*v "C:\Temp\uninstall-log.txt"
```

### Repair Installation
```powershell
# Repair installation (reinstall all files)
msiexec /fa "SSDOneStopTool-Setup.msi"

# Repair with logging
msiexec /fa "SSDOneStopTool-Setup.msi" /l*v "C:\Temp\repair-log.txt"
```

## Feature IDs for Command-Line Installation

Use these feature IDs with the `ADDLOCAL` or `REMOVE` properties:

| Feature ID | Description |
|------------|-------------|
| `MainApplication` | Core application (Required) |
| `StartMenuShortcuts` | Start Menu shortcuts |
| `DesktopShortcut` | Desktop shortcut |

## Installation Properties

| Property | Description | Default Value |
|----------|-------------|---------------|
| `INSTALLFOLDER` | Installation directory | `C:\Program Files\3rdRock\SSD One-Stop Tool` |
| `ADDLOCAL` | Features to install | `ALL` |
| `REMOVE` | Features to exclude | (none) |

## Post-Installation

After successful installation:

### Files Installed:
```
C:\Program Files\3rdRock\SSD One-Stop Tool\
??? SSDToolsWPF.UI.exe              # Main executable
??? SSDToolsWPF.Core.dll            # Core library
??? Microsoft.Win32.TaskScheduler.dll
??? SSDToolsWPF.UI.deps.json
??? SSDToolsWPF.UI.runtimeconfig.json
??? Resource\
    ??? favicon.ico
    ??? ssd_drive.png
    ??? 3rdRockLogo256.png
    ??? drive-image.png
```

### Start Menu Shortcut:
```
Start Menu\Programs\3rdRock\SSD One-Stop Tool
```

### Desktop Shortcut (if selected):
```
Desktop\SSD One-Stop Tool
```

### Registry Entries:
```
HKEY_CURRENT_USER\Software\3rdRock\SSDOneStopTool
??? installed = 1
??? desktopShortcut = 1 (if desktop shortcut installed)
```

### Add/Remove Programs Entry:
- **Display Name**: SSD One-Stop Tool
- **Publisher**: 3rdRock
- **Version**: 1.0.0.0
- **Install Location**: C:\Program Files\3rdRock\SSD One-Stop Tool

## Testing the Installation

### Manual Testing Steps:
1. **Double-click** `SSDOneStopTool-Setup.msi`
2. Go through each wizard screen
3. Select different options to test feature selection
4. Test changing installation directory
5. Verify the application launches if checkbox is selected
6. Check Start Menu and Desktop shortcuts
7. Verify application runs correctly
8. Test uninstallation through Control Panel

### Automated Testing:
```powershell
# Test silent install
msiexec /i "SSDOneStopTool-Setup.msi" /quiet /norestart /l*v "install-test.log"

# Verify installation
Test-Path "C:\Program Files\3rdRock\SSD One-Stop Tool\SSDToolsWPF.UI.exe"

# Test silent uninstall
msiexec /x "SSDOneStopTool-Setup.msi" /quiet /norestart /l*v "uninstall-test.log"

# Verify uninstallation
Test-Path "C:\Program Files\3rdRock\SSD One-Stop Tool\" -PathType Container
```

## Troubleshooting

### Common Issues:

**"Another version is already installed"**
- Uninstall the existing version first
- Check Add/Remove Programs

**"Installation failed"**
- Check the log file: `msiexec /i "SSDOneStopTool-Setup.msi" /l*v "error-log.txt"`
- Verify Windows Installer service is running
- Run as Administrator

**"Cannot modify installation"**
- The installer doesn't support modify/repair through Control Panel
- Uninstall and reinstall if changes are needed

**".NET Runtime not found"**
- Install .NET 10 Desktop Runtime from https://dotnet.microsoft.com/
- Restart the installer

## Distribution

The MSI file is ready for distribution and can be:
- Shared via network drives
- Deployed through Group Policy
- Downloaded from a website
- Included on USB drives
- Deployed via System Center Configuration Manager (SCCM)
- Deployed via Intune or other MDM solutions

## System Requirements

- **OS**: Windows 10 or later (64-bit)
- **.NET**: .NET 10 Desktop Runtime
- **Disk Space**: ~50 MB
- **RAM**: Minimum 512 MB
- **Administrator Rights**: Required for SSD maintenance operations

---

**Version**: 1.0.0  
**Last Updated**: 2026-01-06  
**Developer**: Jim Barber for 3rdRock
