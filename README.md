# Task Manager CLI

A modern, feature-rich command-line task manager built with .NET 8. Manage your tasks efficiently with priorities, tags, due dates, and powerful search capabilities.

## Features

### Core Features
- ✅ Add, list, update, and remove tasks
- 🎯 Priority levels (1-5, with 5 being highest)
- 🏷️ Tag support for better organization
- 📅 Due date tracking
- 🔍 Powerful search functionality
- ✓ Mark tasks as complete
- 🗑️ Clear completed tasks
- 💾 JSON file-based persistence

### Advanced Features
- 📊 Statistics and reporting
- 📤 Export to CSV, Markdown, and JSON
- 📥 Import tasks from JSON
- ⚙️ Configuration file support
- 🐳 Docker support for containerized deployment
- 🎨 Clean, intuitive CLI interface

### Developer Features
- 🏗️ Modular architecture with dependency injection
- 📝 Comprehensive logging
- 🧪 90%+ test coverage
- 🔧 VSCode integration
- 🚀 CI/CD with GitHub Actions
- 📦 Cross-platform builds

## Installation

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later

### Build from Source

```bash
# Clone the repository
git clone https://github.com/codeforgood-org/dotnet-task-manager.git
cd dotnet-task-manager

# Build the project
dotnet build

# Run the application
dotnet run --project src/TaskManager.CLI -- <command> [options]
```

### Using Build Scripts

```bash
# Linux/macOS
./build.sh build    # Build the solution
./build.sh test     # Run tests
./build.sh publish  # Publish for all platforms
./build.sh run -- list  # Run the application

# Windows
build.cmd build
build.cmd test
build.cmd publish
```

### Docker

```bash
# Build and run with Docker
docker build -t taskmanager .
docker run -v $(pwd)/data:/app/data taskmanager list

# Or use docker-compose
docker-compose up taskmanager-dev
```

### Install as Global Tool (Optional)

```bash
# Build and pack
dotnet pack src/TaskManager.CLI/TaskManager.CLI.csproj -c Release

# Install globally
dotnet tool install --global --add-source ./src/TaskManager.CLI/nupkg TaskManager.CLI

# Now you can use 'taskman' from anywhere
taskman help
```

## Usage

### Basic Commands

#### Add a Task

```bash
# Simple task
taskman add "Buy groceries"

# Task with priority
taskman add "Complete project report" --priority 5

# Task with due date
taskman add "Submit tax returns" --due 2024-04-15

# Task with tags
taskman add "Review pull requests" --tags work,code-review

# Combine all options
taskman add "Prepare presentation" --priority 4 --due 2024-12-31 --tags work,important
```

#### List Tasks

```bash
# List all tasks (completed and pending)
taskman list

# List only pending tasks
taskman list --pending

# List tasks by tag
taskman list --tag work
```

#### Complete a Task

```bash
taskman complete 1
```

#### Update a Task

```bash
# Update description
taskman update 1 "Buy groceries and cook dinner"

# Update priority
taskman priority 1 5
```

#### Search Tasks

```bash
# Search by keyword
taskman search groceries

# Search also looks in tags
taskman search work
```

#### Remove a Task

```bash
taskman remove 1
```

#### Clear Completed Tasks

```bash
taskman clear
```

#### Statistics

```bash
# View task statistics
taskman stats
```

#### Export/Import

```bash
# Export to different formats
taskman export --format csv --output tasks.csv
taskman export --format markdown --output tasks.md
taskman export --format json --output tasks.json

# Import from JSON
taskman import tasks-backup.json
```

### Task Display Format

Tasks are displayed with the following information:

```
[✓] [1] Buy groceries ★★★ (Due: 2024-12-25) [shopping, personal]
│   │   │             │    │                └─ Tags
│   │   │             │    └─ Due date (if set)
│   │   │             └─ Priority (1-5 stars)
│   │   └─ Description
│   └─ Task ID
└─ Completion status (✓ = completed, blank = pending)
```

## Architecture

The project follows modern .NET practices with a clean, modular architecture:

