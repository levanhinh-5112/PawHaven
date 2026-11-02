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
  -e "MSSQL_PID=Express" \
  -e "SA_PASSWORD=<your_strong_password>" \
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

The API will start and output the local URLs where it's running (typically `https://localhost:7005` or `http://localhost:5241`).

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
- **Password**: `<your_strong_password>`

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

## API Endpoints

### Overview

All endpoints are prefixed with `/api/pets` and return standard JSON responses. Error responses follow the RFC 7807 ProblemDetails format with appropriate HTTP status codes.

### Pets Endpoints

#### List Pets
**GET** `/api/pets`

Returns a paginated list of pets with optional filtering and sorting.

**Query Parameters:**
- `page` (int, default: 1) - Page number (1-based)
- `pageSize` (int, default: 10) - Items per page
- `status` (string, optional) - Filter by pet status (Available, Adopted, Reserved)
- `species` (string, optional) - Filter by species (Dog, Cat, Rabbit, etc.)
- `sortBy` (string, default: created) - Sort field (name, species, age, status, created)
- `sortDirection` (string, default: desc) - Sort order (asc, desc)

**Response (200 OK):**
```json
{
  "items": [
    {
      "id": 1,
      "name": "Buddy",
      "species": "Dog",
      "breed": "Golden Retriever",
      "ageInMonths": 24,
      "status": "Available",
      "description": "Friendly and energetic",
      "photoUrl": "https://example.com/buddy.jpg",
      "createdAtUtc": "2026-10-01T10:00:00Z"
    }
  ],
  "page": 1,
  "pageSize": 10,
  "totalCount": 7,
  "totalPages": 1,
  "hasNextPage": false,
  "hasPreviousPage": false
}
```

**Error (400 Bad Request):**
- Invalid `page` (e.g., 0) → 400 with validation errors
- `pageSize` exceeds maximum → 400 with validation errors
- Unknown `sortBy` field → 400 with validation errors

---

#### Get Pet by ID
**GET** `/api/pets/{id}`

Retrieves a single pet by ID.

**Path Parameters:**
- `id` (int, required) - Pet ID

**Response (200 OK):**
```json
{
  "id": 1,
  "name": "Buddy",
  "species": "Dog",
  "status": "Available",
  "breed": "Golden Retriever",
  "ageInMonths": 24,
  "photoUrl": "https://example.com/buddy.jpg",
  "createdAtUtc": "2026-10-01T10:00:00Z"
}
```

**Error (404 Not Found):**
- Pet with given ID does not exist → 404 ProblemDetails

---

#### Create Pet
**POST** `/api/pets`

Creates a new pet. Returns 201 with Location header pointing to the new pet resource.

**Request Body:**
```json
{
  "name": "Max",
  "species": "Dog",
  "breed": "Labrador",
  "ageInMonths": 12,
  "description": "Friendly and energetic",
  "photoUrl": "https://example.com/max.jpg"
}
```

**Response (201 Created):**
- Location header: `/api/pets/{newPetId}`
- Response body: Created pet object (same structure as Get by ID)

**Errors (400 Bad Request):**
- Empty `name` → 400 with field error: "Pet name cannot be empty."
- Negative `ageInMonths` → 400 with field error
- Invalid `photoUrl` format → 400 with field error

---

#### Update Pet
**PUT** `/api/pets/{id}`

Updates an existing pet (except status). Returns 204 No Content.

**Path Parameters:**
- `id` (int, required) - Pet ID

**Request Body:**
```json
{
  "name": "Max Updated",
  "species": "Dog",
  "breed": "Labrador Mix",
  "ageInMonths": 13,
  "description": "Still friendly",
  "photoUrl": "https://example.com/max-updated.jpg"
}
```

**Response (204 No Content):**
- No response body; status remains unchanged

**Errors:**
- Pet ID not found → 404 ProblemDetails
- Invalid field (empty name, negative age, etc.) → 400 ProblemDetails with errors dictionary

---

#### Delete Pet
**DELETE** `/api/pets/{id}`

Deletes a pet. Returns 204 No Content.

**Path Parameters:**
- `id` (int, required) - Pet ID

**Response (204 No Content):**
- No response body

**Errors:**
- Pet ID not found → 404 ProblemDetails
- Pet has existing adoption applications → 409 Conflict ProblemDetails with message

---

### Response Format

#### Success Response
All endpoints return responses in JSON format with appropriate HTTP status codes (200, 201, 204).

