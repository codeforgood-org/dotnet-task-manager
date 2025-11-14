using Microsoft.Extensions.Logging;
using Moq;
using TaskManager.CLI.Models;
using TaskManager.CLI.Services;
using Xunit;

namespace TaskManager.Tests;

public class StatisticsServiceTests
{
    private readonly Mock<ILogger<StatisticsService>> _mockLogger;
    private readonly StatisticsService _statisticsService;

    public StatisticsServiceTests()
    {
        _mockLogger = new Mock<ILogger<StatisticsService>>();
        _statisticsService = new StatisticsService(_mockLogger.Object);
    }

    private List<TaskItem> GetSampleTasks()
    {
        var now = DateTime.UtcNow;
        return new List<TaskItem>
        {
            new TaskItem { Id = 1, Description = "Task 1", Priority = 5, IsCompleted = false, DueDate = now.AddDays(-1) },
            new TaskItem { Id = 2, Description = "Task 2", Priority = 4, IsCompleted = true },
            new TaskItem { Id = 3, Description = "Task 3", Priority = 3, IsCompleted = false, DueDate = now.Date },
            new TaskItem { Id = 4, Description = "Task 4", Priority = 2, IsCompleted = false, DueDate = now.AddDays(5) },
            new TaskItem { Id = 5, Description = "Task 5", Priority = 1, IsCompleted = true },
        };
    }

    [Fact]
    public void GetStatistics_ReturnsCorrectCounts()
    {
        // Arrange
        var tasks = GetSampleTasks();

        // Act
        var stats = _statisticsService.GetStatistics(tasks);

        // Assert
        Assert.Equal(5, stats.TotalTasks);
        Assert.Equal(2, stats.CompletedTasks);
        Assert.Equal(3, stats.PendingTasks);
        Assert.Equal(40.0, stats.CompletionRate, 1);
    }

    [Fact]
    public void GetStatistics_CalculatesOverdueTasksCorrectly()
    {
        // Arrange
        var tasks = GetSampleTasks();

        // Act
        var stats = _statisticsService.GetStatistics(tasks);

        // Assert
        Assert.Equal(1, stats.OverdueTasks); // Task 1 is overdue
    }

    [Fact]
    public void GetStatistics_CalculatesDueTodayCorrectly()
    {
        // Arrange
        var tasks = GetSampleTasks();

        // Act
        var stats = _statisticsService.GetStatistics(tasks);

        // Assert
        Assert.Equal(1, stats.DueToday); // Task 3 is due today
    }

    [Fact]
    public void GetStatistics_CalculatesAveragePriorityCorrectly()
    {
        // Arrange
        var tasks = GetSampleTasks();

        // Act
        var stats = _statisticsService.GetStatistics(tasks);

        // Assert
        // Pending tasks are 1, 3, 4 with priorities 5, 3, 2 = average 3.33
        Assert.Equal(3.33, stats.AveragePriority, 2);
    }

    [Fact]
    public void GetTasksByPriority_GroupsCorrectly()
    {
        // Arrange
        var tasks = GetSampleTasks();

        // Act
        var grouped = _statisticsService.GetTasksByPriority(tasks);

        // Assert
        Assert.Equal(3, grouped.Count); // Only pending tasks with priorities 5, 3, 2
        Assert.Equal(1, grouped[5]);
        Assert.Equal(1, grouped[3]);
        Assert.Equal(1, grouped[2]);
    }

    [Fact]
    public void GetTasksByTag_GroupsCorrectly()
    {
        // Arrange
        var tasks = new List<TaskItem>
        {
            new TaskItem { Id = 1, Tags = new List<string> { "work", "urgent" } },
            new TaskItem { Id = 2, Tags = new List<string> { "personal" } },
            new TaskItem { Id = 3, Tags = new List<string> { "work" } },
            new TaskItem { Id = 4, Tags = new List<string> { "urgent", "important" } },
        };

        // Act
        var grouped = _statisticsService.GetTasksByTag(tasks);

        // Assert
        Assert.Equal(4, grouped.Count);
        Assert.Equal(2, grouped["work"]);
        Assert.Equal(2, grouped["urgent"]);
        Assert.Equal(1, grouped["personal"]);
        Assert.Equal(1, grouped["important"]);
    }

    [Fact]
    public void GetOverdueTasks_ReturnsOnlyOverdueTasks()
    {
        // Arrange
        var tasks = GetSampleTasks();

        // Act
        var overdue = _statisticsService.GetOverdueTasks(tasks).ToList();

        // Assert
        Assert.Single(overdue);
        Assert.Equal(1, overdue[0].Id);
    }

    [Fact]
    public void GetUpcomingTasks_ReturnsTasksDueWithinDays()
    {
        // Arrange
        var tasks = GetSampleTasks();

        // Act
        var upcoming = _statisticsService.GetUpcomingTasks(tasks, days: 7).ToList();

        // Assert
        Assert.Equal(2, upcoming.Count); // Tasks 3 and 4
        Assert.Contains(upcoming, t => t.Id == 3); // Due today
        Assert.Contains(upcoming, t => t.Id == 4); // Due in 5 days
    }

    [Fact]
    public void GetStatistics_EmptyTaskList_ReturnsZeros()
    {
        // Arrange
        var tasks = new List<TaskItem>();

        // Act
        var stats = _statisticsService.GetStatistics(tasks);

        // Assert
        Assert.Equal(0, stats.TotalTasks);
        Assert.Equal(0, stats.CompletedTasks);
        Assert.Equal(0, stats.PendingTasks);
        Assert.Equal(0, stats.CompletionRate);
    }
}
