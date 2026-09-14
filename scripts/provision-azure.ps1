# =========================================================
# PetCareAI - CLYVO VET
# Provisionamento da infraestrutura Azure (DevOps Tools & Cloud Computing)
# Opção: App Service + Banco PaaS (Azure SQL) - sem containers
# =========================================================

az login

$RESOURCE_GROUP    = "rg-petcareai-devops"
$LOCATION_SQL      = "centralus"
$LOCATION_APP      = "eastus"        # regiao diferente por limite de taxa (throttle) da assinatura educacional
$SQL_SERVER        = "sql-petcareai-566366v2"
$SQL_DB            = "petcareaidb"
$SQL_ADMIN_USER    = "petcareaiadmin"
$SQL_ADMIN_PASS    = "SUASENHA"      # NUNCA commitar a senha real
$APP_SERVICE_PLAN  = "asp-petcareai-devops"
$WEBAPP_NAME       = "app-petcareai-566366"

az provider register --namespace Microsoft.Sql
az provider register --namespace Microsoft.Web

az group create --name $RESOURCE_GROUP --location $LOCATION_SQL

az sql server create `
  --name $SQL_SERVER `
  --resource-group $RESOURCE_GROUP `
  --location $LOCATION_SQL `
  --admin-user $SQL_ADMIN_USER `
  --admin-password $SQL_ADMIN_PASS

az sql server firewall-rule create `
  --resource-group $RESOURCE_GROUP `
  --server $SQL_SERVER `
  --name AllowAzureServices `
  --start-ip-address 0.0.0.0 `
  --end-ip-address 0.0.0.0

az sql db create `
  --resource-group $RESOURCE_GROUP `
  --server $SQL_SERVER `
  --name $SQL_DB `
  --service-objective Basic

az appservice plan create `
  --name $APP_SERVICE_PLAN `
  --resource-group $RESOURCE_GROUP `
  --location $LOCATION_APP `
  --sku B1 `
  --is-linux

az webapp create `
  --name $WEBAPP_NAME `
  --resource-group $RESOURCE_GROUP `
  --plan $APP_SERVICE_PLAN `
  --runtime "DOTNETCORE:8.0"

az webapp config connection-string set `
  --name $WEBAPP_NAME `
  --resource-group $RESOURCE_GROUP `
  --connection-string-type SQLAzure `
  --settings DefaultConnection="Server=tcp:$SQL_SERVER.database.windows.net,1433;Database=$SQL_DB;User ID=$SQL_ADMIN_USER;Password=$SQL_ADMIN_PASS;Encrypt=true;"

# =========================================================
# Deploy da aplicacao (publica especificamente para Linux, evita
# problemas de compatibilidade de runtimes multiplataforma)
# =========================================================
# dotnet publish -c Release -r linux-x64 --self-contained false -o ./publish
# Add-Type -AssemblyName System.IO.Compression.FileSystem
# [System.IO.Compression.ZipFile]::CreateFromDirectory((Resolve-Path ./publish).Path, (Join-Path (Get-Location) "publish.zip"))
# az webapp deploy --resource-group $RESOURCE_GROUP --name $WEBAPP_NAME --src-path ./publish.zip --type zip --clean true
