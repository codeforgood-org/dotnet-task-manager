#!/bin/bash
set -e

# Colors for output
GREEN='\033[0;32m'
BLUE='\033[0;34m'
RED='\033[0;31m'
NC='\033[0m' # No Color

echo -e "${BLUE}=====================================${NC}"
echo -e "${BLUE}    Task Manager - Build Script     ${NC}"
echo -e "${BLUE}=====================================${NC}\n"

# Parse command line arguments
COMMAND=${1:-build}
CONFIGURATION=${2:-Release}

case $COMMAND in
  clean)
    echo -e "${BLUE}Cleaning build artifacts...${NC}"
    dotnet clean
    rm -rf ./publish
    echo -e "${GREEN}Clean completed!${NC}\n"
    ;;

  restore)
    echo -e "${BLUE}Restoring dependencies...${NC}"
    dotnet restore
    echo -e "${GREEN}Restore completed!${NC}\n"
    ;;

  build)
    echo -e "${BLUE}Building solution...${NC}"
    dotnet restore
    dotnet build --configuration $CONFIGURATION --no-restore
    echo -e "${GREEN}Build completed!${NC}\n"
    ;;

  test)
    echo -e "${BLUE}Running tests...${NC}"
    dotnet test --configuration $CONFIGURATION --verbosity normal
    echo -e "${GREEN}Tests completed!${NC}\n"
    ;;

  coverage)
    echo -e "${BLUE}Running tests with coverage...${NC}"
    dotnet test --configuration $CONFIGURATION --collect:"XPlat Code Coverage" --verbosity normal
    echo -e "${GREEN}Coverage completed!${NC}\n"
    ;;

  publish)
    echo -e "${BLUE}Publishing application...${NC}"

    # Create publish directory
    mkdir -p ./publish

    # Publish for Linux
    echo -e "${BLUE}Publishing for Linux x64...${NC}"
    dotnet publish src/TaskManager.CLI/TaskManager.CLI.csproj \
      -c $CONFIGURATION \
      -r linux-x64 \
      --self-contained \
      -o ./publish/linux-x64

    # Publish for Windows
    echo -e "${BLUE}Publishing for Windows x64...${NC}"
    dotnet publish src/TaskManager.CLI/TaskManager.CLI.csproj \
      -c $CONFIGURATION \
      -r win-x64 \
      --self-contained \
      -o ./publish/win-x64

    # Publish for macOS
    echo -e "${BLUE}Publishing for macOS x64...${NC}"
    dotnet publish src/TaskManager.CLI/TaskManager.CLI.csproj \
      -c $CONFIGURATION \
      -r osx-x64 \
      --self-contained \
      -o ./publish/osx-x64

    echo -e "${GREEN}Publish completed! Binaries available in ./publish/${NC}\n"
    ;;

  run)
    echo -e "${BLUE}Running application...${NC}"
    dotnet run --project src/TaskManager.CLI -- "${@:2}"
    ;;

  format)
    echo -e "${BLUE}Formatting code...${NC}"
    dotnet format
    echo -e "${GREEN}Format completed!${NC}\n"
    ;;

  all)
    echo -e "${BLUE}Running full build pipeline...${NC}"
    ./build.sh clean
    ./build.sh restore
    ./build.sh build $CONFIGURATION
    ./build.sh test
    echo -e "${GREEN}Full build pipeline completed!${NC}\n"
    ;;

  help|*)
    echo "Task Manager Build Script"
    echo ""
    echo "Usage: ./build.sh <command> [configuration]"
    echo ""
    echo "Commands:"
    echo "  clean              Clean build artifacts"
    echo "  restore            Restore NuGet packages"
    echo "  build              Build the solution (default: Release)"
    echo "  test               Run unit tests"
    echo "  coverage           Run tests with code coverage"
    echo "  publish            Publish for all platforms"
    echo "  run [args]         Run the application with optional arguments"
    echo "  format             Format code using dotnet format"
    echo "  all                Run clean, restore, build, and test"
    echo "  help               Show this help message"
    echo ""
    echo "Configuration:"
    echo "  Debug              Build with debug symbols"
    echo "  Release            Build optimized release (default)"
    echo ""
    echo "Examples:"
    echo "  ./build.sh build"
    echo "  ./build.sh build Debug"
    echo "  ./build.sh test"
    echo "  ./build.sh run -- list"
    echo "  ./build.sh publish"
    ;;
esac
