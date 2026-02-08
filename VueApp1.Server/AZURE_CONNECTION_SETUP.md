# Azure SQL Connection String Configuration

## Your Setup

You have:
- **dev_azuresql** → Connection string name: `dev_DefaultConnection`
- **prod_azuresql** → Connection string name: `prod_DefaultConnection`

## How It Works

The app automatically selects the correct connection string based on `ASPNETCORE_ENVIRONMENT`:

```
ASPNETCORE_ENVIRONMENT = Development  →  Uses dev_DefaultConnection  →  dev_azuresql
ASPNETCORE_ENVIRONMENT = Production   →  Uses prod_DefaultConnection  →  prod_azuresql
```

---

## Azure App Service Configuration

### For Development Slot

Go to **Azure Portal → Your App Service → Configuration → Connection strings**

Add:
- **Name:** `dev_DefaultConnection`
- **Value:** `Server=your-dev-server.database.windows.net;Database=your-dev-db;User Id=...;Password=...;Encrypt=true;TrustServerCertificate=false;`
- **Type:** SQLAzure

Also set Environment Variable:
- Go to **Configuration → Application settings** and add:
  ```
  ASPNETCORE_ENVIRONMENT = Development
  ```

### For Production Slot

Go to **Azure Portal → Your App Service → Configuration → Connection strings**

Add:
- **Name:** `prod_DefaultConnection`
- **Value:** `Server=your-prod-server.database.windows.net;Database=your-prod-db;User Id=...;Password=...;Encrypt=true;TrustServerCertificate=false;`
- **Type:** SQLAzure

Also set Environment Variable:
- Go to **Configuration → Application settings** and add:
  ```
  ASPNETCORE_ENVIRONMENT = Production
  ```

---

## Local Development

For local development, update **appsettings.Development.json**:

```json
{
  "ConnectionStrings": {
    "dev_DefaultConnection": "Server=(local);Database=VueApp1Db;Trusted_Connection=true;"
  }
}
```

---

## Code Logic

**Program.cs:**
```csharp
string connectionStringKey = builder.Environment.IsProduction() 
    ? "prod_DefaultConnection" 
    : "dev_DefaultConnection";
```

**DapperService:**
```csharp
string connectionStringKey = environment.IsProduction() 
    ? "prod_DefaultConnection" 
    : "dev_DefaultConnection";
    
_connectionString = configuration.GetConnectionString(connectionStringKey);
```

---

## Testing

### Test Dev Connection
```bash
dotnet run
# Should use dev_DefaultConnection
```

### Test Prod Connection
```bash
ASPNETCORE_ENVIRONMENT=Production dotnet run
# Should use prod_DefaultConnection (will fail locally if you don't have it configured)
```

---

## Common Issues

❌ **Error:** "Connection string 'dev_DefaultConnection' is not found"
✅ **Fix:** Ensure connection string is added to appsettings.Development.json or Azure portal

❌ **Error:** App using wrong database
✅ **Fix:** Check that ASPNETCORE_ENVIRONMENT is set correctly in Azure portal

❌ **Error:** Cannot connect to prod database
✅ **Fix:** Verify firewall rules allow your IP/Azure App Service to connect

