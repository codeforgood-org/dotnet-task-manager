using Microsoft.Extensions.Logging;
using TaskManager.CLI.Interfaces;
using TaskManager.CLI.Models;

namespace TaskManager.CLI.Services;

/// <summary>
/// Service for task statistics and reporting.
/// </summary>
public class StatisticsService : IStatisticsService
{
    private readonly ILogger<StatisticsService> _logger;

    public StatisticsService(ILogger<StatisticsService> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public TaskStatistics GetStatistics(IEnumerable<TaskItem> tasks)
    {
        var taskList = tasks.ToList();
        var now = DateTime.UtcNow;
        var today = now.Date;
        var endOfWeek = today.AddDays(7);

        var pendingTasks = taskList.Where(t => !t.IsCompleted).ToList();
        var overdueTasks = pendingTasks.Where(t => t.DueDate.HasValue && t.DueDate.Value.Date < today).ToList();

        return new TaskStatistics
        {
            TotalTasks = taskList.Count,
            CompletedTasks = taskList.Count(t => t.IsCompleted),
            PendingTasks = pendingTasks.Count,
            OverdueTasks = overdueTasks.Count,
            DueToday = pendingTasks.Count(t => t.DueDate.HasValue && t.DueDate.Value.Date == today),
            DueThisWeek = pendingTasks.Count(t => t.DueDate.HasValue && t.DueDate.Value.Date >= today && t.DueDate.Value.Date <= endOfWeek),
            AveragePriority = pendingTasks.Any() ? pendingTasks.Average(t => t.Priority) : 0,
            TotalTags = taskList.SelectMany(t => t.Tags).Distinct().Count()
        };
    }

    public Dictionary<int, int> GetTasksByPriority(IEnumerable<TaskItem> tasks)
    {
        return tasks
            .Where(t => !t.IsCompleted)
            .GroupBy(t => t.Priority)
            .OrderByDescending(g => g.Key)
            .ToDictionary(g => g.Key, g => g.Count());
    }

    public Dictionary<string, int> GetTasksByTag(IEnumerable<TaskItem> tasks)
    {
        return tasks
            .SelectMany(t => t.Tags)
            .GroupBy(tag => tag, StringComparer.OrdinalIgnoreCase)
            .OrderByDescending(g => g.Count())
            .ToDictionary(g => g.Key, g => g.Count());
    }

    public IEnumerable<TaskItem> GetOverdueTasks(IEnumerable<TaskItem> tasks)
    {
        var today = DateTime.UtcNow.Date;
        return tasks
            .Where(t => !t.IsCompleted && t.DueDate.HasValue && t.DueDate.Value.Date < today)
            .OrderBy(t => t.DueDate);
    }

    public IEnumerable<TaskItem> GetUpcomingTasks(IEnumerable<TaskItem> tasks, int days = 7)
    {
        var today = DateTime.UtcNow.Date;
        var endDate = today.AddDays(days);

        return tasks
            .Where(t => !t.IsCompleted && t.DueDate.HasValue && t.DueDate.Value.Date >= today && t.DueDate.Value.Date <= endDate)
            .OrderBy(t => t.DueDate);
    }
}
