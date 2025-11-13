# Contributing to Task Manager CLI

Thank you for your interest in contributing to Task Manager CLI! This document provides guidelines and instructions for contributing.

## Code of Conduct

Please be respectful and constructive in all interactions. We're building this together!

## Getting Started

### Prerequisites

- .NET 8 SDK or later
- Git
- A code editor (VS Code, Visual Studio, Rider, etc.)

### Setting Up Development Environment

1. Fork the repository on GitHub
2. Clone your fork locally:
   ```bash
   git clone https://github.com/YOUR_USERNAME/dotnet-task-manager.git
   cd dotnet-task-manager
   ```

3. Add the upstream remote:
   ```bash
   git remote add upstream https://github.com/codeforgood-org/dotnet-task-manager.git
   ```

4. Restore dependencies:
   ```bash
   dotnet restore
   ```

5. Build the project:
   ```bash
   ./build.sh build    # Linux/macOS
   build.cmd build     # Windows
   ```

## Development Workflow

### Creating a Branch

Create a new branch for your feature or bugfix:

```bash
git checkout -b feature/your-feature-name
# or
git checkout -b fix/your-bugfix-name
```

### Making Changes

1. Write your code following our coding standards (see below)
2. Add or update tests for your changes
3. Ensure all tests pass:
   ```bash
   ./build.sh test
   ```
4. Format your code:
   ```bash
   dotnet format
   ```

### Coding Standards

- Follow the .editorconfig settings
- Use meaningful variable and method names
- Add XML documentation comments for public APIs
- Keep methods focused and small
- Write unit tests for new functionality
- Maintain or improve code coverage

### Project Structure

```
src/TaskManager.CLI/
├── Models/           # Data models
├── Interfaces/       # Service interfaces
├── Services/         # Business logic implementation
└── Program.cs        # CLI entry point

tests/TaskManager.Tests/
└── TaskServiceTests.cs  # Unit tests
```

### Testing Guidelines

- Write unit tests for all new functionality
- Use descriptive test names that explain what is being tested
- Follow the Arrange-Act-Assert pattern
- Use xUnit for test framework
- Use Moq for mocking dependencies

Example test:

```csharp
[Fact]
public void AddTask_WithValidDescription_ReturnsTask()
{
    // Arrange
    var service = CreateService();
    var description = "Test task";

    // Act
    var task = service.AddTask(description);

    // Assert
    Assert.NotNull(task);
    Assert.Equal(description, task.Description);
}
```

## Submitting Changes

### Commit Messages

Write clear, concise commit messages:

- Use the imperative mood ("Add feature" not "Added feature")
- First line should be 50 characters or less
- Include more details in the body if necessary
- Reference issue numbers when applicable

Example:
```
Add search functionality for tasks

- Implement SearchTasks method in TaskService
- Add search command to CLI
- Add unit tests for search functionality

Fixes #123
```

### Pull Requests

1. Update your branch with the latest upstream changes:
   ```bash
   git fetch upstream
   git rebase upstream/main
   ```

2. Push your changes to your fork:
   ```bash
   git push origin feature/your-feature-name
   ```

3. Create a pull request on GitHub with:
   - Clear description of changes
   - Reference to related issues
   - Screenshots if applicable (for UI changes)
   - Confirmation that tests pass

### Pull Request Checklist

- [ ] Code follows project coding standards
- [ ] All tests pass locally
- [ ] New tests added for new functionality
- [ ] Documentation updated if needed
- [ ] Commit messages are clear and descriptive
- [ ] Branch is up to date with main
- [ ] No merge conflicts

## Building and Testing

### Build Commands

```bash
# Clean build artifacts
./build.sh clean

# Restore dependencies
./build.sh restore

# Build the project
./build.sh build

# Run tests
./build.sh test

# Run tests with coverage
./build.sh coverage

# Publish for all platforms
./build.sh publish

# Run the application
./build.sh run -- <command> [options]

# Format code
./build.sh format

# Run full pipeline
./build.sh all
```

### Running Tests

```bash
# Run all tests
dotnet test

# Run tests with detailed output
dotnet test --verbosity detailed

# Run specific test
dotnet test --filter "FullyQualifiedName~AddTask_WithValidDescription"

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage"
```

## Adding New Features

When adding new features:

1. **Check existing issues** - Someone might already be working on it
2. **Create an issue** - Discuss your idea before starting work
3. **Design first** - Think about the interface and architecture
4. **Write tests** - Test-driven development is encouraged
5. **Implement** - Write clean, documented code
6. **Update docs** - Update README.md and other documentation
7. **Submit PR** - Follow the pull request guidelines

## Reporting Bugs

When reporting bugs, include:

- **Description** - Clear description of the issue
- **Steps to reproduce** - Detailed steps to reproduce the bug
- **Expected behavior** - What you expected to happen
- **Actual behavior** - What actually happened
- **Environment** - OS, .NET version, etc.
- **Screenshots** - If applicable

## Feature Requests

When requesting features:

- **Use case** - Explain why this feature would be useful
- **Proposed solution** - Describe how you envision it working
- **Alternatives** - Any alternative solutions you've considered
- **Additional context** - Any other relevant information

## Questions?

If you have questions:

- Check existing issues and pull requests
- Review the README.md
- Open a new issue with the "question" label

## Recognition

Contributors will be recognized in:

- The project README.md
- Release notes
- GitHub contributors page

Thank you for contributing to Task Manager CLI!
