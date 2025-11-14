using TaskManager.CLI.Models;

namespace TaskManager.CLI.Interfaces;

/// <summary>
/// Interface for exporting tasks to various formats.
/// </summary>
public interface IExportService
{
    /// <summary>
    /// Exports tasks to CSV format.
    /// </summary>
    /// <param name="tasks">Tasks to export.</param>
    /// <param name="filePath">Output file path.</param>
    Task ExportToCsvAsync(IEnumerable<TaskItem> tasks, string filePath);

    /// <summary>
    /// Exports tasks to Markdown format.
    /// </summary>
    /// <param name="tasks">Tasks to export.</param>
    /// <param name="filePath">Output file path.</param>
    Task ExportToMarkdownAsync(IEnumerable<TaskItem> tasks, string filePath);

    /// <summary>
    /// Exports tasks to JSON format.
    /// </summary>
    /// <param name="tasks">Tasks to export.</param>
    /// <param name="filePath">Output file path.</param>
    Task ExportToJsonAsync(IEnumerable<TaskItem> tasks, string filePath);

    /// <summary>
    /// Imports tasks from JSON format.
    /// </summary>
    /// <param name="filePath">Input file path.</param>
    /// <returns>Imported tasks.</returns>
    Task<List<TaskItem>> ImportFromJsonAsync(string filePath);
}
