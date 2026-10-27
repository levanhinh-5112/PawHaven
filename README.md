# PawHaven - Pet Care Management System

A modern, layered ASP.NET Core 10 application for managing pet care services. Built with Clean Architecture principles, featuring dependency injection patterns and a scalable foundation for future feature development.

## Table of Contents

- [Prerequisites](#prerequisites)
- [Quick Start](#quick-start)
- [Running the API](#running-the-api)
- [Project Structure](#project-structure)
- [Architecture Overview](#architecture-overview)
- [Architecture Conventions](#architecture-conventions)

## Prerequisites

Before you begin, ensure you have the following installed:

- **.NET 10 SDK** or later
  - Download from [dotnet.microsoft.com](https://dotnet.microsoft.com/download)
  - Verify installation: `dotnet --version`

- **Visual Studio 2022+** (recommended) or **Visual Studio Code**
  - Visual Studio Community edition is free and recommended
  - VS Code requires the C# extension

- **Git** (for cloning the repository)
  - Download from [git-scm.com](https://git-scm.com)

## Quick Start

### 1. Clone the Repository

```bash
git clone https://github.com/levanhinh-5112/PawHaven.git
cd PawHaven
```

### 2. Start SQL Server in Docker

The application uses SQL Server 2022 Express. Start the container:

```bash
docker run -e "ACCEPT_EULA=Y" \
  -e "SA_PASSWORD=PawHaven@123" \
  -p 1433:1433 \
  --name pawHavenDb \
  -d mcr.microsoft.com/mssql/server:2022-latest
```

Verify the container is running:

```bash
docker ps | grep pawHavenDb
```

### 3. Configure User Secrets

Store sensitive configuration locally without committing to source control:

```bash
cd PawHaven.API
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost,1433;Database=PawHavenDb;User Id=sa;Password=PawHaven@123;TrustServerCertificate=true;Encrypt=false;"
```

Verify secrets are set:

```bash
dotnet user-secrets list
```

### 4. Apply Database Migrations

Create the database schema and seed data:

```bash
dotnet ef database update --project PawHaven.Infrastructure --startup-project PawHaven.API
```

This creates:
- `PawHavenDb` database on SQL Server
- `Pets`, `Users`, `AdoptionApplications` tables
- 10 seed pets for development/testing
- Unique index on `Users.Email`
- Foreign key constraints with restrict delete behavior

### 5. Build the Solution

```bash
dotnet build
```

This command will:
- Restore all NuGet package dependencies
- Compile all four projects
- Verify the build succeeds with no errors or warnings

### 6. Run the API

```bash
cd PawHaven.API
dotnet run
```

The API will start and output the local URLs where it's running (typically `https://localhost:7126`).

### 7. Access the API

Once the API is running, you can access:

- **Swagger UI** (API Explorer): `https://localhost:7126/swagger/index.html`
  - Interactive interface to test all endpoints
  - View request/response schemas

- **Health Check**: `https://localhost:7126/api/v1/health`
  - Confirms the API is running and healthy
  - Returns `200 OK` with a status message

## Database Management

### Verify Database Setup

Connect to SQL Server via SSMS or Azure Data Studio:
- **Server**: `localhost,1433`
- **Login**: `sa`
- **Password**: `PawHaven@123`

Verify tables and seed data:

```sql
-- Check table existence
SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_CATALOG = 'PawHavenDb' AND TABLE_TYPE = 'BASE TABLE'
ORDER BY TABLE_NAME;

-- Count seed pets
SELECT COUNT(*) AS PetCount FROM Pets;

-- View all seed pets
SELECT Id, Name, Species, Status, Breed, CreatedAtUtc FROM Pets ORDER BY Id;

-- Verify unique index on Users.Email
SELECT * FROM sys.indexes 
WHERE object_id = OBJECT_ID('Users') AND name LIKE '%Email%';

-- Verify foreign key constraints
SELECT name, referenced_object_name, delete_referential_action_desc 
FROM sys.foreign_keys 
WHERE parent_object_id = OBJECT_ID('AdoptionApplications');
```

### Entity Framework Core Commands

Add a new migration:

```bash
dotnet ef migrations add MigrationName --project PawHaven.Infrastructure
```

Remove the latest unapplied migration:

```bash
dotnet ef migrations remove --project PawHaven.Infrastructure
```

Update database to latest migration:

```bash
dotnet ef database update --project PawHaven.Infrastructure
```

Reset database (development only):

```bash
dotnet ef database drop --project PawHaven.Infrastructure
dotnet ef database update --project PawHaven.Infrastructure
```

## Running the API

### From Command Line

```bash
cd PawHaven.API
dotnet run
```

### From Visual Studio

1. Open `PawHaven.slnx` in Visual Studio
2. Right-click **PawHaven.API** project → **Set as Startup Project**
3. Press `F5` or click **Start**
4. The API will launch and open in your default browser

### From Visual Studio Code

1. Install the C# and C# Dev Kit extensions
2. Open the PawHaven folder
3. Open the integrated terminal: `` Ctrl+` ``
4. Run: `cd PawHaven.API && dotnet run`

## Project Structure

The PawHaven solution consists of four projects following **Clean Architecture** principles:

```
PawHaven/
├── PawHaven.Domain/              # Core business entities (no dependencies)
│   └── (Pet domain models, entities, value objects)
│
├── PawHaven.Application/         # Business logic layer (depends on Domain)
│   ├── Abstractions/             # Interfaces and contracts
│   │   └── IPetRepository.cs     # Repository interface for pet data access
│   ├── Extensions/               # DI extension methods
│   │   └── ApplicationServiceExtensions.cs
│   └── (Services, use cases, application-level logic)
│
├── PawHaven.Infrastructure/      # Data access & external services (depends on Application)
│   ├── Extensions/               # DI extension methods
│   │   └── InfrastructureServiceExtensions.cs
│   ├── Repositories/             # Data access implementations
│   │   └── PetRepository.cs      # Internal implementation of IPetRepository
│   └── (Database contexts, external APIs, persistence)
│
└── PawHaven.API/                 # Web API layer (depends on Application & Infrastructure)
    ├── Program.cs                # Dependency injection configuration & startup
    ├── Controllers/              # API endpoints
    └── (Middleware, filters, API configuration)
```

## Architecture Overview

### Layered Architecture Diagram

```
┌─────────────────────────────────────────────────┐
│  PawHaven.API (ASP.NET Core Web API)            │ ← Entry point
│  - Controllers & HTTP handlers                  │
├─────────────────────────────────────────────────┤
│  ↓ (depends on)                                 │
├─────────────────────────────────────────────────┤
│  PawHaven.Application (Business Logic)          │
│  - Use cases & service abstractions             │
├─────────────────────────────────────────────────┤
│  ↓ (depends on)                                 │
├─────────────────────────────────────────────────┤
│  PawHaven.Infrastructure (Data & Integration)  │
│  - Repository implementations                   │
│  - Database access                              │
├─────────────────────────────────────────────────┤
│  ↓ (depends on)                                 │
├─────────────────────────────────────────────────┤
│  PawHaven.Domain (Core Business)                │
│  - Entities & value objects                     │
│  - NO external dependencies                     │
└─────────────────────────────────────────────────┘
```

### Dependency Flow

| Layer | Depends On | Purpose |
|-------|-----------|---------|
| **API** | Application, Infrastructure | HTTP request handling, routing, middleware |
| **Application** | Domain | Business logic, use cases, service abstractions |
| **Infrastructure** | Application | Data access, repository implementations, external integrations |
| **Domain** | Nothing | Core business entities, rules, and models |

**Key Rule:** Dependencies flow **downward only**. A lower layer never references an upper layer.

## Architecture Conventions

### 1. Dependency Injection (DI)

All service registrations happen in extension methods within each layer:

- **Application Layer**: `ApplicationServiceExtensions.AddApplication()`
  - Registers application-level services
  - Called from `Program.cs`

- **Infrastructure Layer**: `InfrastructureServiceExtensions.AddInfrastructure()`
  - Registers repository implementations and data access services
  - Called from `Program.cs`

Example in `Program.cs`:

```csharp
using PawHaven.Application.Extensions;
using PawHaven.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Register all services
builder.Services.AddApplication();
builder.Services.AddInfrastructure();

var app = builder.Build();
app.Run();
```

### 2. Repository Pattern

- **Interfaces** are defined in the **Application** layer (`Abstractions` folder)
  - Example: `PawHaven.Application.Abstractions.IPetRepository`
  - Accessible to all layers that depend on Application

- **Implementations** are `internal` classes in the **Infrastructure** layer
  - Example: `PawHaven.Infrastructure.Repositories.PetRepository`
  - Hidden from external consumers; only exposed via interface

### 3. Namespace Conventions

Follow this structure for organization:

```
Namespace: PawHaven.<LayerName>.<FolderName>
Example:  PawHaven.Application.Abstractions
		  PawHaven.Infrastructure.Repositories
		  PawHaven.Domain.Entities
```

### 4. Adding New Features

When adding a new feature (e.g., "Pets"):

1. **Domain Layer** → Define `Pet` entity (if applicable)
2. **Application Layer** → Define `IPetRepository` interface in `Abstractions/`
3. **Infrastructure Layer** → Implement `PetRepository` in `Repositories/` (as `internal`)
4. **Infrastructure Layer** → Register in `InfrastructureServiceExtensions`
5. **API Layer** → Create `PetsController` and inject dependencies
6. **Program.cs** → Ensure extension methods are called (already done in template)

### 5. Folder Structure Guidelines

- Keep related files in the same folder
- Use meaningful folder names that reflect the layer's responsibility
- Remove placeholder files (`.gitkeep`) once actual content is added
- Example:
  ```
  Application/
  ├── Abstractions/     (Interfaces & contracts)
  ├── Extensions/       (DI extension methods)
  ├── Services/         (Service implementations)
  └── Models/           (DTOs, view models)
  ```

## Development Workflow

### Creating a Feature Branch

```bash
git checkout -b feature/PAW-XX-short-description
```

Follow the ticket ID format: `feature/PAW-<ticket-number>-<kebab-case-description>`

### Committing Changes

Write clear, descriptive commit messages:

```bash
git commit -m "feat: PAW-12 Add placeholder IPetRepository and register in DI"
```

### Push and Create PR

```bash
git push origin feature/PAW-XX-short-description
```

Then create a Pull Request on GitHub with a detailed body (see `.github/copilot-instructions.md` for PR template).

## Troubleshooting

### Build Errors

**Issue**: `dotnet build` fails with missing packages

**Solution**:
```bash
dotnet restore
dotnet build
```

### API Won't Start

**Issue**: Port `5001` already in use

**Solution**:
```bash
# Run on a different port
dotnet run --urls "https://localhost:5002"
```

### Swagger UI Not Loading

**Issue**: Swagger returns 404

**Solution**:
1. Verify API is running (no startup errors)
2. Check the correct URL format: `https://localhost:5001/swagger/index.html`
3. Clear browser cache

## Additional Resources

- [.NET Documentation](https://docs.microsoft.com/dotnet)
- [Clean Architecture by Robert Martin](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [Microsoft Dependency Injection Docs](https://docs.microsoft.com/en-us/dotnet/core/extensions/dependency-injection)
- [ASP.NET Core Documentation](https://docs.microsoft.com/aspnet/core)

## Contributing

Refer to `.github/copilot-instructions.md` for pull request guidelines, including title format, body template, and acceptance criteria.

---

**Last Updated**: October 2026  
**Maintained By**: PawHaven Team
