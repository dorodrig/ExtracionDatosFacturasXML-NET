# ?? GUÍA DE SEGURIDAD Y DESPLIEGUE

## ?? INFORMACIÓN SENSIBLE - NO SUBIR A GITHUB

Este documento explica cómo manejar información sensible y dónde configurarla en cada ambiente.

---

## ?? DATOS SENSIBLES QUE NO DEBEN SUBIRSE A GITHUB

### ? Archivos que NO deben estar en GitHub:

1. **`appsettings.json`** - Contiene credenciales reales
2. **`appsettings.Production.json`** - Credenciales de producción
3. **`appsettings.Development.json`** - Credenciales de desarrollo
4. **Carpeta `uploads/`** - Archivos temporales
5. **Archivos `.csv`** - Datos generados
6. **Carpetas `bin/` y `obj/`** - Compilados

### ? Archivos que SÍ deben estar en GitHub:

1. **`appsettings.Example.json`** - Plantilla sin credenciales
2. **Código fuente** (`.cs`, `.cshtml`)
3. **Documentación** (`.md`)
4. **Estilos** (`.css`)
5. **`.gitignore`** - Configuración de Git

---

## ?? INFORMACIÓN SENSIBLE A CONFIGURAR

### 1. **Credenciales de VisualVault API**

#### Dónde están actualmente (NO SUBIR):
```json
"Username": "a38b4b30-1965-4ff8-8305-a81a68ffec7a",
"Password": "/qVOY3wFyXAqdK4P7zq83M7jo+cylI613RSIBIzH9n4="
```

#### Dónde configurarlas en cada ambiente:

##### ??? **Desarrollo Local**

**Opción A: User Secrets (Recomendado)**

```bash
cd ExtraerDatosFacturasXML
dotnet user-secrets init
dotnet user-secrets set "VisualVault:Username" "a38b4b30-1965-4ff8-8305-a81a68ffec7a"
dotnet user-secrets set "VisualVault:Password" "/qVOY3wFyXAqdK4P7zq83M7jo+cylI613RSIBIzH9n4="
dotnet user-secrets set "VisualVault:FormTemplateId" "689fd49a-8383-f011-8265-e107c40f10fc"
```

**Opción B: Archivo Local (no commitear)**

Crear `appsettings.Development.json` (está en .gitignore):
```json
{
  "VisualVault": {
    "Username": "a38b4b30-1965-4ff8-8305-a81a68ffec7a",
    "Password": "/qVOY3wFyXAqdK4P7zq83M7jo+cylI613RSIBIzH9n4=",
    "FormTemplateId": "689fd49a-8383-f011-8265-e107c40f10fc"
  }
}
```

---

##### ?? **IIS (Producción)**

**1. Variables de Entorno del Application Pool**

IIS Manager ? Application Pools ? Tu App Pool ? Advanced Settings ? Environment Variables:

```
Variable                          Valor
VisualVault__Username            a38b4b30-1965-4ff8-8305-a81a68ffec7a
VisualVault__Password            /qVOY3wFyXAqdK4P7zq83M7jo+cylI613RSIBIzH9n4=
VisualVault__FormTemplateId      689fd49a-8383-f011-8265-e107c40f10fc
VisualVault__CustomerAlias       Fasecolda
VisualVault__DatabaseAlias       Main
```

**Nota**: Usar doble guion bajo `__` para niveles anidados.

**2. Archivo appsettings.Production.json (en servidor, no en GitHub)**

Crear directamente en el servidor:
```json
{
  "VisualVault": {
    "Username": "a38b4b30-1965-4ff8-8305-a81a68ffec7a",
    "Password": "/qVOY3wFyXAqdK4P7zq83M7jo+cylI613RSIBIzH9n4=",
    "FormTemplateId": "689fd49a-8383-f011-8265-e107c40f10fc"
  }
}
```

---

##### ?? **Azure App Service**

**Configuration ? Application Settings**:

