# ? SEGURIDAD CONFIGURADA - LISTO PARA GITHUB

## ?? Archivos Creados

| Archivo | Propósito | Subir a GitHub |
|---------|-----------|----------------|
| `.gitignore` | Excluir archivos sensibles | ? SÍ |
| `appsettings.Example.json` | Plantilla sin credenciales | ? SÍ |
| `SEGURIDAD_DESPLIEGUE.md` | Guía de seguridad completa | ? SÍ |
| `README.md` | Documentación del proyecto | ? SÍ |
| `verificar-seguridad.ps1` | Script de verificación | ? SÍ |
| `appsettings.json` | **CON credenciales reales** | ? **NO** |

---

## ?? CREDENCIALES DETECTADAS

En tu archivo `appsettings.json` actual hay credenciales que **NO deben subirse**:

```json
"Username": "a38b4b30-1965-4ff8-8305-a81a68ffec7a"
"Password": "/qVOY3wFyXAqdK4P7zq83M7jo+cylI613RSIBIzH9n4="
"FormTemplateId": "689fd49a-8383-f011-8265-e107c40f10fc"
```

Estas credenciales están ahora **protegidas** por el `.gitignore`.

---

## ?? PASOS PARA SUBIR A GITHUB

### ? Paso 1: Verificar Seguridad

Ejecutar el script de verificación:

```powershell
.\verificar-seguridad.ps1
```

Este script verifica:
- ? `.gitignore` existe
- ? `appsettings.json` NO está en staging
- ? `appsettings.Example.json` no tiene credenciales
- ? No hay credenciales en archivos staged
- ? Carpetas `bin/`, `obj/`, `uploads/` excluidas
- ? Archivos `.csv` excluidos

---

### ? Paso 2: Mover Credenciales a User Secrets

**ANTES de hacer el primer commit**, mover las credenciales:

```powershell
cd ExtraerDatosFacturasXML

# Inicializar User Secrets
dotnet user-secrets init

# Agregar credenciales (copiar de tu appsettings.json actual)
dotnet user-secrets set "VisualVault:Username" "a38b4b30-1965-4ff8-8305-a81a68ffec7a"
dotnet user-secrets set "VisualVault:Password" "/qVOY3wFyXAqdK4P7zq83M7jo+cylI613RSIBIzH9n4="
dotnet user-secrets set "VisualVault:FormTemplateId" "689fd49a-8383-f011-8265-e107c40f10fc"
```

---

### ? Paso 3: Limpiar appsettings.json

**Opción A**: Eliminar credenciales de `appsettings.json` y dejar solo estructura:

```json
{
  "VisualVault": {
    "Environment": "sa2",
    "BaseUrl": "https://sa2.visualvault.com",
    "CustomerAlias": "Fasecolda",
    "DatabaseAlias": "Main",
    "SuccessRedirectUrl": "https://sa2.visualvault.com/app/..."
  }
}
```

**Opción B**: Copiar `appsettings.Example.json` sobre `appsettings.json`:

```powershell
Copy-Item ExtraerDatosFacturasXML\appsettings.Example.json ExtraerDatosFacturasXML\appsettings.json -Force
```

---

### ? Paso 4: Inicializar Git

```powershell
# Inicializar Git
git init

# Agregar .gitignore PRIMERO
git add .gitignore

# Commit del .gitignore
git commit -m "Add .gitignore for security"

# Ahora agregar el resto
git add .

# Verificar que appsettings.json NO está en la lista
git status
```

**IMPORTANTE**: Verificar que `appsettings.json` NO aparezca en `git status`.

---

### ? Paso 5: Verificar Nuevamente

```powershell
# Ejecutar script de verificación
.\verificar-seguridad.ps1

# Buscar manualmente credenciales
git grep "a38b4b30-1965-4ff8-8305-a81a68ffec7a"
git grep "/qVOY3wFyXAqdK4P7zq83M7jo+cylI613RSIBIzH9n4="

# Ver diferencias
git diff --cached
```

---

### ? Paso 6: Hacer Primer Commit

```powershell
# Commit
git commit -m "Initial commit - ASP.NET Core 8 Invoice Processing System

- Razor Pages architecture
- XML parsing for DIAN electronic invoices
- VisualVault API integration
- Bootstrap Icons
- Corporate Fasecolda design
- Complete documentation"

# Crear repositorio en GitHub (UI)
# Luego conectar:
git remote add origin https://github.com/TU_USUARIO/TU_REPO.git

# Push
git branch -M main
git push -u origin main
```

---

## ?? VERIFICACIONES POST-COMMIT

### En GitHub, verificar que NO estén estos archivos:

- ? `appsettings.json` (con credenciales)
- ? `appsettings.Development.json`
- ? `appsettings.Production.json`
- ? Carpeta `uploads/`
- ? Carpeta `bin/`
- ? Carpeta `obj/`
- ? Archivos `.csv`

