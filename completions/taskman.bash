#!/usr/bin/env bash
# Bash completion script for taskman

_taskman_completions()
{
    local cur prev opts
    COMPREPLY=()
    cur="${COMP_WORDS[COMP_CWORD]}"
    prev="${COMP_WORDS[COMP_CWORD-1]}"

    # Available commands
    local commands="add list remove complete update priority search clear stats export import help"

    # Options for specific commands
    local add_opts="--priority --due --tags"
    local list_opts="--pending --tag"
    local export_opts="--format --output"

    # Complete commands
    if [[ ${COMP_CWORD} == 1 ]]; then
        COMPREPLY=( $(compgen -W "${commands}" -- ${cur}) )
        return 0
    fi

    # Complete based on the command
    case "${COMP_WORDS[1]}" in
        add)
            if [[ ${cur} == -* ]]; then
                COMPREPLY=( $(compgen -W "${add_opts}" -- ${cur}) )
            fi
            return 0
            ;;
        list)
            if [[ ${cur} == -* ]]; then
                COMPREPLY=( $(compgen -W "${list_opts}" -- ${cur}) )
            fi
            return 0
            ;;
        export)
            case "${prev}" in
                --format)
                    COMPREPLY=( $(compgen -W "csv json markdown md" -- ${cur}) )
                    return 0
                    ;;
                --output)
                    # Complete filenames
                    COMPREPLY=( $(compgen -f -- ${cur}) )
                    return 0
                    ;;
                *)
                    if [[ ${cur} == -* ]]; then
                        COMPREPLY=( $(compgen -W "${export_opts}" -- ${cur}) )
                    fi
                    return 0
                    ;;
            esac
            ;;
        import)
            # Complete JSON files
            COMPREPLY=( $(compgen -f -X '!*.json' -- ${cur}) )
            return 0
            ;;
        complete|remove|update|priority)
            # These commands expect task IDs - could be enhanced to list actual task IDs
            return 0
            ;;
    esac
}

complete -F _taskman_completions taskman
