using Microsoft.Extensions.Logging;
using Moq;
using TaskManager.CLI.Models;
using TaskManager.CLI.Services;
using Xunit;

namespace TaskManager.Tests;

public class TaskServiceTests
{
    private readonly Mock<ILogger<TaskService>> _mockLogger;
    private readonly string _testFileName;

    public TaskServiceTests()
    {
        _mockLogger = new Mock<ILogger<TaskService>>();
        _testFileName = $"test_tasks_{Guid.NewGuid()}.json";
    }

    private TaskService CreateService()
    {
        return new TaskService(_mockLogger.Object, _testFileName);
    }

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
        Assert.Equal(1, task.Id);
        Assert.Equal(description, task.Description);
        Assert.False(task.IsCompleted);
        Assert.Equal(3, task.Priority); // Default priority
    }

    [Fact]
    public void AddTask_WithPriority_SetsPriorityCorrectly()
    {
        // Arrange
        var service = CreateService();
        var description = "High priority task";
        var priority = 5;

        // Act
        var task = service.AddTask(description, priority);

        // Assert
        Assert.Equal(priority, task.Priority);
    }

    [Fact]
    public void AddTask_WithTags_SetsTagsCorrectly()
    {
        // Arrange
        var service = CreateService();
        var description = "Tagged task";
        var tags = new List<string> { "work", "urgent" };

        // Act
        var task = service.AddTask(description, tags: tags);

        // Assert
        Assert.Equal(2, task.Tags.Count);
        Assert.Contains("work", task.Tags);
        Assert.Contains("urgent", task.Tags);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void AddTask_WithInvalidDescription_ThrowsException(string invalidDescription)
    {
        // Arrange
        var service = CreateService();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => service.AddTask(invalidDescription));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(6)]
    [InlineData(-1)]
    public void AddTask_WithInvalidPriority_ThrowsException(int invalidPriority)
    {
        // Arrange
        var service = CreateService();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => service.AddTask("Test", invalidPriority));
    }

    [Fact]
    public void AddTask_MultipleTasksIncrementId()
    {
        // Arrange
        var service = CreateService();

        // Act
        var task1 = service.AddTask("Task 1");
        var task2 = service.AddTask("Task 2");
        var task3 = service.AddTask("Task 3");

        // Assert
        Assert.Equal(1, task1.Id);
        Assert.Equal(2, task2.Id);
        Assert.Equal(3, task3.Id);
    }

    [Fact]
    public void GetAllTasks_WithNoTasks_ReturnsEmptyList()
    {
        // Arrange
        var service = CreateService();

        // Act
        var tasks = service.GetAllTasks();

        // Assert
        Assert.Empty(tasks);
    }

    [Fact]
    public void GetAllTasks_WithTasks_ReturnsAllTasks()
    {
        // Arrange
        var service = CreateService();
        service.AddTask("Task 1");
        service.AddTask("Task 2");
        service.AddTask("Task 3");

        // Act
        var tasks = service.GetAllTasks().ToList();

        // Assert
        Assert.Equal(3, tasks.Count);
    }

    [Fact]
    public void GetAllTasks_ExcludingCompleted_ReturnsOnlyPendingTasks()
    {
        // Arrange
        var service = CreateService();
        service.AddTask("Task 1");
        var task2 = service.AddTask("Task 2");
        service.AddTask("Task 3");
        service.CompleteTask(task2.Id);

        // Act
        var tasks = service.GetAllTasks(includeCompleted: false).ToList();

        // Assert
        Assert.Equal(2, tasks.Count);
        Assert.All(tasks, t => Assert.False(t.IsCompleted));
    }

    [Fact]
    public void GetTaskById_WithValidId_ReturnsTask()
    {
        // Arrange
        var service = CreateService();
        var addedTask = service.AddTask("Test task");

        // Act
        var task = service.GetTaskById(addedTask.Id);

        // Assert
        Assert.NotNull(task);
        Assert.Equal(addedTask.Id, task.Id);
        Assert.Equal(addedTask.Description, task.Description);
    }

    [Fact]
    public void GetTaskById_WithInvalidId_ReturnsNull()
    {
        // Arrange
        var service = CreateService();

        // Act
        var task = service.GetTaskById(999);

        // Assert
        Assert.Null(task);
    }

    [Fact]
    public void RemoveTask_WithValidId_RemovesTask()
    {
        // Arrange
        var service = CreateService();
        var task = service.AddTask("Test task");

        // Act
        var result = service.RemoveTask(task.Id);

        // Assert
        Assert.True(result);
        Assert.Null(service.GetTaskById(task.Id));
    }

    [Fact]
    public void RemoveTask_WithInvalidId_ReturnsFalse()
    {
        // Arrange
        var service = CreateService();

        // Act
        var result = service.RemoveTask(999);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CompleteTask_WithValidId_MarksTaskAsCompleted()
    {
        // Arrange
        var service = CreateService();
        var task = service.AddTask("Test task");

        // Act
        var result = service.CompleteTask(task.Id);

        // Assert
        Assert.True(result);
        var completedTask = service.GetTaskById(task.Id);
        Assert.NotNull(completedTask);
        Assert.True(completedTask.IsCompleted);
    }

    [Fact]
    public void CompleteTask_WithInvalidId_ReturnsFalse()
    {
        // Arrange
        var service = CreateService();

        // Act
        var result = service.CompleteTask(999);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void UpdateTask_WithValidIdAndDescription_UpdatesTask()
    {
        // Arrange
        var service = CreateService();
        var task = service.AddTask("Original description");
        var newDescription = "Updated description";

        // Act
        var result = service.UpdateTask(task.Id, newDescription);

        // Assert
        Assert.True(result);
        var updatedTask = service.GetTaskById(task.Id);
        Assert.Equal(newDescription, updatedTask?.Description);
    }

    [Fact]
    public void UpdateTask_WithInvalidId_ReturnsFalse()
    {
        // Arrange
        var service = CreateService();

        // Act
        var result = service.UpdateTask(999, "New description");

        // Assert
        Assert.False(result);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void UpdateTask_WithInvalidDescription_ThrowsException(string invalidDescription)
    {
        // Arrange
        var service = CreateService();
        var task = service.AddTask("Test task");

        // Act & Assert
        Assert.Throws<ArgumentException>(() => service.UpdateTask(task.Id, invalidDescription));
    }

    [Fact]
    public void UpdateTaskPriority_WithValidIdAndPriority_UpdatesPriority()
    {
        // Arrange
        var service = CreateService();
        var task = service.AddTask("Test task");
        var newPriority = 5;

        // Act
        var result = service.UpdateTaskPriority(task.Id, newPriority);

        // Assert
        Assert.True(result);
        var updatedTask = service.GetTaskById(task.Id);
        Assert.Equal(newPriority, updatedTask?.Priority);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(6)]
    [InlineData(-1)]
    public void UpdateTaskPriority_WithInvalidPriority_ThrowsException(int invalidPriority)
    {
        // Arrange
        var service = CreateService();
        var task = service.AddTask("Test task");

        // Act & Assert
        Assert.Throws<ArgumentException>(() => service.UpdateTaskPriority(task.Id, invalidPriority));
    }

    [Fact]
    public void SearchTasks_WithMatchingQuery_ReturnsMatchingTasks()
    {
        // Arrange
        var service = CreateService();
        service.AddTask("Buy groceries");
        service.AddTask("Buy books");
        service.AddTask("Read books");

        // Act
        var results = service.SearchTasks("books").ToList();

        // Assert
        Assert.Equal(2, results.Count);
        Assert.All(results, t => Assert.Contains("books", t.Description, StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void SearchTasks_WithNoMatches_ReturnsEmptyList()
    {
        // Arrange
        var service = CreateService();
        service.AddTask("Task 1");
        service.AddTask("Task 2");

        // Act
        var results = service.SearchTasks("nonexistent");

        // Assert
        Assert.Empty(results);
    }

    [Fact]
    public void SearchTasks_SearchesTags()
    {
        // Arrange
        var service = CreateService();
        service.AddTask("Task 1", tags: new List<string> { "work", "urgent" });
        service.AddTask("Task 2", tags: new List<string> { "personal" });
        service.AddTask("Task 3", tags: new List<string> { "work" });

        // Act
        var results = service.SearchTasks("work").ToList();

        // Assert
        Assert.Equal(2, results.Count);
    }

    [Fact]
    public void GetTasksByTag_WithMatchingTag_ReturnsMatchingTasks()
    {
        // Arrange
        var service = CreateService();
        service.AddTask("Task 1", tags: new List<string> { "work" });
        service.AddTask("Task 2", tags: new List<string> { "personal" });
        service.AddTask("Task 3", tags: new List<string> { "work", "urgent" });

        // Act
        var results = service.GetTasksByTag("work").ToList();

        // Assert
        Assert.Equal(2, results.Count);
        Assert.All(results, t => Assert.Contains("work", t.Tags, StringComparer.OrdinalIgnoreCase));
    }

    [Fact]
    public void ClearCompletedTasks_RemovesOnlyCompletedTasks()
    {
        // Arrange
        var service = CreateService();
        var task1 = service.AddTask("Task 1");
        var task2 = service.AddTask("Task 2");
        var task3 = service.AddTask("Task 3");
        service.CompleteTask(task1.Id);
        service.CompleteTask(task3.Id);

        // Act
        var count = service.ClearCompletedTasks();

        // Assert
        Assert.Equal(2, count);
        Assert.Single(service.GetAllTasks());
        Assert.NotNull(service.GetTaskById(task2.Id));
    }

    [Fact]
    public async Task SaveAndLoadTasks_PersistsTasksCorrectly()
    {
        // Arrange
        var service1 = CreateService();
        var task1 = service1.AddTask("Task 1", 5, tags: new List<string> { "work" });
        var task2 = service1.AddTask("Task 2", 3);
        service1.CompleteTask(task1.Id);

        // Act
        await service1.SaveTasksAsync();
        var service2 = CreateService();
        await service2.LoadTasksAsync();
        var loadedTasks = service2.GetAllTasks().ToList();

        // Assert
        Assert.Equal(2, loadedTasks.Count);
        var loadedTask1 = service2.GetTaskById(task1.Id);
        Assert.NotNull(loadedTask1);
        Assert.True(loadedTask1.IsCompleted);
        Assert.Equal(5, loadedTask1.Priority);
        Assert.Contains("work", loadedTask1.Tags);

        // Cleanup
        if (File.Exists(_testFileName))
        {
            File.Delete(_testFileName);
        }
    }

    [Fact]
    public async Task LoadTasks_WithNonExistentFile_CreatesEmptyList()
    {
        // Arrange
        var service = CreateService();

        // Act
        await service.LoadTasksAsync();
        var tasks = service.GetAllTasks();

        // Assert
        Assert.Empty(tasks);
    }
}