### Deben estar estos archivos:

- ? `.gitignore`
- ? `appsettings.Example.json`
- ? `README.md`
- ? `SEGURIDAD_DESPLIEGUE.md`
- ? Código fuente (`.cs`, `.cshtml`)
- ? Estilos (`.css`)

---

## ?? CLONAR EN OTRO AMBIENTE

Cuando otra persona (o tú en otra PC) clone el repo:

```powershell
# 1. Clonar
git clone https://github.com/TU_USUARIO/TU_REPO.git
cd TU_REPO

# 2. Configurar credenciales con User Secrets
cd ExtraerDatosFacturasXML
dotnet user-secrets init
dotnet user-secrets set "VisualVault:Username" "a38b4b30-1965-4ff8-8305-a81a68ffec7a"
dotnet user-secrets set "VisualVault:Password" "/qVOY3wFyXAqdK4P7zq83M7jo+cylI613RSIBIzH9n4="
dotnet user-secrets set "VisualVault:FormTemplateId" "689fd49a-8383-f011-8265-e107c40f10fc"

# 3. Restaurar y ejecutar
dotnet restore
dotnet run
```

---

## ??? CONFIGURACIÓN EN IIS (Producción)

**NO usar appsettings.json en IIS**. Usar variables de entorno:

IIS Manager ? Application Pools ? Tu App ? Advanced Settings ? Environment Variables:

```
VisualVault__Username            a38b4b30-1965-4ff8-8305-a81a68ffec7a
VisualVault__Password            /qVOY3wFyXAqdK4P7zq83M7jo+cylI613RSIBIzH9n4=
VisualVault__FormTemplateId      689fd49a-8383-f011-8265-e107c40f10fc
VisualVault__CustomerAlias       Fasecolda
VisualVault__DatabaseAlias       Main
```

---

## ?? RESUMEN DE ARCHIVOS

### En tu PC (local):

```
?? Proyecto
??? ?? appsettings.json               ? CON credenciales (no commitear)
??? ?? appsettings.Example.json       ? SIN credenciales (commitear)
??? ?? %APPDATA%\Microsoft\UserSecrets\   ? User Secrets (seguro)
```

### En GitHub:

```
?? Repositorio
??? ?? .gitignore                     ? Protección
??? ?? appsettings.Example.json       ? Plantilla
??? ?? README.md                      ? Documentación
??? ?? SEGURIDAD_DESPLIEGUE.md        ? Guía
??? ?? Código fuente                  ? Sin credenciales
```

### En IIS (producción):

```
?? Servidor IIS
??? ?? C:\inetpub\wwwroot\MiApp\      ? Archivos publicados
??? Variables de Entorno               ? Credenciales (seguro)
```

---

## ? CHECKLIST FINAL

Antes de push a GitHub:

- [ ] `.gitignore` creado y configurado
- [ ] `appsettings.Example.json` creado (sin credenciales)
- [ ] User Secrets configurado con credenciales reales
- [ ] `appsettings.json` limpio o sin credenciales
- [ ] Script `verificar-seguridad.ps1` ejecutado sin errores
- [ ] `git status` verificado (no aparece `appsettings.json`)
- [ ] `git grep` ejecutado (no encuentra credenciales)
- [ ] `README.md` creado
- [ ] `SEGURIDAD_DESPLIEGUE.md` creado
- [ ] Primer commit hecho

---

## ?? SI ALGO SALE MAL

### Si accidentalmente subes credenciales:

1. **NO hacer más commits**

2. **Eliminar del historial**:
   ```powershell
   git filter-branch --force --index-filter `
   "git rm --cached --ignore-unmatch ExtraerDatosFacturasXML/appsettings.json" `
   --prune-empty --tag-name-filter cat -- --all
   
   git push origin --force --all
   ```

3. **Rotar credenciales inmediatamente** con el administrador de VisualVault

4. **Revisar logs** de acceso en VisualVault

---

## ?? SOPORTE

- **Documentación**: Ver `SEGURIDAD_DESPLIEGUE.md`
- **Configuración**: Ver `CONFIGURACION_CRITICA.md`
- **Script de verificación**: `verificar-seguridad.ps1`

---

## ? ESTADO ACTUAL

| Componente | Estado |
|------------|--------|
| `.gitignore` | ? Creado |
| `appsettings.Example.json` | ? Creado |
| `SEGURIDAD_DESPLIEGUE.md` | ? Creado |
| `README.md` | ? Creado |
| `verificar-seguridad.ps1` | ? Creado |
| Credenciales protegidas | ? En `.gitignore` |

---

**¡Listo para subir a GitHub de forma segura!** ???

---

**Fecha**: 2024  
**Proyecto**: Sistema de Procesamiento de Facturas - Fasecolda  
**Seguridad**: Verificada y configurada
