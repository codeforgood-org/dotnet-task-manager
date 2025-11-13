# Task Manager Examples

This directory contains example files and usage scenarios for the Task Manager CLI.

## Sample Tasks

The `sample-tasks.json` file contains a variety of example tasks demonstrating different features:

- Tasks with different priority levels (1-5)
- Completed and pending tasks
- Tasks with due dates
- Tasks with multiple tags
- Work and personal tasks

### Loading Sample Tasks

To try out the sample tasks:

```bash
# Copy sample tasks to your working directory
cp examples/sample-tasks.json tasks.json

# List all tasks
taskman list

# List only pending tasks
taskman list --pending

# Filter by tag
taskman list --tag work
```

## Common Usage Scenarios

### 1. Daily Task Management

```bash
# Morning: Add today's tasks
taskman add "Review team PRs" --priority 5 --tags work,urgent
taskman add "Prepare status report" --priority 4 --due 2024-01-20
taskman add "Call client about project" --priority 3

# Throughout the day: Check tasks
taskman list --pending

# Complete tasks as you finish them
taskman complete 1
taskman complete 2

# Evening: Review what's left
taskman list --pending
```

### 2. Project-Based Organization

```bash
# Add tasks for a specific project using tags
taskman add "Design API endpoints" --priority 5 --tags project-x,api,design
taskman add "Write unit tests" --priority 4 --tags project-x,testing
taskman add "Update API documentation" --priority 3 --tags project-x,docs

# View all tasks for the project
taskman list --tag project-x

# Search within project
taskman search API
```

### 3. Weekly Planning

```bash
# Monday: Plan the week
taskman add "Team standup" --priority 4 --due 2024-01-22 --tags work,meeting
taskman add "Code review session" --priority 3 --due 2024-01-23 --tags work
taskman add "Submit expense report" --priority 2 --due 2024-01-26 --tags admin

# Check statistics
taskman stats

# View upcoming tasks
taskman list --pending

# Friday: Clear completed tasks
taskman clear
```

### 4. Personal Task Management

```bash
# Add personal tasks
taskman add "Buy birthday gift" --priority 4 --due 2024-02-01 --tags personal,shopping
taskman add "Book vacation flights" --priority 5 --due 2024-01-25 --tags personal,travel
taskman add "Read new book" --priority 2 --tags personal,leisure

# Filter personal tasks
taskman list --tag personal

# Update priorities as needed
taskman priority 3 5
```

### 5. Team Collaboration Export

```bash
# Export tasks for sharing with team
taskman export --format markdown --output team-tasks.md

# Export high-priority items to CSV
taskman list --pending > pending-tasks.txt
taskman export --format csv --output tasks-report.csv

# Import tasks from team member
taskman import team-member-tasks.json
```

### 6. Priority Management

```bash
# Add tasks with priorities
taskman add "Critical bug fix" --priority 5 --tags urgent,bug
taskman add "Nice-to-have feature" --priority 1 --tags enhancement

# List tasks (automatically sorted by priority)
taskman list --pending

# Update priority when circumstances change
taskman priority 1 5  # Escalate task 1 to highest priority
```

### 7. Tag-Based Workflows

```bash
# Use tags for contexts
taskman add "Email project update" --tags work,communication,@office
taskman add "Pick up dry cleaning" --tags personal,errands,@out
taskman add "Review code" --tags work,coding,@computer

# Work from specific contexts
taskman list --tag @office
taskman list --tag @computer
```

### 8. Batch Operations

```bash
# Complete multiple tasks
for id in 1 2 3 4 5; do
  taskman complete $id
done

# Clear all completed
taskman clear

# Export weekly report
taskman stats
taskman export --format markdown --output weekly-report.md
```

## Task Templates

### Work Tasks
```bash
# Bug fix template
taskman add "Fix: [description]" --priority 5 --tags work,bug,urgent

# Feature template
taskman add "Feature: [description]" --priority 3 --tags work,feature

# Meeting template
taskman add "Meeting: [topic]" --priority 4 --due [date] --tags work,meeting
```

### Personal Tasks
```bash
# Shopping template
taskman add "Buy [item]" --priority 2 --tags personal,shopping

# Health template
taskman add "Doctor: [appointment]" --priority 4 --due [date] --tags personal,health

# Learning template
taskman add "Learn [topic]" --priority 3 --tags personal,learning
```

### Recurring Task Patterns
```bash
# Weekly review (add every Friday)
taskman add "Weekly review" --priority 3 --due [next-friday] --tags work,review

# Monthly tasks
taskman add "Submit timesheet" --priority 4 --due [end-of-month] --tags work,admin
```

## Advanced Features

### Statistics and Reporting
```bash
# View overall statistics
taskman stats

# Check upcoming deadlines
taskman list --pending | grep "Due:"

# Analyze productivity
taskman stats --period week
```

### Backup and Restore
```bash
# Backup current tasks
cp tasks.json tasks-backup-$(date +%Y%m%d).json

# Export to multiple formats for archival
taskman export --format json --output archive/tasks-$(date +%Y%m%d).json
taskman export --format csv --output archive/tasks-$(date +%Y%m%d).csv
taskman export --format markdown --output archive/tasks-$(date +%Y%m%d).md
```

### Integration with Other Tools
```bash
# Use with grep for custom filtering
taskman list | grep urgent

# Count pending tasks
taskman list --pending | wc -l

# Pipe to text processing tools
taskman list --tag work | awk '{print $1, $2}'
```

## Tips and Best Practices

1. **Use Consistent Tags**: Establish a tagging convention early
2. **Set Realistic Priorities**: Not everything can be priority 5
3. **Regular Reviews**: Clean up completed tasks weekly
4. **Use Due Dates**: But don't over-schedule
5. **Export Regularly**: Keep backups of your task data
6. **Leverage Search**: Use search instead of scrolling through long lists
7. **Update as You Go**: Keep task status current for best planning

## Troubleshooting

### Tasks Not Saving
```bash
# Check file permissions
ls -la tasks.json

# Verify JSON syntax
cat tasks.json | python -m json.tool
```

### Lost Tasks
```bash
# Check for backup files
ls tasks*.json

# Restore from backup
cp tasks-backup.json tasks.json
```

For more information, see the main [README.md](../README.md) and [CONTRIBUTING.md](../CONTRIBUTING.md).
