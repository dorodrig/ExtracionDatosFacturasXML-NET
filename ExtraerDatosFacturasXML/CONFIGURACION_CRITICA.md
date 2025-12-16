# ?? PUNTOS CRÍTICOS DE CONFIGURACIÓN - CHECKLIST

## ?? ANTES DE EJECUTAR - CONFIGURACIÓN OBLIGATORIA

### ?? 1. ARCHIVO: `appsettings.json`

**Ubicación**: `ExtraerDatosFacturasXML/appsettings.json`

#### ?? Credenciales de Autenticación
```json
"Username": "YOUR_USERNAME_HERE",    // ?? CAMBIAR por usuario de API de VisualVault
"Password": "YOUR_PASSWORD_HERE",    // ?? CAMBIAR por contraseña de API de VisualVault
```

**Cómo obtener**:
- Contactar al administrador de VisualVault
- Solicitar credenciales con permisos de escritura en formularios

---

#### ?? Configuración de Ambiente
```json
"Environment": "sa2",                           // ?? CAMBIAR según ambiente
"BaseUrl": "https://sa2.visualvault.com",      // ?? CAMBIAR según ambiente
```

**Opciones comunes**:
- Desarrollo: `sa1`, `https://sa1.visualvault.com`
- Staging: `sa2`, `https://sa2.visualvault.com`
- Producción: Consultar con administrador

---

#### ?? Configuración del Cliente
```json
"CustomerAlias": "Fasecolda",       // ?? VERIFICAR o CAMBIAR
"DatabaseAlias": "Main",            // ?? VERIFICAR o CAMBIAR
```

**Cómo obtener**:
1. Abrir VisualVault en el navegador
2. Ver la URL: `https://{ambiente}.visualvault.com/app/{CustomerAlias}/{DatabaseAlias}/...`
3. Copiar los valores de CustomerAlias y DatabaseAlias

**Ejemplo**:
```
URL: https://sa2.visualvault.com/app/Fasecolda/Main/FormDataDetails
     CustomerAlias = "Fasecolda"
     DatabaseAlias = "Main"
```

---

#### ?? ID del Formulario
```json
"FormTemplateId": "ca1b3859-7088-ee11-825c-eacefa672ead",  // ?? CAMBIAR
```

**Cómo obtener**:
1. Ir a VisualVault ? Admin ? Forms
2. Buscar el formulario de "Facturas" o "Radicación de Documentos"
3. Abrir el formulario
4. Copiar el GUID de la URL

**Ejemplo de URL**:
```
https://sa2.visualvault.com/admin/Fasecolda/Main/formtemplates/ca1b3859-7088-ee11-825c-eacefa672ead
                                                                 ? Este es el FormTemplateId
```

---

#### ?? URL de Redirección
```json
"SuccessRedirectUrl": "https://sa2.visualvault.com/app/Fasecolda/Main/FormDataDetails?Mode=ReadOnly&ReportID=889898a6-7931-ee11-aa2e-0eb3a1cc4944"  // ?? CAMBIAR
```

**Cómo obtener**:
1. Ir al reporte o vista donde quieres que lleguen después de procesar
2. Copiar la URL completa
3. Pegar en la configuración

---

### ?? 2. ARCHIVO: `appsettings.Production.json`

**Ubicación**: `ExtraerDatosFacturasXML/appsettings.Production.json`

**IMPORTANTE**: Este archivo se usa en producción (IIS). Modificar los mismos valores pero para el ambiente de producción.

?? **NO** incluir credenciales reales aquí. Usar variables de entorno en IIS.

---

## ?? SEGURIDAD - MÉTODOS RECOMENDADOS

### ? Opción 1: Variables de Entorno (IIS - RECOMENDADO)

En IIS Manager:
1. Clic derecho en el Application Pool ? Advanced Settings
2. Environment Variables ? Add
3. Agregar:
   ```
   VisualVault__Username = tu_usuario_real
   VisualVault__Password = tu_password_real
   ```

### ? Opción 2: User Secrets (Desarrollo Local)

En línea de comandos:
```bash
cd ExtraerDatosFacturasXML
dotnet user-secrets init
dotnet user-secrets set "VisualVault:Username" "tu_usuario"
dotnet user-secrets set "VisualVault:Password" "tu_password"
```

### ? Opción 3: Azure Key Vault (Producción Cloud)

Si se despliega en Azure, usar Azure Key Vault para almacenar las credenciales.

---

## ?? CAMPOS DEL FORMULARIO DE VISUALVAULT

**IMPORTANTE**: Los nombres de los campos en el código deben coincidir EXACTAMENTE con los del formulario en VisualVault.

### ?? Campos Actuales (Modelo: `VisualVaultFormData.cs`) ? CORREGIDO

```csharp
// Campos sin espacios (no requieren atributo especial)
txt_idfactura          ? ID de la factura
txt_numerodocumento    ? Número de documento (mismo que ID factura)
txt_numvalorf          ? Valor numérico de la factura
txt_facturavalor       ? Valor formateado (ej: "$1.000.000")
Observaciones          ? Descripción o notas

// Campos CON ESPACIOS (usan [JsonPropertyName])
[JsonPropertyName("Nit proveedor")]        ? NIT del proveedor
[JsonPropertyName("Fecha del documento")]  ? Fecha de emisión
[JsonPropertyName("ddl_tipo de documetno")]? Tipo de documento (fijo: "Factura")
[JsonPropertyName("Tipo de radicación")]   ? Tipo (fijo: "Electrónica")
[JsonPropertyName("Estado General")]       ? Estado (fijo: "cargue realizado")
```

### ?? Nombres Exactos que se Envían a VisualVault

