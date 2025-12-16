# Script de Verificación de Seguridad - EJECUTAR ANTES DE COMMIT
# PowerShell

Write-Host "======================================" -ForegroundColor Cyan
Write-Host "  VERIFICACIÓN DE SEGURIDAD - GitHub" -ForegroundColor Cyan
Write-Host "======================================" -ForegroundColor Cyan
Write-Host ""

$erroresEncontrados = 0

# 1. Verificar que .gitignore existe
Write-Host "1. Verificando .gitignore..." -ForegroundColor Yellow
if (Test-Path ".gitignore") {
    Write-Host "   ? .gitignore existe" -ForegroundColor Green
} else {
    Write-Host "   ? .gitignore NO existe - CREAR ANTES DE CONTINUAR" -ForegroundColor Red
    $erroresEncontrados++
}

# 2. Verificar que appsettings.json NO está en staging
Write-Host "2. Verificando appsettings.json..." -ForegroundColor Yellow
$gitStatus = git status --porcelain
if ($gitStatus -match "appsettings\.json") {
    Write-Host "   ? appsettings.json está en staging - REMOVER INMEDIATAMENTE" -ForegroundColor Red
    Write-Host "      Ejecutar: git rm --cached ExtraerDatosFacturasXML/appsettings.json" -ForegroundColor Yellow
    $erroresEncontrados++
} else {
    Write-Host "   ? appsettings.json no está en staging" -ForegroundColor Green
}

# 3. Verificar que appsettings.Example.json existe
Write-Host "3. Verificando appsettings.Example.json..." -ForegroundColor Yellow
if (Test-Path "ExtraerDatosFacturasXML/appsettings.Example.json") {
    Write-Host "   ? appsettings.Example.json existe" -ForegroundColor Green
    
    # Verificar que NO tiene credenciales reales
    $exampleContent = Get-Content "ExtraerDatosFacturasXML/appsettings.Example.json" -Raw
    if ($exampleContent -match "a38b4b30-1965-4ff8-8305-a81a68ffec7a") {
        Write-Host "   ? appsettings.Example.json contiene credenciales reales" -ForegroundColor Red
        $erroresEncontrados++
    } else {
        Write-Host "   ? appsettings.Example.json no contiene credenciales reales" -ForegroundColor Green
    }
} else {
    Write-Host "   ? appsettings.Example.json NO existe - CREAR ANTES DE CONTINUAR" -ForegroundColor Red
    $erroresEncontrados++
}

# 4. Buscar credenciales en archivos staged
Write-Host "4. Buscando credenciales en archivos staged..." -ForegroundColor Yellow
$credencialesSensibles = @(
    "a38b4b30-1965-4ff8-8305-a81a68ffec7a",
    "/qVOY3wFyXAqdK4P7zq83M7jo+cylI613RSIBIzH9n4=",
    "689fd49a-8383-f011-8265-e107c40f10fc"
)

foreach ($credencial in $credencialesSensibles) {
    $resultado = git grep $credencial $(git diff --cached --name-only) 2>$null
    if ($resultado) {
        Write-Host "   ? Credencial encontrada en archivos staged: $credencial" -ForegroundColor Red
        Write-Host "      Archivos: $resultado" -ForegroundColor Yellow
        $erroresEncontrados++
    }
}

if ($erroresEncontrados -eq 0) {
    Write-Host "   ? No se encontraron credenciales en archivos staged" -ForegroundColor Green
}

# 5. Verificar carpetas bin/ y obj/
Write-Host "5. Verificando carpetas de compilación..." -ForegroundColor Yellow
if ($gitStatus -match "bin/|obj/") {
    Write-Host "   ? Carpetas bin/ u obj/ en staging - Verificar .gitignore" -ForegroundColor Red
    $erroresEncontrados++
} else {
    Write-Host "   ? Carpetas bin/ y obj/ excluidas" -ForegroundColor Green
}

# 6. Verificar carpeta uploads/
Write-Host "6. Verificando carpeta uploads/..." -ForegroundColor Yellow
if ($gitStatus -match "uploads/") {
    Write-Host "   ? Carpeta uploads/ en staging - Verificar .gitignore" -ForegroundColor Red
    $erroresEncontrados++
} else {
    Write-Host "   ? Carpeta uploads/ excluida" -ForegroundColor Green
}

# 7. Verificar archivos .csv
Write-Host "7. Verificando archivos CSV..." -ForegroundColor Yellow
if ($gitStatus -match "\.csv$") {
    Write-Host "   ? Archivos CSV en staging - Verificar .gitignore" -ForegroundColor Red
    $erroresEncontrados++
} else {
    Write-Host "   ? Archivos CSV excluidos" -ForegroundColor Green
}

# 8. Verificar documentación
Write-Host "8. Verificando documentación..." -ForegroundColor Yellow
$documentosRequeridos = @(
    "README.md",
    "ExtraerDatosFacturasXML/SEGURIDAD_DESPLIEGUE.md",
    "ExtraerDatosFacturasXML/CONFIGURACION_CRITICA.md"
)

foreach ($doc in $documentosRequeridos) {
    if (Test-Path $doc) {
        Write-Host "   ? $doc existe" -ForegroundColor Green
    } else {
        Write-Host "   ??  $doc NO existe (recomendado)" -ForegroundColor Yellow
    }
}

# Resumen
Write-Host ""
Write-Host "======================================" -ForegroundColor Cyan
Write-Host "  RESUMEN DE VERIFICACIÓN" -ForegroundColor Cyan
Write-Host "======================================" -ForegroundColor Cyan
Write-Host ""

if ($erroresEncontrados -eq 0) {
    Write-Host "? TODO CORRECTO - SEGURO PARA HACER COMMIT" -ForegroundColor Green
    Write-Host ""
    Write-Host "Comandos sugeridos:" -ForegroundColor Cyan
    Write-Host "  git add ." -ForegroundColor White
    Write-Host "  git commit -m 'Initial commit - ASP.NET Core application'" -ForegroundColor White
    Write-Host "  git push -u origin main" -ForegroundColor White
} else {
    Write-Host "? $erroresEncontrados ERROR(ES) ENCONTRADO(S)" -ForegroundColor Red
    Write-Host ""
    Write-Host "NO HACER COMMIT HASTA RESOLVER LOS ERRORES" -ForegroundColor Red
    Write-Host ""
    Write-Host "Acciones recomendadas:" -ForegroundColor Yellow
    Write-Host "  1. Revisar los errores arriba" -ForegroundColor White
    Write-Host "  2. Corregir cada uno" -ForegroundColor White
    Write-Host "  3. Ejecutar este script nuevamente" -ForegroundColor White
    
    exit 1
}
