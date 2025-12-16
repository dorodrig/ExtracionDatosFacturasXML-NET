# ?? Sistema de Procesamiento de Facturas Electrónicas - Fasecolda

Aplicación **ASP.NET Core 8 Razor Pages** para procesar facturas electrónicas colombianas (DIAN UBL 2.1) y enviarlas automáticamente a VisualVault.

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)
![C#](https://img.shields.io/badge/C%23-12.0-239120?logo=csharp)
![License](https://img.shields.io/badge/License-Proprietary-red)

---

## ?? Características

- ? **Procesamiento en memoria**: Sin almacenamiento temporal de archivos
- ? **Procesamiento concurrente**: Múltiples archivos XML simultáneamente
- ? **Caché de tokens OAuth**: Optimización de autenticación con VisualVault
- ? **Reintentos automáticos**: Manejo resiliente de errores HTTP con Polly
- ? **Validaciones robustas**: Seguridad en carga de archivos
- ? **Logging estructurado**: Trazabilidad completa con ILogger
- ? **Diseño corporativo**: Identidad visual de Fasecolda
- ? **Responsive design**: Compatible con desktop, tablet y móvil

---

## ?? Requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Credenciales de API de VisualVault
- Windows Server con IIS (para producción) o cualquier servidor compatible con ASP.NET Core

---

## ?? Instalación

### 1. Clonar el repositorio

```bash
git clone https://github.com/TU_USUARIO/TU_REPO.git
cd TU_REPO/ExtraerDatosFacturasXML
```

### 2. Configurar credenciales

**Opción A: User Secrets (Desarrollo - Recomendado)**

```bash
dotnet user-secrets init
dotnet user-secrets set "VisualVault:Username" "TU_USERNAME"
dotnet user-secrets set "VisualVault:Password" "TU_PASSWORD"
dotnet user-secrets set "VisualVault:FormTemplateId" "TU_FORM_TEMPLATE_ID"
```

**Opción B: Archivo de configuración (No commitear)**

Copiar `appsettings.Example.json` a `appsettings.json` y completar con tus credenciales:

```bash
cp appsettings.Example.json appsettings.json
# Editar appsettings.json con tus credenciales
```

### 3. Restaurar dependencias

```bash
dotnet restore
```

### 4. Ejecutar la aplicación

```bash
dotnet run
```

Navegar a: `https://localhost:5001`

---

## ?? Configuración

### Archivo: `appsettings.json`

```json
{
  "VisualVault": {
    "Environment": "sa2",
    "BaseUrl": "https://sa2.visualvault.com",
    "Username": "YOUR_USERNAME",
    "Password": "YOUR_PASSWORD",
    "CustomerAlias": "YOUR_CUSTOMER",
    "DatabaseAlias": "YOUR_DATABASE",
    "FormTemplateId": "YOUR_GUID",
    "SuccessRedirectUrl": "https://..."
  }
}
```

**?? IMPORTANTE**: 
- Nunca subir `appsettings.json` con credenciales reales a GitHub
- Ver `SEGURIDAD_DESPLIEGUE.md` para más detalles

---

## ?? Estructura del Proyecto

```
ExtraerDatosFacturasXML/
??? Models/                      # Modelos de datos
?   ??? FacturaElectronica.cs
?   ??? VisualVaultSettings.cs
?   ??? VisualVaultFormData.cs
??? Services/                    # Lógica de negocio
?   ??? XmlParserService.cs      # Parseo de XML DIAN
?   ??? VisualVaultService.cs    # Cliente API VisualVault
?   ??? FacturaProcessingService.cs
??? Pages/                       # Razor Pages
?   ??? Index.cshtml             # Upload de archivos
?   ??? Success.cshtml           # Página de éxito
?   ??? Error.cshtml             # Página de error
??? wwwroot/                     # Archivos estáticos
?   ??? css/
?       ??? fasecolda-styles.css # Estilos corporativos
??? appsettings.Example.json     # Plantilla de configuración
```

---

## ?? Despliegue

### IIS (Windows Server)

Ver guía completa en: [`SEGURIDAD_DESPLIEGUE.md`](SEGURIDAD_DESPLIEGUE.md)

```bash
# 1. Publicar
dotnet publish -c Release -o ./publish

# 2. Copiar a IIS
# 3. Configurar Application Pool
# 4. Configurar variables de entorno
```

### Azure App Service

```bash
# 1. Crear App Service
az webapp create --name mi-app --resource-group mi-rg

# 2. Configurar variables de entorno en Azure Portal
# Configuration ? Application Settings

# 3. Deploy
az webapp deployment source config-zip --src publish.zip
```

---

## ?? Documentación

| Documento | Descripción |
|-----------|-------------|
| [`CONFIGURACION_CRITICA.md`](CONFIGURACION_CRITICA.md) | Configuración paso a paso |
| [`SEGURIDAD_DESPLIEGUE.md`](SEGURIDAD_DESPLIEGUE.md) | Guía de seguridad y despliegue |
| [`IDENTIDAD_VISUAL.md`](IDENTIDAD_VISUAL.md) | Guía de diseño corporativo |
| [`BOOTSTRAP_ICONS_IMPLEMENTACION.md`](BOOTSTRAP_ICONS_IMPLEMENTACION.md) | Iconos utilizados |

---

## ?? Identidad Visual

La aplicación utiliza los colores corporativos oficiales de **Fasecolda**:

- **Navy**: `#1B3B6F` (Headers, títulos)
- **Azul**: `#0066CC` (Links, botones)
- **Verde**: `#7ACC00` (Acentos, CTAs)

Ver más en: [`IDENTIDAD_VISUAL.md`](IDENTIDAD_VISUAL.md)

---

## ?? Pruebas

```bash
# Ejecutar pruebas (si existen)
dotnet test

# Verificar compilación
dotnet build
```

---

## ?? Tecnologías Utilizadas

- **ASP.NET Core 8** - Framework web
- **Razor Pages** - Motor de vistas
- **C# 12** - Lenguaje de programación
- **System.Xml.Linq** - Parseo de XML
- **HttpClient + Polly** - Cliente HTTP resiliente
- **ILogger** - Logging estructurado
- **Bootstrap Icons** - Iconografía

---

## ?? Seguridad

### Datos Sensibles

Este proyecto NO incluye credenciales reales en el repositorio. Las credenciales deben configurarse mediante:

1. **User Secrets** (desarrollo)
2. **Variables de entorno** (IIS/Azure)
3. **Azure Key Vault** (producción cloud)

Ver: [`SEGURIDAD_DESPLIEGUE.md`](SEGURIDAD_DESPLIEGUE.md)

### Archivos Excluidos

Ver [`.gitignore`](.gitignore) para lista completa de archivos excluidos.

---

## ?? Solución de Problemas

### Error: "No se pudo obtener el token OAuth"
- Verificar credenciales en configuración
- Verificar conectividad con VisualVault
- Revisar logs para más detalles

### Error: "Error al crear formulario"
- Verificar `FormTemplateId` correcto
- Verificar `CustomerAlias` y `DatabaseAlias`
- Verificar nombres de campos en `VisualVaultFormData.cs`

Ver más en: [`CONFIGURACION_CRITICA.md`](CONFIGURACION_CRITICA.md)

---

## ?? Licencia

Este proyecto es propiedad de **GRM Document Management** y **Fasecolda**.

Todos los derechos reservados.

---

## ?? Autores

- **Proyecto original**: Node.js
- **Migración a .NET**: ASP.NET Core 8
- **Cliente**: Fasecolda (Federación de Aseguradores de Colombia)
- **Desarrollador**: GRM Document Management

---

## ?? Soporte

Para problemas o consultas:

1. Revisar la documentación en la carpeta del proyecto
2. Contactar al administrador de VisualVault
3. Revisar logs de la aplicación

---

## ?? Historial de Versiones

### v1.0.0 (2024)
- ? Migración completa de Node.js a ASP.NET Core 8
- ? Procesamiento en memoria (sin archivos temporales)
- ? Procesamiento paralelo
- ? Caché de tokens OAuth
- ? Diseño corporativo Fasecolda
- ? Bootstrap Icons
- ? Documentación completa

---

## ?? Roadmap

- [ ] Pruebas unitarias
- [ ] Pruebas de integración
- [ ] Dashboard de estadísticas
- [ ] Soporte para más tipos de documentos
- [ ] API REST para integración con otros sistemas

---

**? Si este proyecto te fue útil, no olvides dar una estrella!**

---

**Nota**: Este README asume que ya has leído y seguido las instrucciones en [`SEGURIDAD_DESPLIEGUE.md`](SEGURIDAD_DESPLIEGUE.md) para configurar correctamente las credenciales.
