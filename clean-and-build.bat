@echo off
echo Cleaning solution...

REM Kill any running instances
taskkill /F /IM "PCDriveTools.exe" 2>nul
taskkill /F /IM "PC Drive Tools.exe" 2>nul

REM Clean bin and obj folders
for /d /r . %%d in (bin,obj) do @if exist "%%d" rd /s /q "%%d"

echo Building solution...
dotnet build --no-incremental

echo Done! Press any key to exit...
pause >nul
