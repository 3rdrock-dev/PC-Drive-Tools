# Quick Testing Guide

## What Was Fixed

### 1. **Hardcoded Path Issue** ? ? ?
- **Problem**: Application used hardcoded path `D:\PCTools` which didn't exist on installation machine
- **Fix**: Now uses `%USERPROFILE%\Documents\SSDTools` (dynamic, user-specific path)

### 2. **No Error Logging** ? ? ?
- **Problem**: Application crashed silently with no diagnostic information
- **Fix**: Comprehensive logging to `C:\SSDTools_Startup.log`

### 3. **No Exception Handling** ? ? ?
- **Problem**: Unhandled exceptions caused immediate crash
- **Fix**: Global exception handlers with logging and user messages

## How to Test

### Step 1: Uninstall Old Version (if installed)
```powershell
# Open Settings > Apps > Installed apps
# Find "SSD One-Stop Tool" and uninstall
```

### Step 2: Delete Old Log (to start fresh)
```powershell
Remove-Item C:\SSDTools_Startup.log -Force -ErrorAction SilentlyContinue
```

### Step 3: Install New Version
1. Navigate to: `D:\PCTools\src\SSDToolsWPF\PCToolsSetup\bin\x64\Release\en-US\`
2. Double-click: `SSDOneStopTool-Setup.msi`
3. Follow the installation wizard
4. **Important**: Check "Launch SSD One-Stop Tool" at the end

### Step 4: Check If Application Stays Open
- Application should open and stay running
- You should see the main window with all controls
- Check for any error messages

### Step 5: View the Startup Log
```powershell
# Open the log file
notepad C:\SSDTools_Startup.log
```

### Step 6: Verify Expected Log Contents

**Successful startup should show:**
```
[INFO] Application constructor started
[INFO] Exception handlers registered
[INFO] OnStartup called
[MAINWINDOW] MainWindow constructor started
[MAINWINDOW] InitializeComponent completed
[MAINWINDOW] PNG icon loaded successfully (or failed - not critical)
[MAINWINDOW] ICO icon loaded successfully (or failed - not critical)
[MAINWINDOW] Base folder: C:\Users\<YourName>\Documents\SSDTools
[MAINWINDOW] Created base folder (if didn't exist)
[MAINWINDOW] Creating services...
[MAINWINDOW] LoggingService created
[MAINWINDOW] DefragService created
[MAINWINDOW] TrimService created
[MAINWINDOW] TaskSchedulerService created
[MAINWINDOW] Creating MainViewModel...
[MAINWINDOW] MainViewModel created
[MAINWINDOW] DataContext set
[MAINWINDOW] MainWindow constructor completed successfully
[INFO] OnStartup completed successfully
```

### Step 7: Verify Application Folders

Check that these folders were created:
```powershell
# Should exist and contain log file
Test-Path "$env:USERPROFILE\Documents\SSDTools"

# List contents
Get-ChildItem "$env:USERPROFILE\Documents\SSDTools"
```

Expected files:
- `SSD_Trim_Log.txt` - Operational log (created when you use features)
- `TrimTasks\` - Folder for scheduled task scripts

## If Application Still Crashes

### Immediately Check the Log
```powershell
Get-Content C:\SSDTools_Startup.log
```

### Look for [ERROR] Lines
These indicate what failed:

#### Example Error 1: Missing DLL
```
[ERROR] OnStartup: FileNotFoundException: Could not load file or assembly 'Microsoft.Win32.TaskScheduler'
```
**Solution**: Reinstall - DLL is missing

#### Example Error 2: Permission Denied
```
[MAINWINDOW] Failed to create base folder: UnauthorizedAccessException
```
**Solution**: Check folder permissions or run as Administrator

#### Example Error 3: XAML Parse Error
```
[ERROR] XamlParseException: Cannot create instance of 'MainWindow'
```
**Solution**: Resource files may be missing - check installation directory

### Check Installation Directory
```powershell
# Navigate to installation folder
cd "C:\Program Files\3rdRock\SSD One-Stop Tool"

# List all files
Get-ChildItem -Recurse

# Should see:
# - SSDToolsWPF.UI.exe
# - SSDToolsWPF.Core.dll
# - Microsoft.Win32.TaskScheduler.dll
# - *.json, *.dll files
# - Resource\ folder with icons
```

## Alternative Testing Without Installer

### Test Directly from Build Output
```powershell
# Run from Debug build
cd D:\PCTools\src\SSDToolsWPF\SSDToolsWPF.UI\bin\Debug\net10.0-windows7.0
.\SSDToolsWPF.UI.exe

# Check log
notepad C:\SSDTools_Startup.log
```

This tests the application without installing.

## Verify Fixes Applied

### Fix 1: Dynamic Path
Check log for line like:
```
[MAINWINDOW] Base folder: C:\Users\<YourName>\Documents\SSDTools
```
Should NOT be `D:\PCTools`

### Fix 2: Folder Creation
Check log for:
```
[MAINWINDOW] Created base folder: C:\Users\<YourName>\Documents\SSDTools
```

### Fix 3: Exception Handling
If an error occurs, you should see:
- A message box with error details
- Full exception logged to C:\SSDTools_Startup.log
- Application doesn't just silently crash

## Reporting Results

### If Successful ?
1. Application opens and stays running
2. No error messages
3. Log shows "completed successfully"
4. All features work

### If Failed ?
Provide:
1. **Complete log contents**:
   ```powershell
   Get-Content C:\SSDTools_Startup.log | Out-File -FilePath Desktop\startup-log.txt
   ```

2. **Windows version**:
   ```powershell
   Get-ComputerInfo | Select-Object WindowsProductName, WindowsVersion, OsArchitecture
   ```

3. **Installation path**:
   ```
   C:\Program Files\3rdRock\SSD One-Stop Tool\
   ```

4. **List installed files**:
   ```powershell
   Get-ChildItem "C:\Program Files\3rdRock\SSD One-Stop Tool" -Recurse | Select-Object FullName
   ```

## Additional Diagnostics

### Check .NET Runtime
```powershell
dotnet --list-runtimes
```
Should show: `Microsoft.WindowsDesktop.App 10.0.x`

### Check Event Viewer
```powershell
# Open Event Viewer
eventvwr

# Navigate to: Windows Logs > Application
# Look for errors from "SSDToolsWPF.UI" or ".NET Runtime"
```

### Run as Administrator
Right-click the application and select "Run as administrator"
- Check if this makes a difference
- Log should show: `[INFO] Is elevated: True`

## Success Criteria

? Application launches without crashing  
? Main window displays correctly  
? Log file created at C:\SSDTools_Startup.log  
? No [ERROR] entries in log  
? Documents\SSDTools folder created  
? All buttons/features accessible  

---

**Current Installer Location**:  
`D:\PCTools\src\SSDToolsWPF\PCToolsSetup\bin\x64\Release\en-US\SSDOneStopTool-Setup.msi`

**Log Documentation**:  
See `SSDToolsWPF.UI\STARTUP-LOGGING.md` for complete logging details
