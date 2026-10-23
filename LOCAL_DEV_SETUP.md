# PawHaven - Local Development Setup

## Prerequisites

- .NET 10 SDK
- Docker Desktop (running)
- SQL Server command-line tools (sqlcmd)
- Entity Framework CLI tool: `dotnet ef`

## Setup Steps

### 1. Verify .NET 10 SDK
```powershell
dotnet --version
```

### 2. Verify EF Core CLI Tool
The `dotnet-ef` tool should already be installed globally:
```powershell
dotnet ef --version
# Expected output: Entity Framework Core .NET Command-line Tools 10.0.13 (or similar)
```

### 3. Install EF Core Tools (if needed)
```powershell
dotnet tool install --global dotnet-ef
```

### 4. Verify NuGet Packages

The following packages should be installed:
- **PawHaven.Infrastructure**: `Microsoft.EntityFrameworkCore.SqlServer` (v10.0.0)
- **PawHaven.API**: `Microsoft.EntityFrameworkCore.Design` (v10.0.0)
- **PawHaven.Domain**: No EF Core packages
- **PawHaven.Application**: No EF Core packages

Verify with:
```powershell
dotnet list package
```

### 5. Start SQL Server Container

Ensure Docker Desktop is running, then execute:

```powershell
cd D:\Study\.net identity\PawHaven
docker compose up -d
```

This will:
- Pull SQL Server 2022 Express image
- Start container named `pawhaven-sql`
- Listen on port 1433
- Use SA password: `PawHaven@123`

**Note:** Data is stored in the container's temporary filesystem. When the container stops, data is lost. This is fine for local development; for persistent data, add a volume mount to `docker-compose.yml`.

### 6. Verify SQL Server Container is Running

```powershell
docker ps --filter "name=pawhaven-sql"
```

Expected output shows container running on port 1433.

### 7. Test SQL Server Connectivity

```powershell
sqlcmd -S localhost,1433 -U sa -P "PawHaven@123" -Q "SELECT 1"
```

Expected output: `1` (successful connection)

### 8. Build Solution

```powershell
dotnet build
```

Expected: 0 errors, 0 warnings

## Next Steps

Once setup is complete, you can:
- Create EF Core migrations: `dotnet ef migrations add <MigrationName> -p PawHaven.Infrastructure -s PawHaven.API`
- Update database: `dotnet ef database update -p PawHaven.Infrastructure -s PawHaven.API`
- Create repositories in Infrastructure layer
- Configure entity mappings in DbContext

## Troubleshooting

### Docker daemon not running
- Start Docker Desktop
- Ensure it's fully initialized (check system tray)
- Run elevated (admin) PowerShell/cmd for docker commands

### SQL Server connection refused
- Verify container is running: `docker ps`
- Check port 1433 is not in use: `netstat -ano | findstr :1433`
- Wait 30-60 seconds for SQL Server to fully initialize after container start

### EF Core CLI not found
- Reinstall: `dotnet tool update --global dotnet-ef`
- Verify PATH includes .dotnet\tools

## Connection Strings

For future use in `appsettings.json`:
```json
{
  "ConnectionStrings": {
	"DefaultConnection": "Server=localhost,1433;Database=PawHaven;User Id=sa;Password=PawHaven@123;Encrypt=false;TrustServerCertificate=true;"
  }
}
```

## Security Note

The SA password `PawHaven@123` is only for local development. Never use this in production or commit actual credentials to version control.
