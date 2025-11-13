# Shell Completions

Shell completion scripts for Task Manager CLI.

## Installation

### Bash

Add to your `~/.bashrc` or `~/.bash_profile`:

```bash
source /path/to/dotnet-task-manager/completions/taskman.bash
```

Or install system-wide:

```bash
sudo cp completions/taskman.bash /etc/bash_completion.d/taskman
```

Then reload your shell:

```bash
source ~/.bashrc
```

### Zsh

Add to your `~/.zshrc`:

```zsh
fpath=(/path/to/dotnet-task-manager/completions $fpath)
autoload -Uz compinit && compinit
```

Or copy to a directory in your `$fpath`:

```bash
# Find your fpath
echo $fpath

# Copy to one of those directories
cp completions/taskman.zsh /usr/local/share/zsh/site-functions/_taskman
```

Then reload your shell:

```zsh
exec zsh
```

### Oh My Zsh

If you use Oh My Zsh, create a custom plugin:

```bash
mkdir -p ~/.oh-my-zsh/custom/plugins/taskman
cp completions/taskman.zsh ~/.oh-my-zsh/custom/plugins/taskman/_taskman
```

Then add `taskman` to your plugins in `~/.zshrc`:

```zsh
plugins=(... taskman)
```

## Usage

After installation, you can use tab completion with taskman:

```bash
# Complete commands
taskman <TAB>

# Complete options for add command
taskman add "My task" --<TAB>

# Complete export formats
taskman export --format <TAB>

# Complete JSON files for import
taskman import <TAB>
```

## Features

- Command completion
- Option completion
- Format completion for export command
- File completion for import command (JSON files only)
- Helpful descriptions (Zsh)

## Testing

Test the completions:

```bash
# Bash
taskman <TAB><TAB>

# Zsh
taskman <TAB>
```

You should see a list of available commands.

## Troubleshooting

### Bash completion not working

1. Ensure bash-completion is installed:
   ```bash
   # Ubuntu/Debian
   sudo apt-get install bash-completion

   # macOS with Homebrew
   brew install bash-completion
   ```

2. Verify the completion script is sourced:
   ```bash
   grep taskman ~/.bashrc
   ```

### Zsh completion not working

1. Verify compinit is called:
   ```bash
   grep compinit ~/.zshrc
   ```

2. Rebuild completion cache:
   ```zsh
   rm -f ~/.zcompdump
   compinit
   ```

3. Check fpath includes the completions directory:
   ```zsh
   echo $fpath
   ```

## Development

To modify completions:

1. Edit the completion script
2. Reload your shell or source the script
3. Test the completions
4. Submit a PR with your improvements

For more information on writing completion scripts:

- Bash: https://www.gnu.org/software/bash/manual/html_node/Programmable-Completion.html
- Zsh: http://zsh.sourceforge.net/Doc/Release/Completion-System.html
