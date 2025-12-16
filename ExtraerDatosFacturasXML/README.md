# ?? Sistema de Procesamiento de Facturas Electrónicas - GRM

Aplicación ASP.NET Core 8 Razor Pages para procesar facturas electrónicas colombianas en formato XML y enviarlas a VisualVault.

## ?? Características

- ? **Sin almacenamiento temporal**: Procesamiento directo en memoria
- ? **Procesamiento concurrente**: Múltiples archivos XML simultáneamente
- ? **Caché de tokens OAuth**: Optimización de autenticación
- ? **Reintentos automáticos**: Manejo resiliente de errores HTTP
- ? **Validaciones robustas**: Seguridad en carga de archivos
- ? **Logging estructurado**: Trazabilidad completa
- ? **Diseño moderno**: Interfaz responsive y profesional

## ?? Configuración Inicial

### 1. Configurar Credenciales de VisualVault

Editar el archivo `appsettings.json`:

```json
{
  "VisualVault": {
    "Environment": "sa2",                    // ?? CAMBIAR: Ambiente (sa1, sa2, producción)
    "BaseUrl": "https://sa2.visualvault.com", // ?? CAMBIAR: URL base del ambiente
    "Username": "YOUR_USERNAME_HERE",         // ?? CAMBIAR: Usuario de API
    "Password": "YOUR_PASSWORD_HERE",         // ?? CAMBIAR: Contraseña de API
    "CustomerAlias": "Fasecolda",            // ?? CAMBIAR: Alias del cliente
    "DatabaseAlias": "Main",                 // ?? CAMBIAR: Base de datos
    "FormTemplateId": "ca1b3859-7088-ee11-825c-eacefa672ead", // ?? CAMBIAR: ID del template
    "SuccessRedirectUrl": "https://sa2.visualvault.com/app/Fasecolda/Main/FormDataDetails?Mode=ReadOnly&ReportID=889898a6-7931-ee11-aa2e-0eb3a1cc4944" // ?? CAMBIAR: URL de redirección
  }
}
```

### 2. Configuración para Producción (User Secrets - Recomendado)

Para mayor seguridad, usar User Secrets en desarrollo y Azure Key Vault en producción:

```bash
dotnet user-secrets init
dotnet user-secrets set "VisualVault:Username" "tu_usuario_real"
dotnet user-secrets set "VisualVault:Password" "tu_contraseña_real"
```

### 3. Variables de Entorno (IIS / Azure)

En producción, configurar como variables de entorno:

```
VisualVault__Username=tu_usuario
VisualVault__Password=tu_contraseña
VisualVault__Environment=produccion
VisualVault__BaseUrl=https://produccion.visualvault.com
```

## ?? Puntos Críticos de Configuración

### ?? OBLIGATORIO MODIFICAR:

1. **`VisualVault:Username`**: Usuario de API de VisualVault
2. **`VisualVault:Password`**: Contraseña de API de VisualVault
3. **`VisualVault:CustomerAlias`**: Alias del cliente en VisualVault (actualmente: "Fasecolda")
4. **`VisualVault:DatabaseAlias`**: Base de datos del cliente (actualmente: "Main")
5. **`VisualVault:FormTemplateId`**: GUID del formulario de facturas en VisualVault
6. **`VisualVault:Environment`**: Ambiente de trabajo (sa1, sa2, producción)
7. **`VisualVault:BaseUrl`**: URL completa del ambiente
8. **`VisualVault:SuccessRedirectUrl`**: URL a la que redirigir después del procesamiento exitoso

### ?? Cómo Obtener los Valores:

#### **FormTemplateId**:
1. Ir a VisualVault ? Admin ? Forms
2. Seleccionar el formulario de facturas
3. Copiar el GUID de la URL

#### **CustomerAlias y DatabaseAlias**:
1. Ver la URL de VisualVault: `https://sa2.visualvault.com/app/{CustomerAlias}/{DatabaseAlias}/...`

#### **Credenciales de API**:
1. Contactar al administrador de VisualVault
2. Solicitar credenciales con permisos de escritura en formularios

## ??? Arquitectura del Proyecto

