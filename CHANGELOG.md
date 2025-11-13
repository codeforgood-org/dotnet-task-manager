# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [2.0.0] - 2024-01-15

### Added
- Complete project restructure with modern .NET architecture
- Modular code organization (Models, Services, Interfaces, CLI)
- Priority system (1-5 levels) for tasks
- Tag support for better task organization
- Due date tracking for tasks
- Task completion status
- Search functionality across descriptions and tags
- Statistics and reporting features
  - Overall task statistics
  - Tasks by priority breakdown
  - Tasks by tag analysis
  - Overdue task tracking
  - Upcoming task notifications
- Export functionality
  - Export to CSV format
  - Export to Markdown format
  - Export to JSON format
  - Import from JSON format
- Configuration file support (appsettings.json)
- Comprehensive unit tests with xUnit
  - Service layer tests
  - Integration tests
  - Export/Import tests
  - Statistics tests
- Docker support
  - Dockerfile for containerized deployment
  - docker-compose.yml for development
  - .dockerignore for optimized builds
- VSCode integration
  - tasks.json for common operations
  - launch.json for debugging
  - settings.json for editor configuration
  - recommended extensions
- GitHub Actions CI/CD pipeline
  - Automated testing
  - Code coverage reporting
  - Multi-platform builds (Linux, Windows, macOS)
- Build automation scripts
  - build.sh for Linux/macOS
  - build.cmd for Windows
- Comprehensive documentation
  - Enhanced README with examples
  - CONTRIBUTING guidelines
  - ARCHITECTURE documentation
  - EXAMPLES with usage scenarios
- EditorConfig for consistent code style
- Dependency injection throughout the application
- Structured logging with Microsoft.Extensions.Logging
- Async/await for all I/O operations

### Changed
- Migrated from single file to proper .NET solution structure
- Task model enhanced with additional properties
- File-based storage improved with async operations
- Command-line interface redesigned with better UX
- Error handling and validation improved
- Updated .gitignore for .NET projects

### Technical Improvements
- Implemented SOLID principles
- Interface-based design for testability
- Comprehensive error handling
- Input validation at all layers
- Nullable reference types enabled
- XML documentation comments throughout

## [1.0.0] - 2024-01-01

### Added
- Initial release
- Basic task management (add, list, remove)
- JSON file storage
- Simple command-line interface
- MIT License

### Features
- Add tasks with descriptions
- List all tasks
- Remove tasks by ID
- Persistent storage in tasks.json

---

## Upcoming Features

### [2.1.0] - Planned
- Enhanced CLI output with Spectre.Console
  - Color-coded task display
  - Interactive task selection
  - Progress bars for operations
  - Rich tables for task lists
- Task categories/projects
- Recurring tasks support
- Task notes and comments
- Task history and audit log

### [2.2.0] - Planned
- Web API for remote access
- RESTful endpoints
- Authentication and authorization
- Cloud storage integration
- Multi-user support

### [3.0.0] - Future
- Database backend (SQLite/PostgreSQL)
- Web interface
- Mobile companion app
- Real-time synchronization
- Team collaboration features
- Task dependencies
- Gantt chart visualization
- Calendar integration
- Email notifications
- Task templates library

---

## Migration Guide

### Upgrading from 1.0.0 to 2.0.0

Your existing `tasks.json` file will need to be migrated to include new fields:

**Old format (1.0.0):**
```json
[
  {
    "Id": 1,
    "Description": "Sample task"
  }
]
```

**New format (2.0.0):**
```json
[
  {
    "Id": 1,
    "Description": "Sample task",
    "IsCompleted": false,
    "Priority": 3,
    "CreatedAt": "2024-01-01T10:00:00Z",
    "DueDate": null,
    "Tags": []
  }
]
```

**Migration steps:**
1. Backup your existing `tasks.json` file
2. The application will automatically add default values for new fields
3. Update tasks with priorities and tags as needed

---

## Breaking Changes

### Version 2.0.0
- **File Structure**: Solution restructured, source moved to `src/` directory
- **Build Process**: Now requires .NET 8 SDK
- **Command Output**: Format changed for better readability
- **Configuration**: Now supports appsettings.json for configuration

---

## Contributors

- Task Manager Team
- Code for Good Community

For detailed contribution guidelines, see [CONTRIBUTING.md](CONTRIBUTING.md).

---

## Support

- **Issues**: [GitHub Issues](https://github.com/codeforgood-org/dotnet-task-manager/issues)
- **Discussions**: [GitHub Discussions](https://github.com/codeforgood-org/dotnet-task-manager/discussions)
- **Documentation**: See [README.md](README.md) and [docs/](docs/)