| Propiedad C# | Nombre en VisualVault | Observación |
|--------------|----------------------|-------------|
| `txt_idfactura` | `txt_idfactura` | Sin espacios |
| `txt_numerodocumento` | `txt_numerodocumento` | Sin espacios |
| `txt_numvalorf` | `txt_numvalorf` | Sin espacios |
| `txt_facturavalor` | `txt_facturavalor` | Sin espacios |
| `Nit_proveedor` | `Nit proveedor` | ?? CON ESPACIO |
| `Fecha_del_documento` | `Fecha del documento` | ?? CON ESPACIOS |
| `Observaciones` | `Observaciones` | Sin espacios |
| `ddl_tipo_de_documetno` | `ddl_tipo de documetno` | ?? CON ESPACIO (typo intencional) |
| `Tipo_de_radicacion` | `Tipo de radicación` | ?? CON ESPACIOS Y ACENTO |
| `Estado_General` | `Estado General` | ?? CON ESPACIO |

### ?? Cómo Verificar los Nombres de los Campos

1. Ir a VisualVault ? Admin ? Forms
2. Abrir el formulario de facturas
3. Ver la definición de cada campo
4. Los nombres deben coincidir EXACTAMENTE (incluidos espacios y acentos)

### ?? Si los Nombres NO Coinciden

**Ubicación para modificar**: `ExtraerDatosFacturasXML/Models/VisualVaultFormData.cs`

**IMPORTANTE**: Solo modificar el valor del atributo `[JsonPropertyName("...")]`, NO el nombre de la propiedad C#.

Ejemplo de cambio:
```csharp
// SI en VisualVault el campo se llama "NIT del Proveedor" en lugar de "Nit proveedor"
[JsonPropertyName("NIT del Proveedor")]    // ? Cambiar solo esto
public string Nit_proveedor { get; set; } // ? NO cambiar esto
```

### ?? Documento de Referencia

Para más detalles sobre los nombres de campos con espacios, consultar:
**`CORRECCION_NOMBRES_CAMPOS.md`**

---

## ?? PRUEBA INICIAL

### 1. Verificar Configuración

Revisar el archivo `appsettings.json` y confirmar que todos los valores están actualizados.

### 2. Ejecutar Localmente

```bash
cd ExtraerDatosFacturasXML
dotnet run
```

Navegar a: `https://localhost:5001`

### 3. Probar con un XML de Ejemplo

1. Cargar un archivo XML de factura electrónica DIAN
2. Verificar que se procesa correctamente
3. Ir a VisualVault y verificar que el formulario se creó

### 4. Revisar Logs

Los logs se muestran en la consola durante la ejecución. Buscar:
```
? "Token OAuth obtenido exitosamente"
? "Factura procesada: {NumeroFactura}"
? "Formulario creado exitosamente"
```

---

## ?? SOLUCIÓN DE PROBLEMAS COMUNES

### ? Error: "No se pudo obtener el token OAuth"

**Causas posibles**:
- Username o Password incorrectos
- URL base incorrecta
- Falta de conectividad con VisualVault

**Solución**:
- Verificar credenciales en `appsettings.json`
- Probar acceso manual a VisualVault con esas credenciales
- Verificar que el BaseUrl sea correcto

---

### ? Error: "Error al crear formulario"

**Causas posibles**:
- FormTemplateId incorrecto
- CustomerAlias o DatabaseAlias incorrectos
- Nombres de campos no coinciden

**Solución**:
- Verificar FormTemplateId en VisualVault
- Verificar CustomerAlias y DatabaseAlias en la URL de VisualVault
- Comparar nombres de campos en `VisualVaultFormData.cs` con el formulario en VisualVault
- **NUEVO**: Verificar que los nombres con espacios usen correctamente `[JsonPropertyName]`

---

### ? Error: "No se pudo extraer información del XML"

**Causas posibles**:
- XML no sigue el estándar DIAN UBL 2.1
- XML corrupto o incompleto

**Solución**:
- Validar el XML con un validador DIAN
- Verificar que el XML tenga la estructura esperada

---

## ?? RESUMEN DE ARCHIVOS A MODIFICAR

| Archivo | Qué Cambiar | Prioridad |
|---------|-------------|-----------|
| `appsettings.json` | Credenciales, ambiente, FormTemplateId | ?? CRÍTICO |
| `appsettings.Production.json` | Configuración de producción | ?? IMPORTANTE |
| `VisualVaultFormData.cs` | Nombres de campos CON ESPACIOS (usar `[JsonPropertyName]`) | ?? VERIFICAR |

---

## ? CHECKLIST PRE-DESPLIEGUE

- [ ] Credenciales de VisualVault configuradas
- [ ] Environment y BaseUrl correctos para el ambiente
- [ ] CustomerAlias verificado
- [ ] DatabaseAlias verificado
- [ ] FormTemplateId obtenido de VisualVault
- [ ] SuccessRedirectUrl configurado
- [ ] Nombres de campos verificados con el formulario de VisualVault
- [ ] ? **NUEVO**: Verificar que campos con espacios usen `[JsonPropertyName]`
- [ ] Prueba local exitosa
- [ ] Variables de entorno configuradas en IIS (si aplica)
- [ ] Certificado SSL instalado (si aplica)

---

## ?? CONTACTOS NECESARIOS

**Información que necesitas del administrador de VisualVault**:
1. ? Usuario y contraseña de API
2. ? Ambiente a usar (sa1, sa2, producción)
3. ? URL base del ambiente
4. ? CustomerAlias del cliente
5. ? DatabaseAlias a usar
6. ? FormTemplateId del formulario de facturas
7. ? Permisos necesarios para la cuenta API
8. ? **NUEVO**: Nombres exactos de los campos (con espacios y caracteres especiales)

---

**Última actualización**: 2024  
**Versión**: 1.0.1 (Corregido soporte para campos con espacios)
