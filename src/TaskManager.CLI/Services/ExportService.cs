using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using TaskManager.CLI.Interfaces;
using TaskManager.CLI.Models;

namespace TaskManager.CLI.Services;

/// <summary>
/// Service for exporting tasks to various formats.
/// </summary>
public class ExportService : IExportService
{
    private readonly ILogger<ExportService> _logger;
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true
    };

    public ExportService(ILogger<ExportService> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task ExportToCsvAsync(IEnumerable<TaskItem> tasks, string filePath)
    {
        try
        {
            var csv = new StringBuilder();
            csv.AppendLine("Id,Description,IsCompleted,Priority,CreatedAt,DueDate,Tags");

            foreach (var task in tasks)
            {
                var tags = string.Join("|", task.Tags);
                var dueDate = task.DueDate?.ToString("yyyy-MM-dd") ?? "";
                var completed = task.IsCompleted ? "Yes" : "No";

                csv.AppendLine($"{task.Id},\"{task.Description.Replace("\"", "\"\"")}\",{completed},{task.Priority},{task.CreatedAt:yyyy-MM-dd HH:mm:ss},{dueDate},\"{tags}\"");
            }

            await File.WriteAllTextAsync(filePath, csv.ToString());
            _logger.LogInformation("Exported {Count} tasks to CSV: {FilePath}", tasks.Count(), filePath);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exporting to CSV: {FilePath}", filePath);
            throw;
        }
    }

    public async Task ExportToMarkdownAsync(IEnumerable<TaskItem> tasks, string filePath)
    {
        try
        {
            var md = new StringBuilder();
            md.AppendLine("# Task List");
            md.AppendLine();
            md.AppendLine($"*Exported on {DateTime.Now:yyyy-MM-dd HH:mm:ss}*");
            md.AppendLine();

            var pendingTasks = tasks.Where(t => !t.IsCompleted).OrderByDescending(t => t.Priority).ToList();
            var completedTasks = tasks.Where(t => t.IsCompleted).OrderBy(t => t.Id).ToList();

            if (pendingTasks.Any())
            {
                md.AppendLine("## Pending Tasks");
                md.AppendLine();
                foreach (var task in pendingTasks)
                {
                    var priority = new string('★', task.Priority);
                    var tags = task.Tags.Any() ? $" `{string.Join("` `", task.Tags)}`" : "";
                    var due = task.DueDate.HasValue ? $" 📅 {task.DueDate.Value:yyyy-MM-dd}" : "";
                    md.AppendLine($"- [ ] **#{task.Id}** {task.Description} {priority}{due}{tags}");
                }
                md.AppendLine();
            }

            if (completedTasks.Any())
            {
                md.AppendLine("## Completed Tasks");
                md.AppendLine();
                foreach (var task in completedTasks)
                {
                    var tags = task.Tags.Any() ? $" `{string.Join("` `", task.Tags)}`" : "";
                    md.AppendLine($"- [x] **#{task.Id}** {task.Description}{tags}");
                }
                md.AppendLine();
            }

            md.AppendLine("---");
            md.AppendLine($"*Total: {tasks.Count()} tasks ({pendingTasks.Count} pending, {completedTasks.Count} completed)*");

            await File.WriteAllTextAsync(filePath, md.ToString());
            _logger.LogInformation("Exported {Count} tasks to Markdown: {FilePath}", tasks.Count(), filePath);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exporting to Markdown: {FilePath}", filePath);
            throw;
        }
    }

    public async Task ExportToJsonAsync(IEnumerable<TaskItem> tasks, string filePath)
    {
        try
        {
            var json = JsonSerializer.Serialize(tasks, _jsonOptions);
            await File.WriteAllTextAsync(filePath, json);
            _logger.LogInformation("Exported {Count} tasks to JSON: {FilePath}", tasks.Count(), filePath);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exporting to JSON: {FilePath}", filePath);
            throw;
        }
    }

    public async Task<List<TaskItem>> ImportFromJsonAsync(string filePath)
    {
        try
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"Import file not found: {filePath}");
            }

            var json = await File.ReadAllTextAsync(filePath);
            var tasks = JsonSerializer.Deserialize<List<TaskItem>>(json, _jsonOptions);

            if (tasks == null)
            {
                throw new InvalidOperationException("Failed to deserialize tasks from JSON");
            }

            _logger.LogInformation("Imported {Count} tasks from JSON: {FilePath}", tasks.Count, filePath);
            return tasks;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error importing from JSON: {FilePath}", filePath);
            throw;
        }
    }
}
