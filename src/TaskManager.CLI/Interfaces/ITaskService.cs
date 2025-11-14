using TaskManager.CLI.Models;

namespace TaskManager.CLI.Interfaces;

/// <summary>
/// Interface for task management operations.
/// </summary>
public interface ITaskService
{
    /// <summary>
    /// Loads tasks from storage.
    /// </summary>
    Task LoadTasksAsync();

    /// <summary>
    /// Saves tasks to storage.
    /// </summary>
    Task SaveTasksAsync();

    /// <summary>
    /// Adds a new task with the specified description.
    /// </summary>
    /// <param name="description">The task description.</param>
    /// <param name="priority">The task priority (1-5).</param>
    /// <param name="dueDate">Optional due date.</param>
    /// <param name="tags">Optional tags.</param>
    /// <returns>The created task.</returns>
    TaskItem AddTask(string description, int priority = 3, DateTime? dueDate = null, List<string>? tags = null);

    /// <summary>
    /// Gets all tasks.
    /// </summary>
    /// <param name="includeCompleted">Whether to include completed tasks.</param>
    /// <returns>List of tasks.</returns>
    IEnumerable<TaskItem> GetAllTasks(bool includeCompleted = true);

    /// <summary>
    /// Gets a task by its ID.
    /// </summary>
    /// <param name="id">The task ID.</param>
    /// <returns>The task if found, null otherwise.</returns>
    TaskItem? GetTaskById(int id);

    /// <summary>
    /// Removes a task by its ID.
    /// </summary>
    /// <param name="id">The task ID.</param>
    /// <returns>True if the task was removed, false otherwise.</returns>
    bool RemoveTask(int id);

    /// <summary>
    /// Marks a task as completed.
    /// </summary>
    /// <param name="id">The task ID.</param>
    /// <returns>True if the task was marked as completed, false otherwise.</returns>
    bool CompleteTask(int id);

    /// <summary>
    /// Updates a task's description.
    /// </summary>
    /// <param name="id">The task ID.</param>
    /// <param name="newDescription">The new description.</param>
    /// <returns>True if the task was updated, false otherwise.</returns>
    bool UpdateTask(int id, string newDescription);

    /// <summary>
    /// Updates a task's priority.
    /// </summary>
    /// <param name="id">The task ID.</param>
    /// <param name="priority">The new priority (1-5).</param>
    /// <returns>True if the task was updated, false otherwise.</returns>
    bool UpdateTaskPriority(int id, int priority);

    /// <summary>
    /// Searches tasks by description or tags.
    /// </summary>
    /// <param name="query">The search query.</param>
    /// <returns>List of matching tasks.</returns>
    IEnumerable<TaskItem> SearchTasks(string query);

    /// <summary>
    /// Gets tasks filtered by tag.
    /// </summary>
    /// <param name="tag">The tag to filter by.</param>
    /// <returns>List of tasks with the specified tag.</returns>
    IEnumerable<TaskItem> GetTasksByTag(string tag);

    /// <summary>
    /// Clears all completed tasks.
    /// </summary>
    /// <returns>Number of tasks cleared.</returns>
    int ClearCompletedTasks();
}
