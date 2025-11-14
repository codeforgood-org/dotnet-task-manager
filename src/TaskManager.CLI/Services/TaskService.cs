using System.Text.Json;
using Microsoft.Extensions.Logging;
using TaskManager.CLI.Interfaces;
using TaskManager.CLI.Models;

namespace TaskManager.CLI.Services;

/// <summary>
/// Service for managing tasks with file-based persistence.
/// </summary>
public class TaskService : ITaskService
{
    private const string DefaultFileName = "tasks.json";
    private readonly string _fileName;
    private readonly ILogger<TaskService> _logger;
    private List<TaskItem> _tasks = new();
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true
    };

    public TaskService(ILogger<TaskService> logger, string? fileName = null)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _fileName = fileName ?? DefaultFileName;
    }

    public async Task LoadTasksAsync()
    {
        try
        {
            if (File.Exists(_fileName))
            {
                var json = await File.ReadAllTextAsync(_fileName);
                _tasks = JsonSerializer.Deserialize<List<TaskItem>>(json, _jsonOptions) ?? new();
                _logger.LogInformation("Loaded {Count} tasks from {FileName}", _tasks.Count, _fileName);
            }
            else
            {
                _tasks = new();
                _logger.LogInformation("No existing task file found. Starting with empty task list.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading tasks from {FileName}", _fileName);
            _tasks = new();
            throw;
        }
    }

    public async Task SaveTasksAsync()
    {
        try
        {
            var json = JsonSerializer.Serialize(_tasks, _jsonOptions);
            await File.WriteAllTextAsync(_fileName, json);
            _logger.LogInformation("Saved {Count} tasks to {FileName}", _tasks.Count, _fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving tasks to {FileName}", _fileName);
            throw;
        }
    }

    public TaskItem AddTask(string description, int priority = 3, DateTime? dueDate = null, List<string>? tags = null)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            throw new ArgumentException("Task description cannot be empty.", nameof(description));
        }

        if (priority < 1 || priority > 5)
        {
            throw new ArgumentException("Priority must be between 1 and 5.", nameof(priority));
        }

        var nextId = _tasks.Count == 0 ? 1 : _tasks.Max(t => t.Id) + 1;
        var task = new TaskItem
        {
            Id = nextId,
            Description = description,
            Priority = priority,
            DueDate = dueDate,
            Tags = tags ?? new List<string>(),
            CreatedAt = DateTime.UtcNow
        };

        _tasks.Add(task);
        _logger.LogInformation("Added task #{Id}: {Description}", task.Id, task.Description);
        return task;
    }

    public IEnumerable<TaskItem> GetAllTasks(bool includeCompleted = true)
    {
        return includeCompleted
            ? _tasks.OrderBy(t => t.IsCompleted).ThenByDescending(t => t.Priority).ThenBy(t => t.CreatedAt)
            : _tasks.Where(t => !t.IsCompleted).OrderByDescending(t => t.Priority).ThenBy(t => t.CreatedAt);
    }

    public TaskItem? GetTaskById(int id)
    {
        return _tasks.FirstOrDefault(t => t.Id == id);
    }

    public bool RemoveTask(int id)
    {
        var task = GetTaskById(id);
        if (task == null)
        {
            _logger.LogWarning("Task #{Id} not found for removal", id);
            return false;
        }

        _tasks.Remove(task);
        _logger.LogInformation("Removed task #{Id}", id);
        return true;
    }

    public bool CompleteTask(int id)
    {
        var task = GetTaskById(id);
        if (task == null)
        {
            _logger.LogWarning("Task #{Id} not found for completion", id);
            return false;
        }

        task.IsCompleted = true;
        _logger.LogInformation("Completed task #{Id}: {Description}", task.Id, task.Description);
        return true;
    }

    public bool UpdateTask(int id, string newDescription)
    {
        if (string.IsNullOrWhiteSpace(newDescription))
        {
            throw new ArgumentException("Task description cannot be empty.", nameof(newDescription));
        }

        var task = GetTaskById(id);
        if (task == null)
        {
            _logger.LogWarning("Task #{Id} not found for update", id);
            return false;
        }

        var oldDescription = task.Description;
        task.Description = newDescription;
        _logger.LogInformation("Updated task #{Id} from '{OldDescription}' to '{NewDescription}'",
            task.Id, oldDescription, newDescription);
        return true;
    }

    public bool UpdateTaskPriority(int id, int priority)
    {
        if (priority < 1 || priority > 5)
        {
            throw new ArgumentException("Priority must be between 1 and 5.", nameof(priority));
        }

        var task = GetTaskById(id);
        if (task == null)
        {
            _logger.LogWarning("Task #{Id} not found for priority update", id);
            return false;
        }

        task.Priority = priority;
        _logger.LogInformation("Updated task #{Id} priority to {Priority}", task.Id, priority);
        return true;
    }

    public IEnumerable<TaskItem> SearchTasks(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return Enumerable.Empty<TaskItem>();
        }

        var lowerQuery = query.ToLowerInvariant();
        return _tasks.Where(t =>
            t.Description.Contains(lowerQuery, StringComparison.OrdinalIgnoreCase) ||
            t.Tags.Any(tag => tag.Contains(lowerQuery, StringComparison.OrdinalIgnoreCase))
        );
    }

    public IEnumerable<TaskItem> GetTasksByTag(string tag)
    {
        if (string.IsNullOrWhiteSpace(tag))
        {
            return Enumerable.Empty<TaskItem>();
        }

        return _tasks.Where(t => t.Tags.Contains(tag, StringComparer.OrdinalIgnoreCase));
    }

    public int ClearCompletedTasks()
    {
        var completedTasks = _tasks.Where(t => t.IsCompleted).ToList();
        var count = completedTasks.Count;

        foreach (var task in completedTasks)
        {
            _tasks.Remove(task);
        }

        _logger.LogInformation("Cleared {Count} completed tasks", count);
        return count;
    }
}
