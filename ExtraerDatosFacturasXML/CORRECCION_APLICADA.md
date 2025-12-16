# ? CORRECCIÓN APLICADA - Campos con Espacios

## ?? Problema Identificado

Los nombres de campos en VisualVault contienen **espacios y caracteres especiales**, pero en C# los nombres de propiedades no pueden tener espacios.

**Ejemplo**:
- VisualVault espera: `"Nit proveedor"` (con espacio)
- C# solo permite: `Nit_proveedor` (sin espacio)

---

## ? Solución Implementada

Se utilizó el atributo `[JsonPropertyName]` de `System.Text.Json` para mapear las propiedades C# a los nombres JSON con espacios.

### Código Actualizado

```csharp
using System.Text.Json.Serialization;

namespace ExtraerDatosFacturasXML.Models;

public class VisualVaultFormData
{
    // Campos sin espacios (no requieren atributo)
    [JsonPropertyName("txt_idfactura")]
    public string txt_idfactura { get; set; } = string.Empty;

    [JsonPropertyName("txt_numerodocumento")]
    public string txt_numerodocumento { get; set; } = string.Empty;

    [JsonPropertyName("txt_numvalorf")]
    public decimal txt_numvalorf { get; set; }

    [JsonPropertyName("txt_facturavalor")]
    public string txt_facturavalor { get; set; } = string.Empty;

    // Campos CON ESPACIOS (requieren atributo)
    [JsonPropertyName("Nit proveedor")]              // ? Con espacio
    public string Nit_proveedor { get; set; } = string.Empty;

    [JsonPropertyName("Fecha del documento")]        // ? Con espacios
    public string Fecha_del_documento { get; set; } = string.Empty;

    [JsonPropertyName("Observaciones")]
    public string Observaciones { get; set; } = string.Empty;

    [JsonPropertyName("ddl_tipo de documetno")]      // ? Con espacio (typo intencional)
    public string ddl_tipo_de_documetno { get; set; } = "Factura";

    [JsonPropertyName("Tipo de radicación")]         // ? Con espacios y acento
    public string Tipo_de_radicacion { get; set; } = "Electrónica";

    [JsonPropertyName("Estado General")]             // ? Con espacio
    public string Estado_General { get; set; } = "cargue realizado";
}
```

---

## ?? JSON que se Envía a VisualVault

```json
{
  "txt_idfactura": "FACTURA-001",
  "txt_numerodocumento": "FACTURA-001",
  "txt_numvalorf": 1000000,
  "txt_facturavalor": "$1.000.000",
  "Nit proveedor": "900123456-1",
  "Fecha del documento": "2024-01-15",
  "Observaciones": "Descripción de la factura",
  "ddl_tipo de documetno": "Factura",
  "Tipo de radicación": "Electrónica",
  "Estado General": "cargue realizado"
}
```

---

## ?? Campos Modificados

| Campo | Antes (Incorrecto) | Después (Correcto) |
|-------|-------------------|-------------------|
| NIT | `Nit_proveedor` | `"Nit proveedor"` |
| Fecha | `Fecha_del_documento` | `"Fecha del documento"` |
| Tipo Documento | `ddl_tipo_de_documetno` | `"ddl_tipo de documetno"` |
| Tipo Radicación | `Tipo_de_radicacion` | `"Tipo de radicación"` |
| Estado | `Estado_General` | `"Estado General"` |

---

## ?? Nota sobre "documetno"

El campo `"ddl_tipo de documetno"` tiene un **typo** (debería ser "documento").

Este typo está **preservado intencionalmente** porque así está configurado en VisualVault. No cambiar a menos que se corrija primero en VisualVault.

---

## ?? Cómo Probar

1. **Ejecutar la aplicación**:
   ```bash
   dotnet run
   ```

2. **Cargar un XML** de factura DIAN

3. **Verificar en VisualVault** que los datos se guardaron correctamente en los campos:
   - "Nit proveedor"
   - "Fecha del documento"
   - "Tipo de radicación"
   - "Estado General"

4. **Revisar logs** para confirmar que el formulario se creó exitosamente

---

## ?? Documentación

- **Detalles completos**: `CORRECCION_NOMBRES_CAMPOS.md`
- **Configuración**: `CONFIGURACION_CRITICA.md`
- **README general**: `README.md`

---

## ? Estado

- ? Corrección aplicada
- ? Compilación exitosa
- ? Documentación actualizada
- ? Listo para pruebas

---

**Fecha**: 2024  
**Archivo modificado**: `ExtraerDatosFacturasXML/Models/VisualVaultFormData.cs`  
**Versión**: 1.0.1