```
Name                              Value
VisualVault__Username            a38b4b30-1965-4ff8-8305-a81a68ffec7a
VisualVault__Password            /qVOY3wFyXAqdK4P7zq83M7jo+cylI613RSIBIzH9n4=
VisualVault__FormTemplateId      689fd49a-8383-f011-8265-e107c40f10fc
```

O usar **Azure Key Vault** (más seguro):
1. Crear Key Vault
2. Agregar secrets
3. Configurar Managed Identity
4. Referenciar en la aplicación

---

## ?? CHECKLIST ANTES DE SUBIR A GITHUB

### ? Verificaciones Obligatorias:

- [ ] `.gitignore` creado y configurado
- [ ] `appsettings.json` agregado a `.gitignore`
- [ ] `appsettings.Example.json` creado (sin credenciales)
- [ ] Credenciales movidas a User Secrets o variables de entorno
- [ ] Verificar que NO hay credenciales en el código
- [ ] Carpeta `uploads/` agregada a `.gitignore`
- [ ] Archivos `.csv` excluidos
- [ ] Carpetas `bin/` y `obj/` excluidas

### ?? Comando de Verificación:

```bash
# Ver qué archivos se van a subir
git status

# Ver contenido de archivos staged
git diff --cached

# Buscar texto sensible (ejecutar antes de commit)
git grep -i "password"
git grep -i "a38b4b30-1965-4ff8-8305-a81a68ffec7a"
```

---

## ?? PROCESO DE DESPLIEGUE SEGURO

### 1?? **Primer Commit a GitHub**

```bash
# Inicializar Git (si no lo has hecho)
git init

# Agregar .gitignore PRIMERO
git add .gitignore

# Commit del .gitignore
git commit -m "Add .gitignore"

# Ahora agregar el resto (sin appsettings.json)
git add .

# Verificar que appsettings.json NO está en la lista
git status

# Commit
git commit -m "Initial commit - ASP.NET Core application"

# Conectar con GitHub
git remote add origin https://github.com/TU_USUARIO/TU_REPO.git

# Push
git push -u origin main
```

---

### 2?? **Clonar en Otro Ambiente**

```bash
# Clonar el repositorio
git clone https://github.com/TU_USUARIO/TU_REPO.git

# Navegar al proyecto
cd TU_REPO/ExtraerDatosFacturasXML

# Copiar appsettings.Example.json a appsettings.json
cp appsettings.Example.json appsettings.json

# Editar appsettings.json con credenciales reales
nano appsettings.json
# o usar User Secrets:
dotnet user-secrets init
dotnet user-secrets set "VisualVault:Username" "TU_USERNAME"
dotnet user-secrets set "VisualVault:Password" "TU_PASSWORD"

# Restaurar y ejecutar
dotnet restore
dotnet run
```

---

### 3?? **Despliegue a IIS**

```bash
# 1. Publicar la aplicación
dotnet publish -c Release -o ./publish

# 2. Copiar archivos publicados al servidor IIS
# (sin appsettings.json con credenciales)

# 3. En el servidor IIS, crear appsettings.Production.json
# con las credenciales reales

# 4. O configurar variables de entorno en el Application Pool
```

---

## ?? ESTRUCTURA DE ARCHIVOS RECOMENDADA

```
?? Repositorio GitHub (Público/Privado)
??? ?? .gitignore                     ? Sí subir
??? ?? appsettings.Example.json       ? Sí subir (sin credenciales)
??? ?? README.md                      ? Sí subir
??? ?? CONFIGURACION_CRITICA.md       ? Sí subir
??? ?? SEGURIDAD_DESPLIEGUE.md        ? Sí subir
??? ?? Models/                        ? Sí subir (código)
??? ?? Services/                      ? Sí subir (código)
??? ?? Pages/                         ? Sí subir (código)
??? ?? wwwroot/                       ? Sí subir (css, js)
?
??? ?? appsettings.json               ? NO subir (en .gitignore)
??? ?? appsettings.Development.json   ? NO subir (en .gitignore)
??? ?? appsettings.Production.json    ? NO subir (en .gitignore)
??? ?? uploads/                       ? NO subir (en .gitignore)
??? ?? bin/                           ? NO subir (en .gitignore)
??? ?? obj/                           ? NO subir (en .gitignore)
??? ?? datos.csv                      ? NO subir (en .gitignore)
```

