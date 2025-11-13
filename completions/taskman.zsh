#compdef taskman

# Zsh completion script for taskman

_taskman() {
    local -a commands
    commands=(
        'add:Add a new task'
        'list:List tasks'
        'remove:Remove a task'
        'complete:Mark a task as completed'
        'update:Update task description'
        'priority:Update task priority'
        'search:Search tasks'
        'clear:Remove all completed tasks'
        'stats:View task statistics'
        'export:Export tasks to file'
        'import:Import tasks from JSON file'
        'help:Show help message'
    )

    local -a add_opts
    add_opts=(
        '--priority[Set priority (1-5)]:priority:(1 2 3 4 5)'
        '--due[Set due date (yyyy-MM-dd)]:date:'
        '--tags[Add tags (comma-separated)]:tags:'
    )

    local -a list_opts
    list_opts=(
        '--pending[Show only pending tasks]'
        '--tag[Filter by tag]:tag:'
    )

    local -a export_opts
    export_opts=(
        '--format[Export format]:format:(csv json markdown md)'
        '--output[Output file path]:file:_files'
    )

    local curcontext="$curcontext" state line
    typeset -A opt_args

    _arguments -C \
        '1: :->command' \
        '*:: :->args'

    case $state in
        command)
            _describe 'command' commands
            ;;
        args)
            case $line[1] in
                add)
                    _arguments $add_opts
                    ;;
                list)
                    _arguments $list_opts
                    ;;
                export)
                    _arguments $export_opts
                    ;;
                import)
                    _files -g '*.json'
                    ;;
                complete|remove|update|priority)
                    _message 'task ID'
                    ;;
                search)
                    _message 'search query'
                    ;;
            esac
            ;;
    esac
}

_taskman "$@"
