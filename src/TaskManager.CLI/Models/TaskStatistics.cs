namespace TaskManager.CLI.Models;

/// <summary>
/// Statistics about tasks.
/// </summary>
public class TaskStatistics
{
    /// <summary>
    /// Total number of tasks.
    /// </summary>
    public int TotalTasks { get; set; }

    /// <summary>
    /// Number of completed tasks.
    /// </summary>
    public int CompletedTasks { get; set; }

    /// <summary>
    /// Number of pending tasks.
    /// </summary>
    public int PendingTasks { get; set; }

    /// <summary>
    /// Number of overdue tasks.
    /// </summary>
    public int OverdueTasks { get; set; }

    /// <summary>
    /// Number of tasks due today.
    /// </summary>
    public int DueToday { get; set; }

    /// <summary>
    /// Number of tasks due this week.
    /// </summary>
    public int DueThisWeek { get; set; }

    /// <summary>
    /// Completion percentage.
    /// </summary>
    public double CompletionRate => TotalTasks > 0 ? (CompletedTasks * 100.0 / TotalTasks) : 0;

    /// <summary>
    /// Average priority of pending tasks.
    /// </summary>
    public double AveragePriority { get; set; }

    /// <summary>
    /// Total number of unique tags.
    /// </summary>
    public int TotalTags { get; set; }
}