---

## ?? CREDENCIALES POR AMBIENTE

### Desarrollo (Tu PC)

**Método**: User Secrets o appsettings.Development.json (no commitear)

**Credenciales**:
```
Username:        a38b4b30-1965-4ff8-8305-a81a68ffec7a
Password:        /qVOY3wFyXAqdK4P7zq83M7jo+cylI613RSIBIzH9n4=
FormTemplateId:  689fd49a-8383-f011-8265-e107c40f10fc
Environment:     sa2
BaseUrl:         https://sa2.visualvault.com
```

---

### Staging/QA

**Método**: Variables de entorno o archivo en servidor

**Credenciales**: Solicitar al administrador de VisualVault

---

### Producción

**Método**: Variables de entorno en IIS o Azure Key Vault

**Credenciales**: Solicitar al administrador de VisualVault

---

## ??? MEJORES PRÁCTICAS DE SEGURIDAD

### 1. **Nunca hardcodear credenciales**
```csharp
? MAL:
var username = "a38b4b30-1965-4ff8-8305-a81a68ffec7a";

? BIEN:
var username = _configuration["VisualVault:Username"];
```

### 2. **Usar User Secrets en desarrollo**
```bash
dotnet user-secrets set "Key" "Value"
```

### 3. **Usar Variables de Entorno en producción**
```bash
# IIS, Azure, Docker, etc.
VisualVault__Username=valor
```

### 4. **Usar Azure Key Vault en la nube**
```csharp
builder.Configuration.AddAzureKeyVault(...)
```

### 5. **Rotar credenciales periódicamente**
- Cambiar passwords cada 90 días
- Usar diferentes credenciales por ambiente

---

## ?? VERIFICAR QUE NO HAY FUGAS

### Antes de cada commit:

```bash
# Buscar patrones sensibles
git grep -i "password"
git grep -i "secret"
git grep -i "a38b4b30-1965"

# Ver diferencias
git diff

# Ver archivos staged
git diff --cached
```

### Herramientas recomendadas:

1. **git-secrets** (AWS)
   ```bash
   git secrets --install
   git secrets --register-aws
   ```

2. **GitHub Secret Scanning** (automático en repos privados)

3. **GitGuardian** (detecta secretos en commits)

---

## ?? CONTACTO EN CASO DE EXPOSICIÓN

Si accidentalmente subes credenciales:

1. **Inmediatamente**: Eliminar del repositorio
   ```bash
   git filter-branch --force --index-filter \
   "git rm --cached --ignore-unmatch appsettings.json" \
   --prune-empty --tag-name-filter cat -- --all
   ```

2. **Notificar** al administrador de VisualVault

3. **Rotar credenciales** inmediatamente

4. **Revisar logs** de acceso en VisualVault

---

## ? CHECKLIST FINAL

Antes de hacer push a GitHub:

- [ ] `.gitignore` configurado
- [ ] `appsettings.json` en `.gitignore`
- [ ] `appsettings.Example.json` creado
- [ ] Credenciales movidas a User Secrets
- [ ] `git status` verificado
- [ ] `git diff` revisado
- [ ] No hay archivos sensibles en staging
- [ ] README.md actualizado
- [ ] Documentación completa

---

## ?? RECURSOS ADICIONALES

- [ASP.NET Core Configuration](https://docs.microsoft.com/aspnet/core/fundamentals/configuration/)
- [Safe storage of app secrets](https://docs.microsoft.com/aspnet/core/security/app-secrets)
- [Azure Key Vault](https://azure.microsoft.com/services/key-vault/)
- [Git Secrets](https://github.com/awslabs/git-secrets)

---

**Fecha**: 2024  
**Versión**: 1.0.0  
**Proyecto**: Sistema de Procesamiento de Facturas - Fasecolda
