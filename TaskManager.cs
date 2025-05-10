using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

class TaskItem
{
    public int Id { get; set; }
    public string Description { get; set; }
}

class TaskManager
{
    private const string FileName = "tasks.json";
    private static List<TaskItem> tasks = new();

    static void Main(string[] args)
    {
        LoadTasks();

        if (args.Length == 0)
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("  add <description>");
            Console.WriteLine("  list");
            Console.WriteLine("  remove <id>");
            return;
        }

        var command = args[0];

        switch (command)
        {
            case "add":
                AddTask(string.Join(" ", args[1..]));
                break;
            case "list":
                ListTasks();
                break;
            case "remove":
                if (args.Length < 2 || !int.TryParse(args[1], out int id))
                {
                    Console.WriteLine("Please provide a valid task ID.");
                }
                else
                {
                    RemoveTask(id);
                }
                break;
            default:
                Console.WriteLine("Unknown command.");
                break;
        }

        SaveTasks();
    }

    static void LoadTasks()
    {
        if (File.Exists(FileName))
        {
            string json = File.ReadAllText(FileName);
            tasks = JsonSerializer.Deserialize<List<TaskItem>>(json) ?? new();
        }
    }

    static void SaveTasks()
    {
        string json = JsonSerializer.Serialize(tasks, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(FileName, json);
    }

    static void AddTask(string description)
    {
        int nextId = tasks.Count == 0 ? 1 : tasks[^1].Id + 1;
        tasks.Add(new TaskItem { Id = nextId, Description = description });
        Console.WriteLine($"Added task #{nextId}: {description}");
    }

    static void ListTasks()
    {
        if (tasks.Count == 0)
        {
            Console.WriteLine("No tasks found.");
            return;
        }

        foreach (var task in tasks)
        {
            Console.WriteLine($"[{task.Id}] {task.Description}");
        }
    }

    static void RemoveTask(int id)
    {
        var task = tasks.Find(t => t.Id == id);
        if (task == null)
        {
            Console.WriteLine("Task not found.");
            return;
        }

        tasks.Remove(task);
        Console.WriteLine($"Removed task #{id}");
    }
}
