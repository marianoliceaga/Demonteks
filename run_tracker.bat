@echo off
chcp 65001 >nul
echo ========================================
echo   DEMONTEKS - BACKGROUND ASSET TRACKER
echo ========================================
echo.
powershell -ExecutionPolicy Bypass -File "%~dp0download_backgrounds.ps1"
echo.
pause
