using Microsoft.Extensions.Logging;
using Moq;
using TaskManager.CLI.Models;
using TaskManager.CLI.Services;
using Xunit;

namespace TaskManager.Tests;

public class ExportServiceTests : IDisposable
{
    private readonly Mock<ILogger<ExportService>> _mockLogger;
    private readonly ExportService _exportService;
    private readonly List<string> _testFiles = new();

    public ExportServiceTests()
    {
        _mockLogger = new Mock<ILogger<ExportService>>();
        _exportService = new ExportService(_mockLogger.Object);
    }

    public void Dispose()
    {
        foreach (var file in _testFiles.Where(File.Exists))
        {
            File.Delete(file);
        }
    }

    private string GetTestFilePath(string extension)
    {
        var path = $"test_export_{Guid.NewGuid()}.{extension}";
        _testFiles.Add(path);
        return path;
    }

    private List<TaskItem> GetSampleTasks()
    {
        return new List<TaskItem>
        {
            new TaskItem
            {
                Id = 1,
                Description = "Buy groceries",
                Priority = 4,
                Tags = new List<string> { "shopping", "personal" },
                CreatedAt = new DateTime(2024, 1, 1, 10, 0, 0, DateTimeKind.Utc)
            },
            new TaskItem
            {
                Id = 2,
                Description = "Write report",
                Priority = 5,
                IsCompleted = true,
                Tags = new List<string> { "work" },
                CreatedAt = new DateTime(2024, 1, 2, 14, 30, 0, DateTimeKind.Utc),
                DueDate = new DateTime(2024, 1, 15)
            },
            new TaskItem
            {
                Id = 3,
                Description = "Call dentist",
                Priority = 3,
                Tags = new List<string>(),
                CreatedAt = new DateTime(2024, 1, 3, 9, 15, 0, DateTimeKind.Utc)
            }
        };
    }

    [Fact]
    public async Task ExportToCsv_CreatesValidCsvFile()
    {
        // Arrange
        var tasks = GetSampleTasks();
        var filePath = GetTestFilePath("csv");

        // Act
        await _exportService.ExportToCsvAsync(tasks, filePath);

        // Assert
        Assert.True(File.Exists(filePath));
        var content = await File.ReadAllTextAsync(filePath);
        Assert.Contains("Id,Description,IsCompleted,Priority,CreatedAt,DueDate,Tags", content);
        Assert.Contains("Buy groceries", content);
        Assert.Contains("Write report", content);
        Assert.Contains("Call dentist", content);
    }

    [Fact]
    public async Task ExportToMarkdown_CreatesValidMarkdownFile()
    {
        // Arrange
        var tasks = GetSampleTasks();
        var filePath = GetTestFilePath("md");

        // Act
        await _exportService.ExportToMarkdownAsync(tasks, filePath);

        // Assert
        Assert.True(File.Exists(filePath));
        var content = await File.ReadAllTextAsync(filePath);
        Assert.Contains("# Task List", content);
        Assert.Contains("## Pending Tasks", content);
        Assert.Contains("## Completed Tasks", content);
        Assert.Contains("Buy groceries", content);
        Assert.Contains("Write report", content);
    }

    [Fact]
    public async Task ExportToJson_CreatesValidJsonFile()
    {
        // Arrange
        var tasks = GetSampleTasks();
        var filePath = GetTestFilePath("json");

        // Act
        await _exportService.ExportToJsonAsync(tasks, filePath);

        // Assert
        Assert.True(File.Exists(filePath));
        var content = await File.ReadAllTextAsync(filePath);
        Assert.Contains("\"Id\": 1", content);
        Assert.Contains("Buy groceries", content);
    }

    [Fact]
    public async Task ImportFromJson_ReadsTasksCorrectly()
    {
        // Arrange
        var originalTasks = GetSampleTasks();
        var filePath = GetTestFilePath("json");
        await _exportService.ExportToJsonAsync(originalTasks, filePath);

        // Act
        var importedTasks = await _exportService.ImportFromJsonAsync(filePath);

        // Assert
        Assert.Equal(3, importedTasks.Count);
        Assert.Equal("Buy groceries", importedTasks[0].Description);
        Assert.Equal(4, importedTasks[0].Priority);
        Assert.Equal(2, importedTasks[0].Tags.Count);
    }

    [Fact]
    public async Task ImportFromJson_NonExistentFile_ThrowsException()
    {
        // Arrange
        var filePath = "nonexistent_file.json";

        // Act & Assert
        await Assert.ThrowsAsync<FileNotFoundException>(
            () => _exportService.ImportFromJsonAsync(filePath)
        );
    }
}
