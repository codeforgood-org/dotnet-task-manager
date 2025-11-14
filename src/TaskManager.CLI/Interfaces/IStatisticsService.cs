using TaskManager.CLI.Models;

namespace TaskManager.CLI.Interfaces;

/// <summary>
/// Interface for task statistics and reporting.
/// </summary>
public interface IStatisticsService
{
    /// <summary>
    /// Gets overall task statistics.
    /// </summary>
    /// <param name="tasks">Tasks to analyze.</param>
    /// <returns>Statistics summary.</returns>
    TaskStatistics GetStatistics(IEnumerable<TaskItem> tasks);

    /// <summary>
    /// Gets tasks grouped by priority.
    /// </summary>
    /// <param name="tasks">Tasks to group.</param>
    /// <returns>Dictionary of priority to task count.</returns>
    Dictionary<int, int> GetTasksByPriority(IEnumerable<TaskItem> tasks);

    /// <summary>
    /// Gets tasks grouped by tag.
    /// </summary>
    /// <param name="tasks">Tasks to group.</param>
    /// <returns>Dictionary of tag to task count.</returns>
    Dictionary<string, int> GetTasksByTag(IEnumerable<TaskItem> tasks);

    /// <summary>
    /// Gets overdue tasks.
    /// </summary>
    /// <param name="tasks">Tasks to check.</param>
    /// <returns>Overdue tasks.</returns>
    IEnumerable<TaskItem> GetOverdueTasks(IEnumerable<TaskItem> tasks);

    /// <summary>
    /// Gets upcoming tasks due within specified days.
    /// </summary>
    /// <param name="tasks">Tasks to check.</param>
    /// <param name="days">Number of days to look ahead.</param>
    /// <returns>Upcoming tasks.</returns>
    IEnumerable<TaskItem> GetUpcomingTasks(IEnumerable<TaskItem> tasks, int days = 7);
}
