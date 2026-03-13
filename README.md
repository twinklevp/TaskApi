# TaskAPI — .NET 10 + C# 14 POC

A **Task Management API proof-of-concept** built to demonstrate modern features from **.NET 10**, **C# 14**, and **EF Core 10**.

# Prerequisites

* .NET 10 SDK
* DB Browser for SQLite *(optional — for inspecting the database)*

---

# Run the Project

### 1. Clone the repository

```bash
git clone https://github.com/twinklevp/TaskApi.git
cd TaskApi
```

---

### 2. Install EF Core CLI tool *(first time only)*

dotnet tool install --global dotnet-ef
```

---

### 3. Create the migration

dotnet ef migrations add InitialCreate
```

---

### 4. Run the project

dotnet run
```

The SQLite database will be **created and seeded automatically** on first run.

---

# NuGet Packages

dotnet add package Microsoft.AspNetCore.OpenApi --version 10.0.0
dotnet add package Microsoft.EntityFrameworkCore.Sqlite --version 10.0.0
dotnet add package Microsoft.EntityFrameworkCore.Design --version 10.0.0
```

`Microsoft.AspNetCore.Mvc.RazorPages` is included automatically with the **Microsoft.NET.Sdk.Web SDK**, so no separate installation is required.

---

# Migration Commands

dotnet ef migrations add InitialCreate
dotnet ef database update
dotnet run
```

---

# Features Covered

| #  | Feature                         | File                                         | Version    |
| -- | ------------------------------- | -------------------------------------------- | ---------- |
| 1  | Built-in OpenAPI 3.1            | `OpenApiConfig.cs`                           | .NET 10    |
| 2  | Minimal APIs + MapGroup         | `Endpoints/*.cs`                             | .NET 10    |
| 3  | LINQ CountBy                    | `EfTaskRepository.cs`                        | .NET 10    |
| 4  | LINQ AggregateBy                | `EfTaskRepository.cs`                        | .NET 10    |
| 5  | LINQ Index()                    | `EfTaskRepository.cs`                        | .NET 10    |
| 6  | FrozenDictionary                | `EfTaskRepository.cs`                        | .NET 10    |
| 7  | Named Query Filters             | `TaskDbContext.cs`                           | EF Core 10 |
| 8  | Endpoint Filters                | `ValidationFilter.cs`, `LoggingFilter.cs`    | .NET 10    |
| 9  | `field` keyword                 | `Task.cs`                                    | C# 14      |
| 10 | `params IEnumerable<T>`         | `TaskExtensions.cs`                          | C# 14      |
| 11 | Collection expressions `[]`     | `Task.cs`, `TaskEndpoints.cs`                | C# 14      |
| 12 | Primary constructors            | `EfTaskRepository.cs`, Page Models           | C# 14      |
| 13 | Partial members                 | `TaskDbContext.cs`, `TaskDbContext.Hooks.cs` | C# 14      |
| 14 | Null-condition assignment `??=` | `TaskDbContext.Hooks.cs`                     | C# 14      |

---