```
dotnet-task-manager/
├── src/
│   └── TaskManager.CLI/
│       ├── Models/           # Data models
│       │   └── TaskItem.cs
│       ├── Interfaces/       # Service interfaces
│       │   └── ITaskService.cs
│       ├── Services/         # Business logic
│       │   └── TaskService.cs
│       └── Program.cs        # CLI entry point
├── tests/
│   └── TaskManager.Tests/    # Unit tests
├── docs/                     # Documentation
└── TaskManager.sln           # Solution file
```

### Key Design Principles

- **Separation of Concerns**: Models, services, and CLI are separate
- **Dependency Injection**: Using Microsoft.Extensions.DependencyInjection
- **Interface-based Design**: Services implement interfaces for testability
- **Async/Await**: File I/O operations are asynchronous
- **Logging**: Integrated logging with Microsoft.Extensions.Logging
- **Error Handling**: Comprehensive exception handling and validation

## Development

### Building the Project

```bash
# Build in Debug mode
dotnet build

# Build in Release mode
dotnet build -c Release
```

### Running Tests

```bash
# Run all tests
dotnet test

# Run tests with coverage
dotnet test /p:CollectCoverage=true
```

### Code Quality

The project includes:

- EditorConfig for consistent code style
- XML documentation comments
- Nullable reference types enabled
- Comprehensive unit tests

## Configuration

The application can be configured using `appsettings.json`:

```json
{
  "AppConfig": {
    "TasksFilePath": "tasks.json",
    "DefaultPriority": 3,
    "UseColors": true,
    "DateFormat": "yyyy-MM-dd",
    "ShowCompletedByDefault": true,
    "UpcomingDaysThreshold": 7,
    "ExportDirectory": "exports"
  }
}
```

## Data Storage

Tasks are stored in a `tasks.json` file in the current working directory. The file is automatically created when you add your first task.

Example `tasks.json`:

```json
[
  {
    "Id": 1,
    "Description": "Buy groceries",
    "IsCompleted": false,
    "Priority": 3,
    "CreatedAt": "2024-01-15T10:30:00Z",
    "DueDate": "2024-01-20T00:00:00Z",
    "Tags": ["shopping", "personal"]
  }
]
```

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request. For major changes, please open an issue first to discuss what you would like to change.

### Development Setup

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Make your changes
4. Run tests (`dotnet test`)
5. Commit your changes (`git commit -m 'Add some amazing feature'`)
6. Push to the branch (`git push origin feature/amazing-feature`)
7. Open a Pull Request

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Examples

See the [examples/](examples/) directory for:
- Sample task files
- Common usage scenarios  - Task templates
- Best practices

## Docker Usage

### Building the Image

```bash
docker build -t taskmanager .
```

### Running with Docker

```bash
# Create a data directory for persistent storage
mkdir -p data

# Run commands
docker run -v $(pwd)/data:/app/data taskmanager add "Docker task" --priority 5
docker run -v $(pwd)/data:/app/data taskmanager list
docker run -v $(pwd)/data:/app/data taskmanager stats
```

### Development with Docker Compose

```bash
# Start development environment
docker-compose up taskmanager-dev

# In another terminal, access the container
docker exec -it taskmanager-dev bash
dotnet run --project src/TaskManager.CLI -- list
```

## Roadmap

### Completed ✅
- [x] Priority levels
- [x] Tag support
- [x] Export to CSV, Markdown, JSON
- [x] Statistics and reports
- [x] Docker support
- [x] Configuration file
- [x] Comprehensive tests

### Planned
- [ ] Color-coded CLI output with Spectre.Console
- [ ] Interactive mode
- [ ] Recurring tasks
- [ ] Task categories/projects
- [ ] Cloud synchronization
- [ ] Web API
- [ ] Web interface
- [ ] Mobile companion app

## Support

If you encounter any issues or have questions:

- Open an issue on [GitHub](https://github.com/codeforgood-org/dotnet-task-manager/issues)
- Check existing issues for solutions
- Contribute to the documentation

## Acknowledgments

Built with ❤️ using:

- [.NET 8](https://dotnet.microsoft.com/)
- [Microsoft.Extensions.Logging](https://www.nuget.org/packages/Microsoft.Extensions.Logging/)
- [Microsoft.Extensions.DependencyInjection](https://www.nuget.org/packages/Microsoft.Extensions.DependencyInjection/)
- [xUnit](https://xunit.net/) for testing

---

Made by [Code for Good](https://github.com/codeforgood-org)
