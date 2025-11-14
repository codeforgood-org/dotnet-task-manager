# Architecture Documentation

## Overview

Task Manager CLI is built using modern .NET practices with a clean, modular architecture that emphasizes separation of concerns, testability, and maintainability.

## Architecture Diagram

```
┌─────────────────────────────────────────────────────────┐
│                     Program.cs (CLI)                     │
│  - Command parsing                                       │
│  - Dependency injection setup                            │
│  - User interaction                                      │
└────────────────────┬────────────────────────────────────┘
                     │
                     │ Uses
                     ▼
┌─────────────────────────────────────────────────────────┐
│              ITaskService (Interface)                    │
│  - AddTask()                                             │
│  - GetAllTasks()                                         │
│  - RemoveTask()                                          │
│  - CompleteTask()                                        │
│  - UpdateTask()                                          │
│  - SearchTasks()                                         │
│  - etc.                                                  │
└────────────────────┬────────────────────────────────────┘
                     │
                     │ Implemented by
                     ▼
┌─────────────────────────────────────────────────────────┐
│              TaskService (Service)                       │
│  - Business logic                                        │
│  - Validation                                            │
│  - Logging                                               │
│  - File I/O operations                                   │
└────────────────────┬────────────────────────────────────┘
                     │
                     │ Uses
                     ▼
┌─────────────────────────────────────────────────────────┐
│               TaskItem (Model)                           │
│  - Id                                                    │
│  - Description                                           │
│  - IsCompleted                                           │
│  - Priority                                              │
│  - CreatedAt                                             │
│  - DueDate                                               │
│  - Tags                                                  │
└─────────────────────────────────────────────────────────┘
```

## Layers

### 1. Presentation Layer (Program.cs)

**Responsibility**: User interaction and command-line interface

**Key Components**:
- Command parsing and routing
- User input validation
- Output formatting
- Dependency injection configuration

**Design Decisions**:
- Uses async/await for all I/O operations
- Follows command pattern for different operations
- Minimal business logic (delegates to service layer)

### 2. Service Layer (Services/)

**Responsibility**: Business logic and data management

**Key Components**:
- `ITaskService`: Interface defining task operations
- `TaskService`: Implementation of business logic

**Design Decisions**:
- Interface-based for testability
- Async file operations
- Comprehensive logging
- Input validation
- Error handling

**Key Features**:
- Task CRUD operations
- Search and filtering
- Priority management
- Tag support
- Data persistence

### 3. Model Layer (Models/)

**Responsibility**: Data structures and domain entities

**Key Components**:
- `TaskItem`: Core task entity

**Design Decisions**:
- Rich domain model with behavior (ToString override)
- Value objects for task properties
- Immutable where appropriate
- Well-documented properties

## Design Patterns

### Dependency Injection

The application uses Microsoft.Extensions.DependencyInjection for IoC:

```csharp
services.AddLogging(builder => {
    builder.AddConsole();
    builder.SetMinimumLevel(LogLevel.Warning);
});
services.AddSingleton<ITaskService, TaskService>();
```

**Benefits**:
- Loose coupling
- Easy testing with mocks
- Flexible configuration
- Standard .NET pattern

### Repository Pattern (Implicit)

While not explicitly a repository, TaskService implements a data access pattern:

```csharp
public async Task LoadTasksAsync()
public async Task SaveTasksAsync()
```

**Benefits**:
- Abstraction over data storage
- Easy to swap storage mechanisms
- Testable without file system

### Strategy Pattern (Implicit)

Command routing in Program.cs uses a strategy-like pattern:

```csharp
var result = command switch
{
    "add" => await HandleAddCommand(args, taskService),
    "list" => await HandleListCommand(args, taskService),
    // ...
};
```

## Data Flow

### Adding a Task

```
User Input → Program.cs → TaskService.AddTask() → TaskItem created
                                                 ↓
User Output ← Program.cs ← Return TaskItem ← Add to list
                                                 ↓
File Save ← TaskService.SaveTasksAsync() ← JSON serialization
```

### Listing Tasks

```
User Input → Program.cs → TaskService.GetAllTasks() → Filter & Sort
                                                     ↓
User Output ← Format & Display ← Return IEnumerable<TaskItem>
```

## Data Persistence

### Storage Format: JSON

Tasks are stored in `tasks.json`:

```json
[
  {
    "Id": 1,
    "Description": "Task description",
    "IsCompleted": false,
    "Priority": 3,
    "CreatedAt": "2024-01-15T10:30:00Z",
    "DueDate": "2024-01-20T00:00:00Z",
    "Tags": ["tag1", "tag2"]
  }
]
```

**Rationale**:
- Human-readable
- Easy to debug
- Standard format
- Cross-platform compatible
- Easy to migrate to other storage

### Storage Operations

- **Load**: Async read from file, deserialize JSON
- **Save**: Serialize to JSON, async write to file
- **Error Handling**: Graceful fallback on read errors

## Error Handling Strategy

### Validation Errors

```csharp
if (string.IsNullOrWhiteSpace(description))
{
    throw new ArgumentException("Task description cannot be empty.");
}
```

**Approach**: Fast fail with descriptive messages

### File I/O Errors

```csharp
try
{
    await File.WriteAllTextAsync(_fileName, json);
}
catch (Exception ex)
{
    _logger.LogError(ex, "Error saving tasks");
    throw;
}
```

**Approach**: Log and propagate for user notification

### User Input Errors

```csharp
if (!int.TryParse(args[1], out int id))
{
    Console.WriteLine("Error: Please provide a valid task ID.");
    return 1;
}
```

**Approach**: Friendly messages with usage hints

## Logging Strategy

Uses Microsoft.Extensions.Logging with console provider:

```csharp
_logger.LogInformation("Added task #{Id}: {Description}", task.Id, task.Description);
_logger.LogWarning("Task #{Id} not found", id);
_logger.LogError(ex, "Error loading tasks from {FileName}", _fileName);
```

**Levels**:
- **Information**: Normal operations
- **Warning**: Not found scenarios
- **Error**: Exceptions and failures

**Default**: Only warnings and errors shown to users

## Testing Strategy

### Unit Tests

Focus on service layer with mocked dependencies:

```csharp
[Fact]
public void AddTask_WithValidDescription_ReturnsTask()
{
    // Arrange
    var mockLogger = new Mock<ILogger<TaskService>>();
    var service = new TaskService(mockLogger.Object, "test.json");

    // Act
    var task = service.AddTask("Test");

    // Assert
    Assert.NotNull(task);
}
```

### Test Coverage Goals

- Service layer: 90%+ coverage
- Models: Property tests
- CLI: Integration tests (future)

## Security Considerations

### Input Validation

- All user input is validated
- SQL injection: N/A (no database)
- Path traversal: Fixed filename
- Command injection: N/A (no shell execution)

### File System

- Controlled file access
- No arbitrary file operations
- JSON deserialization with type safety

## Performance Considerations

### In-Memory Operations

- Tasks stored in memory during session
- O(n) operations for most queries
- Acceptable for typical use (< 10,000 tasks)

### File I/O

- Async operations prevent blocking
- Full file read/write (no streaming needed for typical use)
- Could be optimized with SQLite for large datasets

### Sorting and Filtering

```csharp
return _tasks
    .Where(t => !t.IsCompleted)
    .OrderByDescending(t => t.Priority)
    .ThenBy(t => t.CreatedAt);
```

Efficient LINQ queries with deferred execution

## Extensibility Points

### Custom Storage

Implement `ITaskService` with different storage:

```csharp
public class DatabaseTaskService : ITaskService
{
    // Use Entity Framework or Dapper
}
```

### Additional Commands

Add new commands in Program.cs:

```csharp
"export" => await HandleExportCommand(args, taskService),
```

### Task Properties

Extend `TaskItem` model:

```csharp
public string? AssignedTo { get; set; }
public TaskCategory Category { get; set; }
```

## Future Architectural Considerations

### Scalability

For larger datasets, consider:
- SQLite for storage
- Indexed queries
- Pagination for list commands

### Multi-User

For shared task lists:
- Cloud storage (Azure, AWS)
- Conflict resolution
- Authentication

### Plugin System

For extensibility:
- Plugin interface
- Dynamic loading
- Command registry

### API Layer

For remote access:
- ASP.NET Core Web API
- REST endpoints
- Authentication/authorization

## Conclusion

The current architecture provides:
- Clear separation of concerns
- High testability
- Easy maintenance
- Room for growth

The design prioritizes simplicity and clarity while maintaining professional software engineering practices.