```
ExtraerDatosFacturasXML/
??? Models/
?   ??? FacturaElectronica.cs         # Modelo de factura
?   ??? VisualVaultSettings.cs        # Configuración de VisualVault
?   ??? VisualVaultFormData.cs        # Estructura del formulario
?   ??? OAuthTokenResponse.cs         # Respuesta OAuth
?   ??? ProcessingResult.cs           # Resultado de procesamiento
??? Services/
?   ??? XmlParserService.cs           # Extracción de datos XML
?   ??? VisualVaultService.cs         # Comunicación con API
?   ??? FacturaProcessingService.cs   # Orquestación de procesamiento
??? Pages/
?   ??? Index.cshtml / .cs            # Página principal (upload)
?   ??? Success.cshtml / .cs          # Página de éxito
?   ??? Error.cshtml / .cs            # Página de error
??? appsettings.json                  # Configuración
??? Program.cs                        # Configuración de servicios
```

## ??? Despliegue en IIS

### 1. Publicar la Aplicación

```bash
dotnet publish -c Release -o ./publish
```

### 2. Configurar IIS

1. **Crear Application Pool**:
   - Name: `GRM_Facturas`
   - .NET CLR Version: `No Managed Code`
   - Managed Pipeline Mode: `Integrated`

2. **Crear Sitio Web**:
   - Site name: `GRM_Facturas`
   - Physical path: `C:\inetpub\wwwroot\GRM_Facturas`
   - Application pool: `GRM_Facturas`
   - Binding: `http://*:80` o `https://*:443`

3. **Instalar ASP.NET Core Hosting Bundle**:
   - Descargar de: https://dotnet.microsoft.com/download/dotnet/8.0
   - Instalar `ASP.NET Core Runtime` y `Hosting Bundle`

4. **Configurar Variables de Entorno**:
   - IIS Manager ? Application Pool ? Advanced Settings ? Environment Variables
   - Agregar las credenciales de VisualVault

### 3. Configurar HTTPS (Recomendado)

1. Obtener certificado SSL
2. Importar en IIS
3. Actualizar binding a HTTPS
4. Modificar `Program.cs` si es necesario

## ?? Desarrollo Local

### Prerequisitos

- .NET 8 SDK
- Visual Studio 2022 o VS Code
- Credenciales de VisualVault API

### Ejecutar

```bash
dotnet restore
dotnet build
dotnet run
```

Navegar a: `https://localhost:5001`

## ?? Dependencias NuGet

```xml
<PackageReference Include="Microsoft.Extensions.Http.Polly" Version="8.0.0" />
<PackageReference Include="Polly.Extensions.Http" Version="3.0.0" />
<PackageReference Include="System.Linq.Async" Version="6.0.1" />
```

## ?? Pruebas

### Archivos XML de Prueba

Los archivos XML deben seguir el estándar DIAN de facturación electrónica colombiana (UBL 2.1).

Estructura esperada:
```xml
<AttachedDocument>
  <cbc:ID>FACTURA-001</cbc:ID>
  <cbc:IssueDate>2024-01-15</cbc:IssueDate>
  <cac:SenderParty>
    <cac:PartyTaxScheme>
      <cbc:CompanyID>900123456-1</cbc:CompanyID>
    </cac:PartyTaxScheme>
  </cac:SenderParty>
  <cac:Attachment>
    <cac:ExternalReference>
      <cbc:Description>
        <!-- XML embebido con Invoice -->
      </cbc:Description>
    </cac:ExternalReference>
  </cac:Attachment>
</AttachedDocument>
```

## ?? Logs y Monitoreo

Los logs se escriben en:
- **Consola**: Durante desarrollo
- **Event Viewer**: En IIS (Windows Event Log)
- **Archivos**: Configurar en `appsettings.json` si es necesario

### Configurar File Logging (Opcional)

Instalar:
```bash
dotnet add package Serilog.AspNetCore
dotnet add package Serilog.Sinks.File
```

## ?? Solución de Problemas

### Error: "No se pudo obtener el token OAuth"

- ? Verificar credenciales en `appsettings.json`
- ? Verificar conectividad con VisualVault
- ? Verificar permisos de la cuenta API

### Error: "Error al crear formulario"

- ? Verificar `FormTemplateId` correcto
- ? Verificar `CustomerAlias` y `DatabaseAlias`
- ? Verificar estructura de datos del formulario

### Error: "No se pudo extraer información del XML"

- ? Verificar formato del XML (UBL 2.1 DIAN)
- ? Verificar namespaces correctos
- ? Revisar logs para detalles específicos

## ?? Soporte

Para problemas con:
- **VisualVault API**: Contactar soporte de VisualVault
- **Facturación DIAN**: Revisar documentación de facturación electrónica colombiana
- **Código**: Revisar logs en `Program.cs` y servicios

## ?? Licencia

Propiedad de GRM Document Management

---

**Autor**: Sistema migrado de Node.js a ASP.NET Core 8  
**Fecha**: 2024  
**Versión**: 1.0.0
