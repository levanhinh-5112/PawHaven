# JWT Configuration & User Secrets

## Overview
JWT authentication configuration is split between public configuration (`appsettings.json`) and secrets (`User Secrets`):

- **appsettings.json**: Non-sensitive settings (Issuer, Audience, ExpirationMinutes)
- **User Secrets** (dev): Sensitive data like JWT Secret (NOT committed to git)
- **Environment Variables/Key Vault** (production): Handled at deployment time

## Development Setup

### User Secrets Location
User Secrets are stored locally in:
- **Windows**: `%APPDATA%\Microsoft\UserSecrets\<UserSecretsId>\secrets.json`
- **Linux/Mac**: `~/.microsoft/usersecrets/<UserSecretsId>/secrets.json`

The `UserSecretsId` for PawHaven.API is stored in `PawHaven.API/PawHaven.API.csproj`.

### Viewing Current Secrets
```powershell
cd PawHaven.API
dotnet user-secrets list
```

### Setting/Updating Secrets
```powershell
cd PawHaven.API

# Set JWT Secret
dotnet user-secrets set "Jwt:Secret" "your-32-character-secret-key"

# Set Database Connection (alternative to appsettings.json)
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost,1433;Database=PawHavenDb;User Id=sa;Password=YourPassword;Encrypt=false;TrustServerCertificate=true;"

# Remove a secret
dotnet user-secrets remove "Jwt:Secret"

# Clear all secrets
dotnet user-secrets clear
```

### Current Configuration
- **Jwt:Secret**: Set in User Secrets (32+ characters for HS256)
- **Jwt:Issuer**: "PawHaven" (from appsettings.json)
- **Jwt:Audience**: "PawHavenClient" (from appsettings.json)
- **Jwt:ExpirationMinutes**: 60 (from appsettings.json)

## Production Deployment

For production, do NOT use `appsettings.json` for secrets:

### Option 1: Environment Variables
```bash
export Jwt__Secret="your-production-secret-key"
export ConnectionStrings__DefaultConnection="production-db-connection-string"
```

### Option 2: Azure Key Vault
```csharp
// In Program.cs
builder.Configuration.AddAzureKeyVault(
	new Uri($"https://{keyVaultName}.vault.azure.net/"),
	new DefaultAzureCredential());
```

### Option 3: AWS Secrets Manager
```csharp
// In Program.cs
builder.Configuration.AddSecretsManager(region: RegionEndpoint.USEast1);
```

## .gitignore Entry

Ensure User Secrets are NOT committed:
```
# User Secrets
D:\Study\.net identity\PawHaven/PawHaven.API/Properties/launchSettings.json
%APPDATA%\Microsoft\UserSecrets\
~/.microsoft/usersecrets/
```

## Security Notes

1. ✅ **Never commit JWT Secret to version control**
2. ✅ **Use strong, cryptographically random secrets** (32+ characters for HS256)
3. ✅ **Rotate secrets periodically** (especially in production)
4. ✅ **Use managed secrets services** in production (Azure Key Vault, AWS Secrets Manager, etc.)
5. ✅ **Restrict access** to secrets in development machines
6. ⚠️ **JWT Secret in appsettings.Development.json** is acceptable only for local development
7. ⚠️ **Never hardcode secrets** in source code

## References
- [Safe storage of app secrets in development (Microsoft Docs)](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets)
- [Configuration in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/)
- [Azure Key Vault configuration provider](https://learn.microsoft.com/en-us/aspnet/core/security/key-vault-configuration)
