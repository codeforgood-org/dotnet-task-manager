# Use the official .NET SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy solution and project files
COPY TaskManager.sln ./
COPY src/TaskManager.CLI/TaskManager.CLI.csproj ./src/TaskManager.CLI/
COPY tests/TaskManager.Tests/TaskManager.Tests.csproj ./tests/TaskManager.Tests/

# Restore dependencies
RUN dotnet restore

# Copy the rest of the source code
COPY . .

# Build the application
RUN dotnet build -c Release --no-restore

# Run tests
RUN dotnet test --no-build -c Release

# Publish the application
RUN dotnet publish src/TaskManager.CLI/TaskManager.CLI.csproj -c Release -o /app/publish --no-build

# Use the official .NET runtime image for running
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS runtime
WORKDIR /app

# Copy the published application
COPY --from=build /app/publish .

# Create a volume for task data
VOLUME ["/app/data"]

# Set environment variable for tasks file location
ENV TASKS_FILE_PATH=/app/data/tasks.json

# Set the entrypoint
ENTRYPOINT ["./taskman"]

# Default command (show help)
CMD ["help"]
