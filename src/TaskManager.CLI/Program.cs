using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TaskManager.CLI.Interfaces;
using TaskManager.CLI.Services;

namespace TaskManager.CLI;

/// <summary>
/// Main program entry point for the Task Manager CLI.
/// </summary>
class Program
{
    static async Task<int> Main(string[] args)
    {
        // Setup dependency injection
        var services = new ServiceCollection();
        ConfigureServices(services);
        var serviceProvider = services.BuildServiceProvider();

        var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
        var taskService = serviceProvider.GetRequiredService<ITaskService>();

        try
        {
            await taskService.LoadTasksAsync();

            if (args.Length == 0)
            {
                ShowUsage();
                return 0;
            }

            var command = args[0].ToLowerInvariant();
            var result = command switch
            {
                "add" => await HandleAddCommand(args, taskService),
                "list" => await HandleListCommand(args, taskService),
                "remove" => await HandleRemoveCommand(args, taskService),
                "complete" => await HandleCompleteCommand(args, taskService),
                "update" => await HandleUpdateCommand(args, taskService),
                "priority" => await HandlePriorityCommand(args, taskService),
                "search" => await HandleSearchCommand(args, taskService),
                "clear" => await HandleClearCommand(taskService),
                "help" => ShowUsage(),
                _ => ShowUnknownCommand(command)
            };

            if (result == 0)
            {
                await taskService.SaveTasksAsync();
            }

            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unexpected error occurred");
            Console.Error.WriteLine($"Error: {ex.Message}");
            return 1;
        }
    }

    private static void ConfigureServices(ServiceCollection services)
    {
        services.AddLogging(builder =>
        {
            builder.AddConsole();
            builder.SetMinimumLevel(LogLevel.Warning); // Only show warnings and errors by default
        });

        services.AddSingleton<ITaskService, TaskService>();
    }

    private static async Task<int> HandleAddCommand(string[] args, ITaskService taskService)
    {
        if (args.Length < 2)
        {
            Console.WriteLine("Error: Please provide a task description.");
            Console.WriteLine("Usage: taskman add <description> [--priority <1-5>] [--due <yyyy-MM-dd>] [--tags <tag1,tag2>]");
            return 1;
        }

        var description = new List<string>();
        int priority = 3;
        DateTime? dueDate = null;
        var tags = new List<string>();

        for (int i = 1; i < args.Length; i++)
        {
            if (args[i] == "--priority" && i + 1 < args.Length)
            {
                if (int.TryParse(args[++i], out var p) && p >= 1 && p <= 5)
                {
                    priority = p;
                }
                else
                {
                    Console.WriteLine("Error: Priority must be between 1 and 5.");
                    return 1;
                }
            }
            else if (args[i] == "--due" && i + 1 < args.Length)
            {
                if (DateTime.TryParse(args[++i], out var d))
                {
                    dueDate = d;
                }
                else
                {
                    Console.WriteLine("Error: Invalid date format. Use yyyy-MM-dd.");
                    return 1;
                }
            }
            else if (args[i] == "--tags" && i + 1 < args.Length)
            {
                tags = args[++i].Split(',').Select(t => t.Trim()).ToList();
            }
            else if (!args[i].StartsWith("--"))
            {
                description.Add(args[i]);
            }
        }

        var descriptionStr = string.Join(" ", description);
        var task = taskService.AddTask(descriptionStr, priority, dueDate, tags);
        Console.WriteLine($"Added task #{task.Id}: {task.Description}");

        return 0;
    }

    private static Task<int> HandleListCommand(string[] args, ITaskService taskService)
    {
        var includeCompleted = true;
        var tag = string.Empty;

        for (int i = 1; i < args.Length; i++)
        {
            if (args[i] == "--pending")
            {
                includeCompleted = false;
            }
            else if (args[i] == "--tag" && i + 1 < args.Length)
            {
                tag = args[++i];
            }
        }

        IEnumerable<Models.TaskItem> tasks;
        if (!string.IsNullOrEmpty(tag))
        {
            tasks = taskService.GetTasksByTag(tag);
        }
        else
        {
            tasks = taskService.GetAllTasks(includeCompleted);
        }

        var taskList = tasks.ToList();
        if (taskList.Count == 0)
        {
            Console.WriteLine("No tasks found.");
            return Task.FromResult(0);
        }

        Console.WriteLine($"\nTotal tasks: {taskList.Count}\n");
        foreach (var task in taskList)
        {
            Console.WriteLine(task.ToString());
        }

        return Task.FromResult(0);
    }

    private static Task<int> HandleRemoveCommand(string[] args, ITaskService taskService)
    {
        if (args.Length < 2 || !int.TryParse(args[1], out int id))
        {
            Console.WriteLine("Error: Please provide a valid task ID.");
            Console.WriteLine("Usage: taskman remove <id>");
            return Task.FromResult(1);
        }

        if (taskService.RemoveTask(id))
        {
            Console.WriteLine($"Removed task #{id}");
            return Task.FromResult(0);
        }

        Console.WriteLine($"Error: Task #{id} not found.");
        return Task.FromResult(1);
    }

