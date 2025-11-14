namespace TaskManager.CLI.Models;

/// <summary>
/// Represents a task item with various properties for task management.
/// </summary>
public class TaskItem
{
    /// <summary>
    /// Gets or sets the unique identifier for the task.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the description of the task.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets whether the task is completed.
    /// </summary>
    public bool IsCompleted { get; set; }

    /// <summary>
    /// Gets or sets the priority level of the task (1-5, where 5 is highest).
    /// </summary>
    public int Priority { get; set; } = 3;

    /// <summary>
    /// Gets or sets the date and time when the task was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the due date for the task (optional).
    /// </summary>
    public DateTime? DueDate { get; set; }

    /// <summary>
    /// Gets or sets tags associated with the task.
    /// </summary>
    public List<string> Tags { get; set; } = new();

    /// <summary>
    /// Returns a formatted string representation of the task.
    /// </summary>
    public override string ToString()
    {
        var status = IsCompleted ? "✓" : " ";
        var priorityStr = new string('★', Priority);
        var dueStr = DueDate.HasValue ? $" (Due: {DueDate.Value:yyyy-MM-dd})" : "";
        var tagsStr = Tags.Count > 0 ? $" [{string.Join(", ", Tags)}]" : "";

        return $"[{status}] [{Id}] {Description} {priorityStr}{dueStr}{tagsStr}";
    }
}
