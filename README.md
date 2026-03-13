# TaskAPI — .NET 10 + C# 14 POC
A POC Task Management API built to demonstrate the latest features in .NET 10, C# 14, and EF Core 10. Includes a full Razor Pages UI alongside the REST API.

# Prerequisites
.NET 10 SDK
DB Browser for SQLite (optional — to inspect the database)

# Run the project
# 1. Clone the repo
git clone https://github.com/twinklevp/TaskApi.git
cd TaskApi

# 2. Install EF Core CLI tool (first time only)
dotnet tool install --global dotnet-ef

# 3. Create the migration
dotnet ef migrations add InitialCreate

# 4. Run — database is created and seeded automatically
dotnet run

# NuGet Packages
dotnet add package Microsoft.AspNetCore.OpenApi --version 10.0.0
dotnet add package Microsoft.EntityFrameworkCore.Sqlite --version 10.0.0
dotnet add package Microsoft.EntityFrameworkCore.Design --version 10.0.0

Microsoft.AspNetCore.Mvc.RazorPages is included in the Microsoft.NET.Sdk.Web SDK — no separate install needed.

# Migration Commands
dotnet ef migrations add InitialCreate
dotnet ef database update
dotnet run  

# Features covered

| #  | Feature                       | File                                         | Version    |
| -- | ----------------------------- | -------------------------------------------- | ---------- |
| 1  | Built-in OpenAPI 3.1          | `OpenApiConfig.cs`                           | .NET 10    |
| 2  | Minimal APIs + MapGroup       | `Endpoints/*.cs`                             | .NET 10    |
| 3  | LINQ CountBy                  | `EfTaskRepository.cs`                        | .NET 10    |
| 4  | LINQ AggregateBy              | `EfTaskRepository.cs`                        | .NET 10    |
| 5  | LINQ Index()                  | `EfTaskRepository.cs`                        | .NET 10    |
| 6  | FrozenDictionary              | `EfTaskRepository.cs`                        | .NET 10    |
| 7  | Named Query Filters           | `TaskDbContext.cs`                           | .NET 10 |
| 8  | Endpoint Filters              | `ValidationFilter.cs`, `LoggingFilter.cs`    | .NET 10    |
| 9  | field keyword                 | `Task.cs`                                    | C# 14      |
| 10 | params IEnumerable<T>         | `TaskExtensions.cs`                          | C# 14      |
| 11 | Collection expressions []     | `Task.cs`, `TaskEndpoints.cs`                | C# 14      |
| 12 | Primary constructors          | `EfTaskRepository.cs`, Page Models           | C# 14      |
| 13 | Partial members               | `TaskDbContext.cs`, `TaskDbContext.Hooks.cs` | C# 14      |
| 14 | Null-condition assignment ??= | `TaskDbContext.Hooks.cs`                     | C# 14      |
