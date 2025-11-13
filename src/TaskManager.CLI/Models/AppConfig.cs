namespace TaskManager.CLI.Models;

/// <summary>
/// Application configuration settings.
/// </summary>
public class AppConfig
{
    /// <summary>
    /// Path to the tasks file.
    /// </summary>
    public string TasksFilePath { get; set; } = "tasks.json";

    /// <summary>
    /// Default priority for new tasks.
    /// </summary>
    public int DefaultPriority { get; set; } = 3;

    /// <summary>
    /// Whether to use colored output.
    /// </summary>
    public bool UseColors { get; set; } = true;

    /// <summary>
    /// Date format for displaying dates.
    /// </summary>
    public string DateFormat { get; set; } = "yyyy-MM-dd";

    /// <summary>
    /// Whether to show completed tasks by default.
    /// </summary>
    public bool ShowCompletedByDefault { get; set; } = true;

    /// <summary>
    /// Number of days to look ahead for upcoming tasks.
    /// </summary>
    public int UpcomingDaysThreshold { get; set; } = 7;

    /// <summary>
    /// Export directory for exported files.
    /// </summary>
    public string ExportDirectory { get; set; } = "exports";
}
