# API Documentation

This document describes the public API of the Task Manager CLI library.

## Table of Contents

- [Interfaces](#interfaces)
- [Models](#models)
- [Services](#services)
- [Usage Examples](#usage-examples)

## Interfaces

### ITaskService

Main interface for task management operations.

```csharp
public interface ITaskService
{
    Task LoadTasksAsync();
    Task SaveTasksAsync();
    TaskItem AddTask(string description, int priority = 3, DateTime? dueDate = null, List<string>? tags = null);
    IEnumerable<TaskItem> GetAllTasks(bool includeCompleted = true);
    TaskItem? GetTaskById(int id);
    bool RemoveTask(int id);
    bool CompleteTask(int id);
    bool UpdateTask(int id, string newDescription);
    bool UpdateTaskPriority(int id, int priority);
    IEnumerable<TaskItem> SearchTasks(string query);
    IEnumerable<TaskItem> GetTasksByTag(string tag);
    int ClearCompletedTasks();
}
```

#### Methods

##### LoadTasksAsync()
Loads tasks from storage asynchronously.

**Returns**: `Task`

**Example**:
```csharp
await taskService.LoadTasksAsync();
```

##### SaveTasksAsync()
Saves tasks to storage asynchronously.

**Returns**: `Task`

**Example**:
```csharp
await taskService.SaveTasksAsync();
```

##### AddTask()
Adds a new task with the specified parameters.

**Parameters**:
- `description` (string): Task description (required)
- `priority` (int): Priority level 1-5 (default: 3)
- `dueDate` (DateTime?): Optional due date
- `tags` (List<string>?): Optional list of tags

**Returns**: `TaskItem` - The created task

**Throws**: `ArgumentException` if description is empty or priority is invalid

**Example**:
```csharp
var task = taskService.AddTask(
    "Complete project report",
    priority: 5,
    dueDate: DateTime.Now.AddDays(7),
    tags: new List<string> { "work", "urgent" }
);
```

##### GetAllTasks()
Gets all tasks, optionally including completed tasks.

**Parameters**:
- `includeCompleted` (bool): Whether to include completed tasks (default: true)

**Returns**: `IEnumerable<TaskItem>` - Collection of tasks

**Example**:
```csharp
// Get all tasks
var allTasks = taskService.GetAllTasks();

// Get only pending tasks
var pendingTasks = taskService.GetAllTasks(includeCompleted: false);
```

##### GetTaskById()
Gets a specific task by ID.

**Parameters**:
- `id` (int): The task ID

**Returns**: `TaskItem?` - The task if found, null otherwise

**Example**:
```csharp
var task = taskService.GetTaskById(1);
if (task != null)
{
    Console.WriteLine(task.Description);
}
```

##### RemoveTask()
Removes a task by ID.

**Parameters**:
- `id` (int): The task ID

**Returns**: `bool` - True if removed, false if not found

**Example**:
```csharp
if (taskService.RemoveTask(1))
{
    Console.WriteLine("Task removed");
}
```

##### CompleteTask()
Marks a task as completed.

**Parameters**:
- `id` (int): The task ID

**Returns**: `bool` - True if marked completed, false if not found

**Example**:
```csharp
taskService.CompleteTask(1);
```

##### UpdateTask()
Updates a task's description.

**Parameters**:
- `id` (int): The task ID
- `newDescription` (string): The new description

**Returns**: `bool` - True if updated, false if not found

**Throws**: `ArgumentException` if new description is empty

**Example**:
```csharp
taskService.UpdateTask(1, "Updated task description");
```

##### UpdateTaskPriority()
Updates a task's priority level.

**Parameters**:
- `id` (int): The task ID
- `priority` (int): The new priority (1-5)

**Returns**: `bool` - True if updated, false if not found

**Throws**: `ArgumentException` if priority is invalid

**Example**:
```csharp
taskService.UpdateTaskPriority(1, 5); // Set to highest priority
```

##### SearchTasks()
Searches tasks by description or tags.

**Parameters**:
- `query` (string): Search query

**Returns**: `IEnumerable<TaskItem>` - Matching tasks

**Example**:
```csharp
var results = taskService.SearchTasks("meeting");
```

##### GetTasksByTag()
Gets tasks with a specific tag.

**Parameters**:
- `tag` (string): Tag to filter by

**Returns**: `IEnumerable<TaskItem>` - Tasks with the tag

**Example**:
```csharp
var workTasks = taskService.GetTasksByTag("work");
```

##### ClearCompletedTasks()
Removes all completed tasks.

**Returns**: `int` - Number of tasks removed

**Example**:
```csharp
int count = taskService.ClearCompletedTasks();
Console.WriteLine($"Cleared {count} tasks");
```

### IExportService

Interface for exporting and importing tasks.

```csharp
public interface IExportService
{
    Task ExportToCsvAsync(IEnumerable<TaskItem> tasks, string filePath);
    Task ExportToMarkdownAsync(IEnumerable<TaskItem> tasks, string filePath);
    Task ExportToJsonAsync(IEnumerable<TaskItem> tasks, string filePath);
    Task<List<TaskItem>> ImportFromJsonAsync(string filePath);
}
```

#### Methods

##### ExportToCsvAsync()
Exports tasks to CSV format.

**Parameters**:
- `tasks` (IEnumerable<TaskItem>): Tasks to export
- `filePath` (string): Output file path

**Returns**: `Task`

**Example**:
```csharp
await exportService.ExportToCsvAsync(tasks, "tasks.csv");
```

##### ExportToMarkdownAsync()
Exports tasks to Markdown format.

**Parameters**:
- `tasks` (IEnumerable<TaskItem>): Tasks to export
- `filePath` (string): Output file path

**Returns**: `Task`

**Example**:
```csharp
await exportService.ExportToMarkdownAsync(tasks, "tasks.md");
```

##### ExportToJsonAsync()
Exports tasks to JSON format.

**Parameters**:
- `tasks` (IEnumerable<TaskItem>): Tasks to export
- `filePath` (string): Output file path

**Returns**: `Task`

**Example**:
```csharp
await exportService.ExportToJsonAsync(tasks, "backup.json");
```

##### ImportFromJsonAsync()
Imports tasks from JSON file.

**Parameters**:
- `filePath` (string): Input file path

**Returns**: `Task<List<TaskItem>>` - Imported tasks

**Throws**: `FileNotFoundException` if file doesn't exist

**Example**:
```csharp
var importedTasks = await exportService.ImportFromJsonAsync("backup.json");
```

### IStatisticsService

Interface for task statistics and analytics.

```csharp
public interface IStatisticsService
{
    TaskStatistics GetStatistics(IEnumerable<TaskItem> tasks);
    Dictionary<int, int> GetTasksByPriority(IEnumerable<TaskItem> tasks);
    Dictionary<string, int> GetTasksByTag(IEnumerable<TaskItem> tasks);
    IEnumerable<TaskItem> GetOverdueTasks(IEnumerable<TaskItem> tasks);
    IEnumerable<TaskItem> GetUpcomingTasks(IEnumerable<TaskItem> tasks, int days = 7);
}
```

#### Methods

##### GetStatistics()
Gets overall task statistics.

**Parameters**:
- `tasks` (IEnumerable<TaskItem>): Tasks to analyze

**Returns**: `TaskStatistics` - Statistics summary

**Example**:
```csharp
var stats = statisticsService.GetStatistics(tasks);
Console.WriteLine($"Completion Rate: {stats.CompletionRate:F1}%");
```

##### GetTasksByPriority()
Gets pending tasks grouped by priority.

**Parameters**:
- `tasks` (IEnumerable<TaskItem>): Tasks to group

**Returns**: `Dictionary<int, int>` - Priority to count mapping

**Example**:
```csharp
var priorityBreakdown = statisticsService.GetTasksByPriority(tasks);
foreach (var (priority, count) in priorityBreakdown)
{
    Console.WriteLine($"Priority {priority}: {count} tasks");
}
```

##### GetTasksByTag()
Gets tasks grouped by tag.

**Parameters**:
- `tasks` (IEnumerable<TaskItem>): Tasks to group

**Returns**: `Dictionary<string, int>` - Tag to count mapping

**Example**:
```csharp
var tagBreakdown = statisticsService.GetTasksByTag(tasks);
```

##### GetOverdueTasks()
Gets all overdue pending tasks.

**Parameters**:
- `tasks` (IEnumerable<TaskItem>): Tasks to check

**Returns**: `IEnumerable<TaskItem>` - Overdue tasks

**Example**:
```csharp
var overdue = statisticsService.GetOverdueTasks(tasks);
foreach (var task in overdue)
{
    Console.WriteLine($"Overdue: {task.Description}");
}
```

##### GetUpcomingTasks()
Gets tasks due within specified days.

**Parameters**:
- `tasks` (IEnumerable<TaskItem>): Tasks to check
- `days` (int): Number of days to look ahead (default: 7)

**Returns**: `IEnumerable<TaskItem>` - Upcoming tasks

**Example**:
```csharp
var upcoming = statisticsService.GetUpcomingTasks(tasks, days: 3);
```

## Models

### TaskItem

Represents a task with all its properties.

```csharp
public class TaskItem
{
    public int Id { get; set; }
    public string Description { get; set; }
    public bool IsCompleted { get; set; }
    public int Priority { get; set; } // 1-5
    public DateTime CreatedAt { get; set; }
    public DateTime? DueDate { get; set; }
    public List<string> Tags { get; set; }
}
```

**Example**:
```csharp
var task = new TaskItem
{
    Id = 1,
    Description = "Buy groceries",
    IsCompleted = false,
    Priority = 3,
    CreatedAt = DateTime.UtcNow,
    DueDate = DateTime.UtcNow.AddDays(1),
    Tags = new List<string> { "shopping", "personal" }
};
```

### TaskStatistics

Contains statistical information about tasks.

```csharp
public class TaskStatistics
{
    public int TotalTasks { get; set; }
    public int CompletedTasks { get; set; }
    public int PendingTasks { get; set; }
    public int OverdueTasks { get; set; }
    public int DueToday { get; set; }
    public int DueThisWeek { get; set; }
    public double CompletionRate { get; }
    public double AveragePriority { get; set; }
    public int TotalTags { get; set; }
}
```

### AppConfig

Application configuration settings.

```csharp
public class AppConfig
{
    public string TasksFilePath { get; set; } = "tasks.json";
    public int DefaultPriority { get; set; } = 3;
    public bool UseColors { get; set; } = true;
    public string DateFormat { get; set; } = "yyyy-MM-dd";
    public bool ShowCompletedByDefault { get; set; } = true;
    public int UpcomingDaysThreshold { get; set; } = 7;
    public string ExportDirectory { get; set; } = "exports";
}
```

## Services

### TaskService

Main implementation of task management.

**Constructor**:
```csharp
public TaskService(ILogger<TaskService> logger, string? fileName = null)
```

**Example Usage**:
```csharp
var logger = serviceProvider.GetRequiredService<ILogger<TaskService>>();
var taskService = new TaskService(logger, "my-tasks.json");

await taskService.LoadTasksAsync();
var task = taskService.AddTask("New task", priority: 4);
await taskService.SaveTasksAsync();
```

### ExportService

Implementation of export/import functionality.

**Constructor**:
```csharp
public ExportService(ILogger<ExportService> logger)
```

**Example Usage**:
```csharp
var exportService = new ExportService(logger);
var tasks = taskService.GetAllTasks();

// Export to different formats
await exportService.ExportToCsvAsync(tasks, "tasks.csv");
await exportService.ExportToMarkdownAsync(tasks, "tasks.md");
await exportService.ExportToJsonAsync(tasks, "backup.json");

// Import from JSON
var importedTasks = await exportService.ImportFromJsonAsync("backup.json");
```

### StatisticsService

Implementation of statistics and analytics.

**Constructor**:
```csharp
public StatisticsService(ILogger<StatisticsService> logger)
```

**Example Usage**:
```csharp
var statisticsService = new StatisticsService(logger);
var tasks = taskService.GetAllTasks();

var stats = statisticsService.GetStatistics(tasks);
Console.WriteLine($"Total: {stats.TotalTasks}");
Console.WriteLine($"Completed: {stats.CompletedTasks}");
Console.WriteLine($"Completion Rate: {stats.CompletionRate:F1}%");

var overdue = statisticsService.GetOverdueTasks(tasks);
foreach (var task in overdue)
{
    Console.WriteLine($"Overdue: {task.Description}");
}
```

## Usage Examples

### Basic Task Management

```csharp
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TaskManager.CLI.Interfaces;
using TaskManager.CLI.Services;

// Setup dependency injection
var services = new ServiceCollection();
services.AddLogging(builder => builder.AddConsole());
services.AddSingleton<ITaskService, TaskService>();

var serviceProvider = services.BuildServiceProvider();
var taskService = serviceProvider.GetRequiredService<ITaskService>();

// Load tasks
await taskService.LoadTasksAsync();

// Add a task
var task = taskService.AddTask(
    "Complete project documentation",
    priority: 5,
    dueDate: DateTime.Now.AddDays(3),
    tags: new List<string> { "work", "documentation" }
);

// List tasks
foreach (var t in taskService.GetAllTasks())
{
    Console.WriteLine(t);
}

// Complete a task
taskService.CompleteTask(task.Id);

// Save changes
await taskService.SaveTasksAsync();
```

### Exporting Tasks

```csharp
var exportService = new ExportService(logger);
var tasks = taskService.GetAllTasks();

// Export to CSV
await exportService.ExportToCsvAsync(tasks, "tasks-backup.csv");

// Export to Markdown report
await exportService.ExportToMarkdownAsync(tasks, "weekly-report.md");

// Export to JSON for backup
await exportService.ExportToJsonAsync(tasks, $"backup-{DateTime.Now:yyyyMMdd}.json");
```

### Statistics and Reporting

```csharp
var statisticsService = new StatisticsService(logger);
var tasks = taskService.GetAllTasks();

// Get overall statistics
var stats = statisticsService.GetStatistics(tasks);
Console.WriteLine($"Total Tasks: {stats.TotalTasks}");
Console.WriteLine($"Completion Rate: {stats.CompletionRate:F1}%");
Console.WriteLine($"Overdue: {stats.OverdueTasks}");

// Get tasks by priority
var byPriority = statisticsService.GetTasksByPriority(tasks);
foreach (var (priority, count) in byPriority.OrderByDescending(kv => kv.Key))
{
    Console.WriteLine($"Priority {priority}: {count} tasks");
}

// Check for overdue tasks
var overdue = statisticsService.GetOverdueTasks(tasks);
if (overdue.Any())
{
    Console.WriteLine("\nOverdue Tasks:");
    foreach (var task in overdue)
    {
        Console.WriteLine($"  - {task.Description} (Due: {task.DueDate:yyyy-MM-dd})");
    }
}
```

## Error Handling

All methods validate inputs and throw appropriate exceptions:

- `ArgumentException`: Invalid parameters (empty description, invalid priority)
- `FileNotFoundException`: File not found during import
- `InvalidOperationException`: JSON deserialization failures
- `IOException`: File system errors during save/load

**Example**:
```csharp
try
{
    var task = taskService.AddTask("", priority: 10); // Will throw ArgumentException
}
catch (ArgumentException ex)
{
    Console.WriteLine($"Invalid input: {ex.Message}");
}

try
{
    await exportService.ImportFromJsonAsync("missing.json"); // Will throw FileNotFoundException
}
catch (FileNotFoundException ex)
{
    Console.WriteLine($"File not found: {ex.Message}");
}
```

## Thread Safety

The current implementation is **not thread-safe**. If you need to use the services from multiple threads, you should:

1. Use separate service instances per thread, or
2. Implement your own synchronization mechanism

Example with locking:
```csharp
private static readonly object _lock = new object();

lock (_lock)
{
    taskService.AddTask("Thread-safe task");
    await taskService.SaveTasksAsync();
}
```

## Performance Considerations

- All file I/O operations are asynchronous
- In-memory operations are O(n) for most queries
- Suitable for up to ~10,000 tasks
- For larger datasets, consider using a database backend

## See Also

- [README.md](../README.md) - Project overview
- [ARCHITECTURE.md](ARCHITECTURE.md) - Architecture documentation
- [CONTRIBUTING.md](../CONTRIBUTING.md) - Contribution guidelines
- [EXAMPLES.md](../examples/EXAMPLES.md) - Usage examples
