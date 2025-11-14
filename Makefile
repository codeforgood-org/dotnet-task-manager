.PHONY: help build test clean restore run publish docker install uninstall format coverage

# Variables
PROJECT_DIR := src/TaskManager.CLI
TEST_DIR := tests/TaskManager.Tests
CONFIGURATION := Release
OUTPUT_DIR := publish

# Colors for output
COLOR_RESET := \033[0m
COLOR_INFO := \033[36m
COLOR_SUCCESS := \033[32m
COLOR_WARNING := \033[33m

help: ## Show this help message
	@echo "$(COLOR_INFO)Task Manager CLI - Makefile Commands$(COLOR_RESET)"
	@echo ""
	@grep -E '^[a-zA-Z_-]+:.*?## .*$$' $(MAKEFILE_LIST) | sort | awk 'BEGIN {FS = ":.*?## "}; {printf "  $(COLOR_SUCCESS)%-15s$(COLOR_RESET) %s\n", $$1, $$2}'

build: restore ## Build the project
	@echo "$(COLOR_INFO)Building project...$(COLOR_RESET)"
	@dotnet build --configuration $(CONFIGURATION) --no-restore
	@echo "$(COLOR_SUCCESS)Build complete!$(COLOR_RESET)"

test: ## Run all tests
	@echo "$(COLOR_INFO)Running tests...$(COLOR_RESET)"
	@dotnet test --configuration $(CONFIGURATION) --verbosity normal
	@echo "$(COLOR_SUCCESS)Tests complete!$(COLOR_RESET)"

clean: ## Clean build artifacts
	@echo "$(COLOR_INFO)Cleaning build artifacts...$(COLOR_RESET)"
	@dotnet clean
	@rm -rf $(OUTPUT_DIR)
	@echo "$(COLOR_SUCCESS)Clean complete!$(COLOR_RESET)"

restore: ## Restore NuGet packages
	@echo "$(COLOR_INFO)Restoring packages...$(COLOR_RESET)"
	@dotnet restore
	@echo "$(COLOR_SUCCESS)Restore complete!$(COLOR_RESET)"

run: ## Run the application (use ARGS="..." to pass arguments)
	@echo "$(COLOR_INFO)Running application...$(COLOR_RESET)"
	@dotnet run --project $(PROJECT_DIR) -- $(ARGS)

publish: ## Publish for all platforms
	@echo "$(COLOR_INFO)Publishing for all platforms...$(COLOR_RESET)"
	@mkdir -p $(OUTPUT_DIR)
	@echo "  Building Linux x64..."
	@dotnet publish $(PROJECT_DIR)/TaskManager.CLI.csproj \
		-c $(CONFIGURATION) \
		-r linux-x64 \
		--self-contained \
		-o $(OUTPUT_DIR)/linux-x64
	@echo "  Building Windows x64..."
	@dotnet publish $(PROJECT_DIR)/TaskManager.CLI.csproj \
		-c $(CONFIGURATION) \
		-r win-x64 \
		--self-contained \
		-o $(OUTPUT_DIR)/win-x64
	@echo "  Building macOS x64..."
	@dotnet publish $(PROJECT_DIR)/TaskManager.CLI.csproj \
		-c $(CONFIGURATION) \
		-r osx-x64 \
		--self-contained \
		-o $(OUTPUT_DIR)/osx-x64
	@echo "  Building macOS ARM64..."
	@dotnet publish $(PROJECT_DIR)/TaskManager.CLI.csproj \
		-c $(CONFIGURATION) \
		-r osx-arm64 \
		--self-contained \
		-o $(OUTPUT_DIR)/osx-arm64
	@echo "$(COLOR_SUCCESS)Publish complete! Binaries in $(OUTPUT_DIR)/$(COLOR_RESET)"

docker-build: ## Build Docker image
	@echo "$(COLOR_INFO)Building Docker image...$(COLOR_RESET)"
	@docker build -t taskmanager:latest .
	@echo "$(COLOR_SUCCESS)Docker build complete!$(COLOR_RESET)"

docker-run: ## Run Docker container
	@echo "$(COLOR_INFO)Running Docker container...$(COLOR_RESET)"
	@docker run -v $$(pwd)/data:/app/data taskmanager:latest $(ARGS)

install: publish ## Install as global tool
	@echo "$(COLOR_INFO)Installing as global tool...$(COLOR_RESET)"
	@dotnet tool uninstall -g taskman || true
	@dotnet pack $(PROJECT_DIR)/TaskManager.CLI.csproj -c $(CONFIGURATION)
	@dotnet tool install --global --add-source $(PROJECT_DIR)/bin/$(CONFIGURATION) taskman
	@echo "$(COLOR_SUCCESS)Install complete! Run 'taskman help' to get started.$(COLOR_RESET)"

uninstall: ## Uninstall global tool
	@echo "$(COLOR_INFO)Uninstalling global tool...$(COLOR_RESET)"
	@dotnet tool uninstall -g taskman
	@echo "$(COLOR_SUCCESS)Uninstall complete!$(COLOR_RESET)"

format: ## Format code
	@echo "$(COLOR_INFO)Formatting code...$(COLOR_RESET)"
	@dotnet format
	@echo "$(COLOR_SUCCESS)Format complete!$(COLOR_RESET)"

coverage: ## Run tests with coverage
	@echo "$(COLOR_INFO)Running tests with coverage...$(COLOR_RESET)"
	@dotnet test --collect:"XPlat Code Coverage" --results-directory ./coverage
	@echo "$(COLOR_SUCCESS)Coverage complete! Results in ./coverage/$(COLOR_RESET)"

lint: ## Run code analysis
	@echo "$(COLOR_INFO)Running code analysis...$(COLOR_RESET)"
	@dotnet format --verify-no-changes --verbosity diagnostic
	@echo "$(COLOR_SUCCESS)Lint complete!$(COLOR_RESET)"

watch: ## Run with file watcher
	@echo "$(COLOR_INFO)Starting file watcher...$(COLOR_RESET)"
	@dotnet watch --project $(PROJECT_DIR) run

all: clean restore build test ## Run clean, restore, build, and test
	@echo "$(COLOR_SUCCESS)All tasks complete!$(COLOR_RESET)"

dev: ## Start development environment
	@echo "$(COLOR_INFO)Starting development environment...$(COLOR_RESET)"
	@docker-compose up taskmanager-dev

# Examples
example-add: ## Example: Add a task
	@$(MAKE) run ARGS='add "Sample task from Makefile" --priority 4 --tags demo'

example-list: ## Example: List tasks
	@$(MAKE) run ARGS='list'

example-stats: ## Example: Show statistics
	@$(MAKE) run ARGS='stats'

# Default target
.DEFAULT_GOAL := help
