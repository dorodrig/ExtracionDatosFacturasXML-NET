# Script de Publicación para IIS
# Ejecutar como Administrador

param(
    [string]$SiteName = "GRM_Facturas",
    [string]$AppPoolName = "GRM_Facturas_Pool",
    [string]$PhysicalPath = "C:\inetpub\wwwroot\GRM_Facturas",
    [int]$Port = 80
)

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  Script de Publicación - GRM Facturas" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# 1. Publicar la aplicación
Write-Host "Paso 1: Publicando la aplicación..." -ForegroundColor Yellow
dotnet publish -c Release -o ./publish
if ($LASTEXITCODE -ne 0) {
    Write-Host "? Error al publicar la aplicación" -ForegroundColor Red
    exit 1
}
Write-Host "? Aplicación publicada exitosamente" -ForegroundColor Green
Write-Host ""

# 2. Verificar IIS
Write-Host "Paso 2: Verificando IIS..." -ForegroundColor Yellow
$iisInstalled = Get-WindowsOptionalFeature -Online -FeatureName IIS-WebServerRole
if ($iisInstalled.State -ne "Enabled") {
    Write-Host "? IIS no está instalado. Por favor instalar IIS primero." -ForegroundColor Red
    exit 1
}
Write-Host "? IIS está instalado" -ForegroundColor Green
Write-Host ""

# 3. Importar módulo WebAdministration
Write-Host "Paso 3: Cargando módulo de IIS..." -ForegroundColor Yellow
Import-Module WebAdministration
Write-Host "? Módulo cargado" -ForegroundColor Green
Write-Host ""

# 4. Crear directorio físico
Write-Host "Paso 4: Creando directorio físico..." -ForegroundColor Yellow
if (!(Test-Path $PhysicalPath)) {
    New-Item -ItemType Directory -Path $PhysicalPath -Force
    Write-Host "? Directorio creado: $PhysicalPath" -ForegroundColor Green
} else {
    Write-Host "? Directorio ya existe: $PhysicalPath" -ForegroundColor Green
}
Write-Host ""

# 5. Copiar archivos publicados
Write-Host "Paso 5: Copiando archivos publicados..." -ForegroundColor Yellow
Copy-Item -Path "./publish/*" -Destination $PhysicalPath -Recurse -Force
Write-Host "? Archivos copiados" -ForegroundColor Green
Write-Host ""

# 6. Crear Application Pool
Write-Host "Paso 6: Configurando Application Pool..." -ForegroundColor Yellow
if (Test-Path "IIS:\AppPools\$AppPoolName") {
    Write-Host "??  Application Pool ya existe, eliminando..." -ForegroundColor Yellow
    Remove-WebAppPool -Name $AppPoolName
}
New-WebAppPool -Name $AppPoolName
Set-ItemProperty "IIS:\AppPools\$AppPoolName" -Name "managedRuntimeVersion" -Value ""
Write-Host "? Application Pool creado: $AppPoolName" -ForegroundColor Green
Write-Host ""

# 7. Crear sitio web
Write-Host "Paso 7: Configurando sitio web..." -ForegroundColor Yellow
if (Test-Path "IIS:\Sites\$SiteName") {
    Write-Host "??  Sitio web ya existe, eliminando..." -ForegroundColor Yellow
    Remove-Website -Name $SiteName
}
New-Website -Name $SiteName -PhysicalPath $PhysicalPath -Port $Port -ApplicationPool $AppPoolName
Write-Host "? Sitio web creado: $SiteName" -ForegroundColor Green
Write-Host ""

# 8. Configurar permisos
Write-Host "Paso 8: Configurando permisos..." -ForegroundColor Yellow
$acl = Get-Acl $PhysicalPath
$identity = "IIS AppPool\$AppPoolName"
$fileSystemRights = "ReadAndExecute"
$type = "Allow"
$rule = New-Object System.Security.AccessControl.FileSystemAccessRule($identity, $fileSystemRights, $type)
$acl.SetAccessRule($rule)
Set-Acl $PhysicalPath $acl
Write-Host "? Permisos configurados" -ForegroundColor Green
Write-Host ""

# 9. Reiniciar sitio
Write-Host "Paso 9: Iniciando sitio web..." -ForegroundColor Yellow
Start-Website -Name $SiteName
Write-Host "? Sitio web iniciado" -ForegroundColor Green
Write-Host ""

# 10. Mostrar información
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  ?? PUBLICACIÓN COMPLETADA" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "?? Información del sitio:" -ForegroundColor Yellow
Write-Host "   - Nombre: $SiteName" -ForegroundColor White
Write-Host "   - URL: http://localhost:$Port" -ForegroundColor White
Write-Host "   - Ruta física: $PhysicalPath" -ForegroundColor White
Write-Host "   - Application Pool: $AppPoolName" -ForegroundColor White
Write-Host ""
Write-Host "??  SIGUIENTE PASO IMPORTANTE:" -ForegroundColor Yellow
Write-Host "   Configurar las variables de entorno en el Application Pool:" -ForegroundColor White
Write-Host ""
Write-Host "   1. Abrir IIS Manager" -ForegroundColor Cyan
Write-Host "   2. Application Pools ? $AppPoolName ? Advanced Settings" -ForegroundColor Cyan
Write-Host "   3. Environment Variables ? Add" -ForegroundColor Cyan
Write-Host "   4. Agregar:" -ForegroundColor Cyan
Write-Host "      Name: VisualVault__Username" -ForegroundColor White
Write-Host "      Value: tu_usuario_real" -ForegroundColor White
Write-Host ""
Write-Host "      Name: VisualVault__Password" -ForegroundColor White
Write-Host "      Value: tu_password_real" -ForegroundColor White
Write-Host ""
Write-Host "?? Revisar CONFIGURACION_CRITICA.md para más detalles" -ForegroundColor Yellow
Write-Host ""