#### Error Response (ProblemDetails - RFC 7807)
```json
{
  "type": "https://api.example.com/errors/validation",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "detail": "The request contains invalid data.",
  "errors": {
    "name": ["Pet name cannot be empty."],
    "ageInMonths": ["Age must be greater than or equal to 0."]
  }
}
```

**Common Status Codes:**
- `200 OK` - Successful GET request
- `201 Created` - Successful POST request with Location header
- `204 No Content` - Successful PUT/DELETE request
- `400 Bad Request` - Validation errors (with `errors` dictionary)
- `404 Not Found` - Resource not found
- `409 Conflict` - Business logic conflict (e.g., cannot delete pet with pending applications)
- `500 Internal Server Error` - Unhandled exception

---

### Testing with Swagger

1. Start the API: `dotnet run` from `PawHaven.API/`
2. Open Swagger UI at: `http://localhost:5241/swagger/index.html`
3. Click "Try it out" on any endpoint
4. Enter parameters and click "Execute"
5. View response status, headers, and body

### Testing with curl

#### List Pets (default)
```bash
curl -X GET "http://localhost:5241/api/pets" -H "accept: application/json"
```

#### List Pets (filter by species and sort)
```bash
curl -X GET "http://localhost:5241/api/pets?species=Dog&sortBy=name&sortDirection=asc" -H "accept: application/json"
```

#### Get Pet by ID
```bash
curl -X GET "http://localhost:5241/api/pets/1" -H "accept: application/json"
```

#### Create Pet
```bash
curl -X POST "http://localhost:5241/api/pets" \
  -H "Content-Type: application/json" \
  -d '{"name":"Buddy","species":"Dog","breed":"Golden","ageInMonths":24,"photoUrl":"https://example.com/buddy.jpg"}'
```

#### Update Pet
```bash
curl -X PUT "http://localhost:5241/api/pets/1" \
  -H "Content-Type: application/json" \
  -d '{"name":"Buddy Updated","species":"Dog","breed":"Golden Mix","ageInMonths":25,"photoUrl":"https://example.com/buddy.jpg"}'
```

#### Delete Pet
```bash
curl -X DELETE "http://localhost:5241/api/pets/1" -H "accept: application/json"
```

---

### Adding a New Command or Query

#### 1. Create the Command/Query Class
In `PawHaven.Application/Features/Pets/Commands/` or `Queries/`:

```csharp
namespace PawHaven.Application.Features.Pets.Commands;

public class CreatePetCommand
{
    public string Name { get; init; }
    public string Species { get; init; }
    // ... other properties
}
```

#### 2. Create the Handler
Implement the `ICommandHandler<T, TResult>` or `IQueryHandler<T, TResult>` interface:

```csharp
namespace PawHaven.Application.Features.Pets.Commands;

public class CreatePetCommandHandler : ICommandHandler<CreatePetCommand, PetDto>
{
    private readonly IPetRepository _petRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreatePetCommandHandler(IPetRepository petRepository, IUnitOfWork unitOfWork)
    {
        _petRepository = petRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<PetDto> HandleAsync(CreatePetCommand command, CancellationToken cancellationToken)
    {
        // Business logic here
        var pet = new Pet(command.Name, command.Species, /* ... */);
        await _petRepository.AddAsync(pet, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return pet.ToPetDto();
    }
}
```

#### 3. Register in DI
Add to `PawHaven.Application.Extensions.ApplicationServiceExtensions.AddApplication()`:

```csharp
services.AddScoped<ICommandHandler<CreatePetCommand, PetDto>, CreatePetCommandHandler>();
```

#### 4. Create API Controller Endpoint
Add method to `PawHaven.API.Controllers.PetsController`:

```csharp
[HttpPost]
public async Task<ActionResult<PetDto>> CreatePet([FromBody] CreatePetRequest request, CancellationToken cancellationToken)
{
    var command = new CreatePetCommand(request.Name, request.Species, /* ... */);
    var result = await _handler.HandleAsync(command, cancellationToken);
    return CreatedAtAction(nameof(GetPetById), new { id = result.Id }, result);
}
```

This follows the **CQRS pattern** where:
- **Commands** modify data (Create, Update, Delete)
- **Queries** retrieve data (List, Get)
- Each has a dedicated handler that encapsulates business logic
- Handlers are injected into controllers for loose coupling

---

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
