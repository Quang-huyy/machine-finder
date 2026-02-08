# Deployment Guide: Dev & Production Configuration

## Quick Summary

Your backend now supports **3 configuration files**:

1. **appsettings.json** - Base/Production defaults
2. **appsettings.Development.json** - Dev overrides (local SQL Server)
3. **appsettings.Production.json** - Production overrides (Cloud DB, strict CORS)

ASP.NET Core automatically loads the environment-specific file based on `ASPNETCORE_ENVIRONMENT`.

---

## Local Development

### Running locally:

```bash
# Runs with appsettings.Development.json
dotnet run
```

**Dev settings:**
- Connection: `Server=(local);Database=VueApp1Db;Trusted_Connection=true;`
- CORS: Allows `http://localhost:*` (all dev ports)
- Swagger: Enabled
- Logging: Debug level

---

## Production Deployment

### Option 1: Azure App Service (Recommended for .NET)

1. **Create Resource Group & SQL Database:**
   ```bash
   # Using Azure CLI
   az group create --name myResourceGroup --location eastus
   az sql server create --name myserver --resource-group myResourceGroup --admin-user dbadmin --admin-password "SecurePassword123!"
   az sql db create --resource-group myResourceGroup --server myserver --name VueApp1Db
   ```

2. **Create App Service:**
   ```bash
   az appservice plan create --name myAppServicePlan --resource-group myResourceGroup --sku B2
   az webapp create --name myapp --resource-group myResourceGroup --plan myAppServicePlan --runtime "dotnet:10"
   ```

3. **Set Environment Variables in Azure Portal:**
   - Go to **Configuration → Application settings** and add:
   
   ```
   ASPNETCORE_ENVIRONMENT = Production
   ConnectionStrings__DefaultConnection = Server=myserver.database.windows.net;Database=VueApp1Db;User Id=dbadmin;Password=YourSecurePassword;Encrypt=true;TrustServerCertificate=false;
   ```

4. **Deploy from Visual Studio or GitHub:**
   - Right-click project → **Publish**
   - Select **Azure App Service**
   - Follow the wizard

---

### Option 2: Docker Deployment (Any Cloud Provider)

Create a **Dockerfile** in VueApp1.Server root:

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

# Copy built app
COPY bin/Release/net10.0/publish/ .

# Expose port
EXPOSE 5000

# Set environment to Production
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://0.0.0.0:5000

ENTRYPOINT ["dotnet", "VueApp1.Server.dll"]
```

Build and run:
```bash
# Build Docker image
docker build -t vueapp1-backend:latest .

# Run with environment variables
docker run -e "ConnectionStrings__DefaultConnection=Server=your-db;Database=VueApp1Db;User Id=admin;Password=pass;" \
           -e "ASPNETCORE_ENVIRONMENT=Production" \
           -p 5000:5000 \
           vueapp1-backend:latest
```

Deploy to **Docker Hub, AWS ECR, or Google Container Registry**, then to:
- AWS ECS / Fargate
- Google Cloud Run
- Kubernetes (self-hosted or managed)

---

### Option 3: Traditional VPS (DigitalOcean, Linode, etc.)

1. **SSH into server:**
   ```bash
   ssh root@your-server-ip
   ```

2. **Install .NET 10 runtime:**
   ```bash
   wget https://dot.net/v1/dotnet-install.sh
   chmod +x dotnet-install.sh
   ./dotnet-install.sh --channel 10.0
   ```

3. **Deploy your app:**
   ```bash
   # Publish locally
   dotnet publish -c Release -o ./publish
   
   # Transfer to server
   scp -r ./publish root@your-server-ip:/opt/vueapp1/
   ```

4. **Create systemd service** (`/etc/systemd/system/vueapp1.service`):
   ```ini
   [Unit]
   Description=VueApp1 Backend
   After=network.target
   
   [Service]
   Type=notify
   User=www-data
   WorkingDirectory=/opt/vueapp1
   ExecStart=/root/.dotnet/dotnet /opt/vueapp1/VueApp1.Server.dll
   
   Environment="ASPNETCORE_ENVIRONMENT=Production"
   Environment="ASPNETCORE_URLS=http://0.0.0.0:5000"
   Environment="ConnectionStrings__DefaultConnection=Server=prod-db.example.com;Database=VueApp1Db;User Id=admin;Password=YourSecurePassword;"
   
   Restart=always
   RestartSec=10
   
   [Install]
   WantedBy=multi-user.target
   ```

5. **Start service:**
   ```bash
   systemctl daemon-reload
   systemctl enable vueapp1
   systemctl start vueapp1
   ```

---

## Environment Variable Reference

| Variable | Dev Value | Prod Value |
|----------|-----------|------------|
| `ASPNETCORE_ENVIRONMENT` | `Development` | `Production` |
| `ASPNETCORE_URLS` | `http://localhost:5000` | `http://0.0.0.0:5000` |
| `ConnectionStrings__DefaultConnection` | Local SQL Server | Cloud SQL Server |
| `AllowedOrigins` | `http://localhost:*` | Your frontend domain |
| `AllowSwagger` | `true` | `false` |

---

## Security Checklist for Production

- ✅ **Secrets in environment variables** (never in code)
- ✅ **HTTPS enabled** (`app.UseHttpsRedirection()` in Production)
- ✅ **Swagger disabled** (`AllowSwagger: false`)
- ✅ **CORS restricted** (only your frontend domain)
- ✅ **Database credentials encrypted** (use cloud provider's secret management)
- ✅ **Logging level reduced** (Warning/Error only)
- ✅ **Firewall rules** (only allow necessary ports)

---

## Testing Your Configuration

### Verify Dev settings:
```bash
dotnet run
# Should show: "Now listening on: http://localhost:5000"
# Swagger accessible at http://localhost:5000/openapi/v1.json
```

### Verify Production settings:
```bash
ASPNETCORE_ENVIRONMENT=Production dotnet run
# Should NOT have Swagger
# Should use HTTPS redirect
```

---

## Next Steps

1. **Choose your deployment platform** (I recommend Azure App Service for .NET)
2. **Update `appsettings.Production.json`** with your actual production DB server
3. **Store sensitive data** in the cloud provider's secret manager (not in appsettings)
4. **Test locally** first with all environments
5. **Deploy and monitor** using your cloud provider's monitoring tools
