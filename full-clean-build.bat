@echo off
echo ============================================
echo  PC Drive Tools - Complete Clean Build
echo ============================================
echo.

echo [1/4] Stopping all instances...
taskkill /F /IM "PCDriveTools.exe" 2>nul
taskkill /F /IM "PC Drive Tools.exe" 2>nul
timeout /t 2 /nobreak >nul

echo [2/4] Cleaning build artifacts...
cd /d "%~dp0"
for /d /r . %%d in (bin,obj) do @if exist "%%d" rd /s /q "%%d"

echo [3/4] Removing Visual Studio cache...
if exist ".vs" rd /s /q ".vs"

echo [4/4] Building solution...
dotnet clean
dotnet build --no-incremental

echo.
echo ============================================
echo Build complete! You can now open VS.
echo ============================================
pause
