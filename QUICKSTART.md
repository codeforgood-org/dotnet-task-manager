# Quick Start Guide

Get started with Task Manager CLI in under 5 minutes!

## Installation

### Quick Install (Recommended)

```bash
# Clone and build
git clone https://github.com/codeforgood-org/dotnet-task-manager.git
cd dotnet-task-manager
./build.sh build

# Run your first command
./build.sh run -- help
```

### Using Docker

```bash
docker build -t taskmanager .
docker run taskmanager help
```

## First Steps

### 1. Add Your First Task

```bash
taskman add "Learn Task Manager CLI" --priority 5
```

### 2. View Your Tasks

```bash
taskman list
```

### 3. Add More Tasks

```bash
# Work tasks
taskman add "Review pull requests" --priority 4 --tags work,urgent
taskman add "Write documentation" --priority 3 --tags work,docs

# Personal tasks
taskman add "Buy groceries" --priority 2 --tags personal,shopping
taskman add "Schedule dentist" --priority 3 --tags personal,health --due 2024-02-01
```

### 4. Complete a Task

```bash
taskman complete 1
```

### 5. View Statistics

```bash
taskman stats
```

## Common Commands Cheat Sheet

| Command | Description | Example |
|---------|-------------|---------|
| `add` | Add new task | `taskman add "Task description" --priority 4 --tags work` |
| `list` | List all tasks | `taskman list` |
| `list --pending` | List pending only | `taskman list --pending` |
| `list --tag TAG` | Filter by tag | `taskman list --tag work` |
| `complete ID` | Mark as done | `taskman complete 1` |
| `remove ID` | Delete task | `taskman remove 1` |
| `update ID DESC` | Update description | `taskman update 1 "New description"` |
| `priority ID NUM` | Change priority | `taskman priority 1 5` |
| `search QUERY` | Find tasks | `taskman search meeting` |
| `clear` | Remove completed | `taskman clear` |
| `stats` | View statistics | `taskman stats` |
| `export` | Export tasks | `taskman export --format csv` |
| `import` | Import tasks | `taskman import backup.json` |

## Priority Levels

- **5** - Critical/Urgent (★★★★★)
- **4** - High (★★★★)
- **3** - Normal (★★★) - Default
- **2** - Low (★★)
- **1** - Very Low (★)

## Tagging Best Practices

Use tags to categorize and filter:

```bash
# Context tags
--tags @work, @home, @errands

# Project tags
--tags project-alpha, project-beta

# Type tags
--tags bug, feature, meeting

# Priority indicators
--tags urgent, important

# Combine multiple
--tags work,urgent,bug
```

## Example Workflows

### Daily Morning Routine

```bash
# Check what's pending
taskman list --pending

# See what's due today
taskman stats

# Add today's priorities
taskman add "Team standup" --priority 4 --tags work,meeting
taskman add "Review PRs" --priority 5 --tags work,urgent
```

### Weekly Review

```bash
# View all tasks
taskman list

# Export weekly report
taskman export --format markdown --output weekly-report.md

# Clear completed tasks
taskman clear

# Plan next week
taskman add "Plan sprint" --priority 4 --due 2024-01-22 --tags work,planning
```

### Project Management

```bash
# Add project tasks
taskman add "Design API" --priority 5 --tags project-x,design
taskman add "Write tests" --priority 4 --tags project-x,testing
taskman add "Documentation" --priority 3 --tags project-x,docs

# View project tasks
taskman list --tag project-x

# Search within project
taskman search API
```

## Configuration

Create `appsettings.json` to customize:

```json
{
  "AppConfig": {
    "DefaultPriority": 3,
    "DateFormat": "yyyy-MM-dd",
    "ShowCompletedByDefault": false
  }
}
```

## Export/Backup

### Quick Backup

```bash
# Backup tasks
cp tasks.json tasks-backup-$(date +%Y%m%d).json

# Or export
taskman export --format json --output backup/tasks-$(date +%Y%m%d).json
```

### Share with Team

```bash
# Export to readable format
taskman export --format markdown --output team-tasks.md

# Or CSV for spreadsheet
taskman export --format csv --output tasks.csv
```

## Docker Usage

### Run Once

```bash
docker run -v $(pwd)/data:/app/data taskmanager add "Docker task" --priority 5
docker run -v $(pwd)/data:/app/data taskmanager list
```

### Interactive Session

```bash
docker-compose up -d taskmanager-dev
docker exec -it taskmanager-dev bash
# Now use taskman commands inside container
```

## Development Setup

### VSCode

1. Open folder in VSCode
2. Install recommended extensions (prompt will appear)
3. Press `F5` to build and debug
4. Use `Ctrl+Shift+B` to run build tasks

### Building

```bash
# Build
./build.sh build

# Run tests
./build.sh test

# Run with arguments
./build.sh run -- list --pending

# Publish for all platforms
./build.sh publish
```

## Keyboard Shortcuts (if running in terminal)

- `Ctrl+C` - Cancel current command
- `Up/Down Arrow` - Navigate command history
- `Tab` - Auto-complete (if shell supports)

## Getting Help

```bash
# General help
taskman help

# Check version
taskman --version

# View examples
cat examples/EXAMPLES.md
```

## Tips

1. **Start Simple**: Begin with just `add` and `list`
2. **Use Tags**: They make filtering much easier
3. **Set Priorities**: Helps focus on what matters
4. **Regular Review**: Check and update tasks daily
5. **Export Often**: Backup your tasks regularly
6. **Search First**: Use `search` before scrolling through lists

## Next Steps

- Read [EXAMPLES.md](examples/EXAMPLES.md) for detailed scenarios
- Check [CONTRIBUTING.md](CONTRIBUTING.md) to contribute
- Review [docs/ARCHITECTURE.md](docs/ARCHITECTURE.md) to understand the code
- See [CHANGELOG.md](CHANGELOG.md) for version history

## Troubleshooting

**Tasks not saving?**
```bash
# Check permissions
ls -la tasks.json
```

**Command not found?**
```bash
# Use full path or build script
./build.sh run -- list
# Or
dotnet run --project src/TaskManager.CLI -- list
```

**Need to reset?**
```bash
# Backup first!
cp tasks.json tasks-backup.json
# Remove tasks file
rm tasks.json
# Start fresh
taskman add "First task"
```

---

**Ready to get productive? Start with:**

```bash
taskman add "Complete first task" --priority 5 --tags getting-started
taskman list
taskman complete 1
taskman stats
```

Happy task managing! 🚀