    private static Task<int> HandleCompleteCommand(string[] args, ITaskService taskService)
    {
        if (args.Length < 2 || !int.TryParse(args[1], out int id))
        {
            Console.WriteLine("Error: Please provide a valid task ID.");
            Console.WriteLine("Usage: taskman complete <id>");
            return Task.FromResult(1);
        }

        if (taskService.CompleteTask(id))
        {
            Console.WriteLine($"Marked task #{id} as completed");
            return Task.FromResult(0);
        }

        Console.WriteLine($"Error: Task #{id} not found.");
        return Task.FromResult(1);
    }

    private static Task<int> HandleUpdateCommand(string[] args, ITaskService taskService)
    {
        if (args.Length < 3 || !int.TryParse(args[1], out int id))
        {
            Console.WriteLine("Error: Please provide a valid task ID and new description.");
            Console.WriteLine("Usage: taskman update <id> <new description>");
            return Task.FromResult(1);
        }

        var newDescription = string.Join(" ", args[2..]);
        if (taskService.UpdateTask(id, newDescription))
        {
            Console.WriteLine($"Updated task #{id}");
            return Task.FromResult(0);
        }

        Console.WriteLine($"Error: Task #{id} not found.");
        return Task.FromResult(1);
    }

    private static Task<int> HandlePriorityCommand(string[] args, ITaskService taskService)
    {
        if (args.Length < 3 || !int.TryParse(args[1], out int id) || !int.TryParse(args[2], out int priority))
        {
            Console.WriteLine("Error: Please provide a valid task ID and priority (1-5).");
            Console.WriteLine("Usage: taskman priority <id> <1-5>");
            return Task.FromResult(1);
        }

        if (priority < 1 || priority > 5)
        {
            Console.WriteLine("Error: Priority must be between 1 and 5.");
            return Task.FromResult(1);
        }

        if (taskService.UpdateTaskPriority(id, priority))
        {
            Console.WriteLine($"Updated task #{id} priority to {priority}");
            return Task.FromResult(0);
        }

        Console.WriteLine($"Error: Task #{id} not found.");
        return Task.FromResult(1);
    }

    private static Task<int> HandleSearchCommand(string[] args, ITaskService taskService)
    {
        if (args.Length < 2)
        {
            Console.WriteLine("Error: Please provide a search query.");
            Console.WriteLine("Usage: taskman search <query>");
            return Task.FromResult(1);
        }

        var query = string.Join(" ", args[1..]);
        var results = taskService.SearchTasks(query).ToList();

        if (results.Count == 0)
        {
            Console.WriteLine($"No tasks found matching '{query}'");
            return Task.FromResult(0);
        }

        Console.WriteLine($"\nFound {results.Count} task(s) matching '{query}':\n");
        foreach (var task in results)
        {
            Console.WriteLine(task.ToString());
        }

        return Task.FromResult(0);
    }

    private static Task<int> HandleClearCommand(ITaskService taskService)
    {
        var count = taskService.ClearCompletedTasks();
        Console.WriteLine($"Cleared {count} completed task(s)");
        return Task.FromResult(0);
    }

    private static int ShowUsage()
    {
        Console.WriteLine("Task Manager - A modern CLI task management tool\n");
        Console.WriteLine("Usage: taskman <command> [options]\n");
        Console.WriteLine("Commands:");
        Console.WriteLine("  add <description>              Add a new task");
        Console.WriteLine("    Options:");
        Console.WriteLine("      --priority <1-5>           Set priority (default: 3)");
        Console.WriteLine("      --due <yyyy-MM-dd>         Set due date");
        Console.WriteLine("      --tags <tag1,tag2>         Add tags");
        Console.WriteLine();
        Console.WriteLine("  list [options]                 List tasks");
        Console.WriteLine("    Options:");
        Console.WriteLine("      --pending                  Show only pending tasks");
        Console.WriteLine("      --tag <tag>                Filter by tag");
        Console.WriteLine();
        Console.WriteLine("  complete <id>                  Mark a task as completed");
        Console.WriteLine("  remove <id>                    Remove a task");
        Console.WriteLine("  update <id> <description>      Update task description");
        Console.WriteLine("  priority <id> <1-5>            Update task priority");
        Console.WriteLine("  search <query>                 Search tasks by description or tags");
        Console.WriteLine("  clear                          Remove all completed tasks");
        Console.WriteLine("  help                           Show this help message");
        Console.WriteLine();
        Console.WriteLine("Examples:");
        Console.WriteLine("  taskman add \"Buy groceries\" --priority 4 --tags shopping,personal");
        Console.WriteLine("  taskman list --pending");
        Console.WriteLine("  taskman complete 1");
        Console.WriteLine("  taskman search groceries");

        return 0;
    }

    private static int ShowUnknownCommand(string command)
    {
        Console.WriteLine($"Unknown command: {command}");
        Console.WriteLine("Run 'taskman help' for usage information.");
        return 1;
    }
}
