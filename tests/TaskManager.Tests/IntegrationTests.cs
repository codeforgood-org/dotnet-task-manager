using Microsoft.Extensions.Logging;
using Moq;
using TaskManager.CLI.Services;
using Xunit;

namespace TaskManager.Tests;

/// <summary>
/// Integration tests for the TaskManager application.
/// </summary>
public class IntegrationTests : IDisposable
{
    private readonly string _testFileName;
    private readonly Mock<ILogger<TaskService>> _mockLogger;
    private readonly TaskService _taskService;

    public IntegrationTests()
    {
        _testFileName = $"integration_test_{Guid.NewGuid()}.json";
        _mockLogger = new Mock<ILogger<TaskService>>();
        _taskService = new TaskService(_mockLogger.Object, _testFileName);
    }

    public void Dispose()
    {
        if (File.Exists(_testFileName))
        {
            File.Delete(_testFileName);
        }
    }

    [Fact]
    public async Task FullWorkflow_AddListCompleteRemove_WorksCorrectly()
    {
        // Arrange & Act - Load empty tasks
        await _taskService.LoadTasksAsync();

        // Act - Add tasks
        var task1 = _taskService.AddTask("Buy groceries", 4, tags: new List<string> { "shopping", "personal" });
        var task2 = _taskService.AddTask("Write report", 5, dueDate: DateTime.UtcNow.AddDays(2), tags: new List<string> { "work" });
        var task3 = _taskService.AddTask("Call dentist", 3);

        // Assert - Verify tasks were added
        var allTasks = _taskService.GetAllTasks().ToList();
        Assert.Equal(3, allTasks.Count);

        // Act - Save tasks
        await _taskService.SaveTasksAsync();

        // Act - Load tasks in new service instance
        var newService = new TaskService(_mockLogger.Object, _testFileName);
        await newService.LoadTasksAsync();

        // Assert - Verify persistence
        var loadedTasks = newService.GetAllTasks().ToList();
        Assert.Equal(3, loadedTasks.Count);

        // Act - Complete a task
        newService.CompleteTask(task1.Id);

        // Assert - Verify completion
        var completedTask = newService.GetTaskById(task1.Id);
        Assert.NotNull(completedTask);
        Assert.True(completedTask.IsCompleted);

        // Act - Search tasks
        var searchResults = newService.SearchTasks("report").ToList();
        Assert.Single(searchResults);
        Assert.Equal(task2.Id, searchResults[0].Id);

        // Act - Filter by tag
        var workTasks = newService.GetTasksByTag("work").ToList();
        Assert.Single(workTasks);
        Assert.Equal(task2.Id, workTasks[0].Id);

        // Act - Remove a task
        newService.RemoveTask(task3.Id);

        // Assert - Verify removal
        Assert.Equal(2, newService.GetAllTasks().Count());
        Assert.Null(newService.GetTaskById(task3.Id));

        // Act - Save final state
        await newService.SaveTasksAsync();

        // Act - Verify final state with another instance
        var finalService = new TaskService(_mockLogger.Object, _testFileName);
        await finalService.LoadTasksAsync();

        var finalTasks = finalService.GetAllTasks().ToList();
        Assert.Equal(2, finalTasks.Count);
        Assert.True(finalTasks.Any(t => t.Id == task1.Id && t.IsCompleted));
        Assert.True(finalTasks.Any(t => t.Id == task2.Id && !t.IsCompleted));
    }

    [Fact]
    public async Task PriorityOrdering_WorksCorrectly()
    {
        // Arrange
        await _taskService.LoadTasksAsync();

        // Act - Add tasks with different priorities
        _taskService.AddTask("Low priority", 1);
        _taskService.AddTask("High priority", 5);
        _taskService.AddTask("Medium priority", 3);

        // Assert - Verify ordering (pending tasks ordered by priority descending)
        var tasks = _taskService.GetAllTasks(includeCompleted: false).ToList();
        Assert.Equal(5, tasks[0].Priority);
        Assert.Equal(3, tasks[1].Priority);
        Assert.Equal(1, tasks[2].Priority);
    }

    [Fact]
    public async Task TagFiltering_WorksCorrectly()
    {
        // Arrange
        await _taskService.LoadTasksAsync();

        // Act - Add tasks with various tags
        _taskService.AddTask("Task 1", tags: new List<string> { "work", "urgent" });
        _taskService.AddTask("Task 2", tags: new List<string> { "personal" });
        _taskService.AddTask("Task 3", tags: new List<string> { "work", "planning" });
        _taskService.AddTask("Task 4", tags: new List<string> { "urgent", "personal" });

        // Assert - Filter by 'work' tag
        var workTasks = _taskService.GetTasksByTag("work").ToList();
        Assert.Equal(2, workTasks.Count);

        // Assert - Filter by 'urgent' tag
        var urgentTasks = _taskService.GetTasksByTag("urgent").ToList();
        Assert.Equal(2, urgentTasks.Count);

        // Assert - Filter by 'personal' tag
        var personalTasks = _taskService.GetTasksByTag("personal").ToList();
        Assert.Equal(2, personalTasks.Count);
    }

    [Fact]
    public async Task ClearCompleted_RemovesOnlyCompletedTasks()
    {
        // Arrange
        await _taskService.LoadTasksAsync();

        // Act - Add and complete some tasks
        var task1 = _taskService.AddTask("Task 1");
        var task2 = _taskService.AddTask("Task 2");
        var task3 = _taskService.AddTask("Task 3");
        var task4 = _taskService.AddTask("Task 4");

        _taskService.CompleteTask(task1.Id);
        _taskService.CompleteTask(task3.Id);

        // Assert - Before clear
        Assert.Equal(4, _taskService.GetAllTasks().Count());

        // Act - Clear completed
        var clearedCount = _taskService.ClearCompletedTasks();

        // Assert - After clear
        Assert.Equal(2, clearedCount);
        Assert.Equal(2, _taskService.GetAllTasks().Count());
        Assert.NotNull(_taskService.GetTaskById(task2.Id));
        Assert.NotNull(_taskService.GetTaskById(task4.Id));
        Assert.Null(_taskService.GetTaskById(task1.Id));
        Assert.Null(_taskService.GetTaskById(task3.Id));
    }

    [Fact]
    public async Task UpdateOperations_WorkCorrectly()
    {
        // Arrange
        await _taskService.LoadTasksAsync();
        var task = _taskService.AddTask("Original description", 3);

        // Act & Assert - Update description
        var updated = _taskService.UpdateTask(task.Id, "New description");
        Assert.True(updated);
        var updatedTask = _taskService.GetTaskById(task.Id);
        Assert.Equal("New description", updatedTask?.Description);

        // Act & Assert - Update priority
        updated = _taskService.UpdateTaskPriority(task.Id, 5);
        Assert.True(updated);
        updatedTask = _taskService.GetTaskById(task.Id);
        Assert.Equal(5, updatedTask?.Priority);

        // Act & Assert - Save and reload
        await _taskService.SaveTasksAsync();
        var newService = new TaskService(_mockLogger.Object, _testFileName);
        await newService.LoadTasksAsync();

        var reloadedTask = newService.GetTaskById(task.Id);
        Assert.NotNull(reloadedTask);
        Assert.Equal("New description", reloadedTask.Description);
        Assert.Equal(5, reloadedTask.Priority);
    }
}
