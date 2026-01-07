# Startup Logging Guide

## Overview
The SSD One-Stop Tool now includes comprehensive startup logging to diagnose any initialization issues.

## Log File Locations

### Primary Log File
```
C:\SSDTools_Startup.log
```
This is the main startup log file that captures:
- Application initialization steps
- Exception details
- Service creation progress
- File path issues
- Any errors that occur during startup

### Fallback Log File
If the application cannot write to `C:\`, it will fallback to:
```
%TEMP%\SSDTools_Startup.log
```
Typically: `C:\Users\<YourName>\AppData\Local\Temp\SSDTools_Startup.log`

### Application Logs
Once the application successfully starts, operational logs are saved to:
```
%USERPROFILE%\Documents\SSDTools\SSD_Trim_Log.txt
```
Typically: `C:\Users\<YourName>\Documents\SSDTools\SSD_Trim_Log.txt`

## What Gets Logged

### Application Startup (`App.xaml.cs`)
- Constructor initialization
- Command-line arguments
- Current directory
- Whether running elevated (as admin)
- Exception handlers registration
- OnStartup events
- OnExit events
- All unhandled exceptions

### Main Window Initialization (`MainWindow.xaml.cs`)
- InitializeComponent completion
- Icon loading (PNG and ICO)
- Base folder creation
- Service initialization (Logging, Defrag, TRIM, TaskScheduler)
- ViewModel creation
- DataContext binding
- Any exceptions during window construction

## How to View Logs

### Option 1: View in Notepad
```powershell
notepad C:\SSDTools_Startup.log
```

### Option 2: View in Command Prompt
```cmd
type C:\SSDTools_Startup.log
```

### Option 3: View in PowerShell
```powershell
Get-Content C:\SSDTools_Startup.log -Tail 50
```

### Option 4: Open in Real-Time
```powershell
Get-Content C:\SSDTools_Startup.log -Wait -Tail 20
```

## Troubleshooting Steps

### Step 1: Run the Application
1. Install or run the application
2. If it crashes or closes immediately, proceed to Step 2

### Step 2: Check the Startup Log
```powershell
# Open the log file
notepad C:\SSDTools_Startup.log

# Or view last 30 lines
Get-Content C:\SSDTools_Startup.log -Tail 30
```

### Step 3: Look for ERROR Entries
Search for lines containing `[ERROR]` - these indicate exceptions or failures

### Step 4: Identify the Problem
Common issues and their log patterns:

#### Missing Directory
```
[MAINWINDOW] Failed to create base folder: Access denied
```
**Solution**: Check folder permissions or run as Administrator

#### Missing Files
```
[ERROR] OnStartup: FileNotFoundException: Could not load file or assembly
```
**Solution**: Verify all DLLs are present in installation directory

#### Icon Loading Issues
```
[MAINWINDOW] PNG icon load failed: IOException
```
**Solution**: Check Resource folder and icon files exist

#### Service Initialization Failure
```
[MAINWINDOW] Failed to create base folder: DirectoryNotFoundException
```
**Solution**: Base folder path issue - check Documents folder access

### Step 5: Clear Old Logs (Optional)
```powershell
# Delete old log to start fresh
Remove-Item C:\SSDTools_Startup.log -ErrorAction SilentlyContinue
```

## Log Entry Format

### INFO Messages
```
2026-01-06 14:30:45.123 [INFO] Application constructor started
2026-01-06 14:30:45.456 [MAINWINDOW] InitializeComponent completed
```

### ERROR Messages
```
2026-01-06 14:30:45.789 [ERROR] DispatcherUnhandledException: NullReferenceException: Object reference not set to an instance of an object.
StackTrace: at SSDToolsWPF.UI.MainWindow..ctor() in D:\PCTools\src\...
InnerException: <inner exception details if any>
```

## Key Changes Made

### 1. Fixed Hardcoded Path Issue
**Before:**
```csharp
string baseFolder = @"D:\PCTools";
```

**After:**
```csharp
string baseFolder = Path.Combine(
    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
    "SSDTools");
```

The application now uses:
- `C:\Users\<YourName>\Documents\SSDTools\` instead of `D:\PCTools`
- Automatically creates the folder if it doesn't exist
- Works on any Windows installation

### 2. Added Exception Handling
- All unhandled exceptions are caught and logged
- User-friendly error messages with log file location
- Application tries to recover gracefully

### 3. Added Detailed Logging
- Every initialization step is logged
- Timestamps with millisecond precision
- Clear context for each log entry
- Separate tags for different components

## Testing the Logging

### Test 1: Normal Startup
```powershell
# Run the application
& "C:\Program Files\3rdRock\SSD One-Stop Tool\SSDToolsWPF.UI.exe"

# Check the log - should show successful initialization
Get-Content C:\SSDTools_Startup.log -Tail 10
```

Expected output:
```
2026-01-06 14:30:45.123 [INFO] Application constructor started
2026-01-06 14:30:45.234 [INFO] Exception handlers registered
2026-01-06 14:30:45.345 [INFO] OnStartup called
2026-01-06 14:30:45.456 [MAINWINDOW] MainWindow constructor started
2026-01-06 14:30:45.567 [MAINWINDOW] InitializeComponent completed
2026-01-06 14:30:45.678 [MAINWINDOW] PNG icon loaded successfully
2026-01-06 14:30:45.789 [MAINWINDOW] ICO icon loaded successfully
2026-01-06 14:30:45.890 [MAINWINDOW] Base folder: C:\Users\Jim\Documents\SSDTools
2026-01-06 14:30:46.001 [MAINWINDOW] MainWindow constructor completed successfully
2026-01-06 14:30:46.112 [INFO] OnStartup completed successfully
```

### Test 2: Simulate Error
To test error logging, you can temporarily break something and observe the logs.

## Common Error Scenarios

### Scenario 1: Missing .NET Runtime
**Log Entry:**
```
[ERROR] OnStartup: FileNotFoundException: Could not load file or assembly 'System.Runtime'
```
**Solution:** Install .NET 10 Desktop Runtime

### Scenario 2: Permission Denied
**Log Entry:**
```
[MAINWINDOW] Failed to create base folder: UnauthorizedAccessException: Access denied
```
**Solution:** Run as Administrator or check folder permissions

### Scenario 3: Missing Resource Files
**Log Entry:**
```
[MAINWINDOW] PNG icon stream was null
```
**Solution:** Verify Resource folder exists in installation directory

### Scenario 4: XAML Parsing Error
**Log Entry:**
```
[ERROR] OnStartup: XamlParseException: Cannot create instance of 'MainWindow'
```
**Solution:** Check XAML files for syntax errors

## Support

When reporting issues, please include:
1. The complete contents of `C:\SSDTools_Startup.log`
2. Windows version (Run: `winver`)
3. .NET version (Run: `dotnet --list-runtimes`)
4. Installation path
5. Whether running as Administrator

## Log File Maintenance

The startup log file is append-only and will grow over time.

### To Clear the Log
```powershell
# Delete the log file
Remove-Item C:\SSDTools_Startup.log -Force

# Or truncate it
Clear-Content C:\SSDTools_Startup.log
```

### To Archive Old Logs
```powershell
# Move to archive with timestamp
$timestamp = Get-Date -Format "yyyyMMdd_HHmmss"
Move-Item C:\SSDTools_Startup.log "C:\SSDTools_Startup_$timestamp.log"
```

---

**Note**: The startup log is separate from the operational log. The operational log tracks SSD maintenance operations and is located in your Documents folder.
