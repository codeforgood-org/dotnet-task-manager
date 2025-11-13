@echo off
setlocal enabledelayedexpansion

echo =====================================
echo     Task Manager - Build Script
echo =====================================
echo.

set COMMAND=%1
set CONFIGURATION=%2

if "%COMMAND%"=="" set COMMAND=build
if "%CONFIGURATION%"=="" set CONFIGURATION=Release

if /I "%COMMAND%"=="clean" goto :clean
if /I "%COMMAND%"=="restore" goto :restore
if /I "%COMMAND%"=="build" goto :build
if /I "%COMMAND%"=="test" goto :test
if /I "%COMMAND%"=="coverage" goto :coverage
if /I "%COMMAND%"=="publish" goto :publish
if /I "%COMMAND%"=="run" goto :run
if /I "%COMMAND%"=="format" goto :format
if /I "%COMMAND%"=="all" goto :all
if /I "%COMMAND%"=="help" goto :help
goto :help

:clean
echo Cleaning build artifacts...
dotnet clean
if exist publish rd /s /q publish
echo Clean completed!
echo.
goto :end

:restore
echo Restoring dependencies...
dotnet restore
echo Restore completed!
echo.
goto :end

:build
echo Building solution...
dotnet restore
dotnet build --configuration %CONFIGURATION% --no-restore
echo Build completed!
echo.
goto :end

:test
echo Running tests...
dotnet test --configuration %CONFIGURATION% --verbosity normal
echo Tests completed!
echo.
goto :end

:coverage
echo Running tests with coverage...
dotnet test --configuration %CONFIGURATION% --collect:"XPlat Code Coverage" --verbosity normal
echo Coverage completed!
echo.
goto :end

:publish
echo Publishing application...
if not exist publish mkdir publish

echo Publishing for Linux x64...
dotnet publish src\TaskManager.CLI\TaskManager.CLI.csproj -c %CONFIGURATION% -r linux-x64 --self-contained -o .\publish\linux-x64

echo Publishing for Windows x64...
dotnet publish src\TaskManager.CLI\TaskManager.CLI.csproj -c %CONFIGURATION% -r win-x64 --self-contained -o .\publish\win-x64

echo Publishing for macOS x64...
dotnet publish src\TaskManager.CLI\TaskManager.CLI.csproj -c %CONFIGURATION% -r osx-x64 --self-contained -o .\publish\osx-x64

echo Publish completed! Binaries available in .\publish\
echo.
goto :end

:run
echo Running application...
shift
dotnet run --project src\TaskManager.CLI -- %*
goto :end

:format
echo Formatting code...
dotnet format
echo Format completed!
echo.
goto :end

:all
echo Running full build pipeline...
call build.cmd clean
call build.cmd restore
call build.cmd build %CONFIGURATION%
call build.cmd test
echo Full build pipeline completed!
echo.
goto :end

:help
echo Task Manager Build Script
echo.
echo Usage: build.cmd ^<command^> [configuration]
echo.
echo Commands:
echo   clean              Clean build artifacts
echo   restore            Restore NuGet packages
echo   build              Build the solution (default: Release)
echo   test               Run unit tests
echo   coverage           Run tests with code coverage
echo   publish            Publish for all platforms
echo   run [args]         Run the application with optional arguments
echo   format             Format code using dotnet format
echo   all                Run clean, restore, build, and test
echo   help               Show this help message
echo.
echo Configuration:
echo   Debug              Build with debug symbols
echo   Release            Build optimized release (default)
echo.
echo Examples:
echo   build.cmd build
echo   build.cmd build Debug
echo   build.cmd test
echo   build.cmd run list
echo   build.cmd publish
goto :end

:end
endlocal
