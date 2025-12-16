# ?? CORRECCIÓN IMPORTANTE - Nombres de Campos con Espacios

## ?? Cambio Realizado

Se corrigió el modelo `VisualVaultFormData.cs` para soportar los **nombres de campos con espacios** que utiliza VisualVault.

---

## ?? Nombres de Campos Correctos

### Antes (Incorrecto)
```csharp
public string Nit_proveedor { get; set; }          // ? Incorrecto
public string Fecha_del_documento { get; set; }    // ? Incorrecto
public string Tipo_de_radicacion { get; set; }     // ? Incorrecto
public string Estado_General { get; set; }         // ? Incorrecto
```

### Después (Correcto) ?
```csharp
[JsonPropertyName("Nit proveedor")]                // ? Con espacio
public string Nit_proveedor { get; set; }

[JsonPropertyName("Fecha del documento")]          // ? Con espacios
public string Fecha_del_documento { get; set; }

[JsonPropertyName("Tipo de radicación")]           // ? Con espacios y acento
public string Tipo_de_radicacion { get; set; }

[JsonPropertyName("Estado General")]               // ? Con espacio
public string Estado_General { get; set; }
```

---

## ?? Lista Completa de Campos

Estos son los **nombres exactos** que se envían a VisualVault:

| Nombre de Propiedad C# | Nombre en VisualVault (JSON) | Notas |
|------------------------|------------------------------|-------|
| `txt_idfactura` | `txt_idfactura` | Sin cambios |
| `txt_numerodocumento` | `txt_numerodocumento` | Sin cambios |
| `txt_numvalorf` | `txt_numvalorf` | Sin cambios |
| `txt_facturavalor` | `txt_facturavalor` | Sin cambios |
| `Nit_proveedor` | `Nit proveedor` | ?? CON ESPACIO |
| `Fecha_del_documento` | `Fecha del documento` | ?? CON ESPACIOS |
| `Observaciones` | `Observaciones` | Sin cambios |
| `ddl_tipo_de_documetno` | `ddl_tipo de documetno` | ?? CON ESPACIO |
| `Tipo_de_radicacion` | `Tipo de radicación` | ?? CON ESPACIOS Y ACENTO |
| `Estado_General` | `Estado General` | ?? CON ESPACIO |

---

## ?? Cómo Funciona

### Atributo `[JsonPropertyName]`

Este atributo de `System.Text.Json` permite mapear propiedades C# (que no pueden tener espacios) a nombres JSON con espacios:

```csharp
[JsonPropertyName("Nit proveedor")]        // Nombre que se envía en JSON
public string Nit_proveedor { get; set; } // Nombre de la propiedad en C#
```

Cuando el objeto se serializa a JSON, produce:

```json
{
  "Nit proveedor": "900123456-1",
  "Fecha del documento": "2024-01-15",
  "Tipo de radicación": "Electrónica",
  "Estado General": "cargue realizado"
}
```

---

## ? Validación

Para verificar que los nombres son correctos:

1. **En VisualVault**: Ir a Admin ? Forms ? Formulario de Facturas
2. **Ver los campos**: Los nombres exactos deben ser:
   - `Nit proveedor` (con espacio)
   - `Fecha del documento` (con espacios)
   - `Tipo de radicación` (con espacios y acento)
   - `Estado General` (con espacio)
   - `ddl_tipo de documetno` (con espacio, note el typo "documetno")

3. **Si los nombres NO coinciden**: Modificar los valores de `[JsonPropertyName("...")]`

---

## ?? IMPORTANTE

### ?? Typo en "documetno"

Note que el campo `ddl_tipo de documetno` tiene un **typo** (debería ser "documento" pero es "documetno").

Este typo está **INTENCIONALMENTE** preservado porque así está nombrado en VisualVault.

**NO cambiar** a menos que se corrija primero en VisualVault.

---

## ?? Prueba de Serialización

El siguiente código JSON es lo que se envía a VisualVault:

```json
{
  "txt_idfactura": "FACTURA-001",
  "txt_numerodocumento": "FACTURA-001",
  "txt_numvalorf": 1000000,
  "txt_facturavalor": "$1.000.000",
  "Nit proveedor": "900123456-1",
  "Fecha del documento": "2024-01-15",
  "Observaciones": "Factura de servicios profesionales",
  "ddl_tipo de documetno": "Factura",
  "Tipo de radicación": "Electrónica",
  "Estado General": "cargue realizado"
}
```

---

## ?? Referencia del Código Node.js Original

En el código Node.js original, esto se manejaba así:

```javascript
const postData = JSON.stringify({
    'txt_idfactura': fact,
    'txt_numerodocumento': fact,
    'txt_numvalorf': val,
    'txt_facturavalor': val1,
    'Nit proveedor': nit,              // ? Con espacio
    'Fecha del documento': fechac,     // ? Con espacios
    'Observaciones': descr,
    'ddl_tipo de documetno': 'Factura', // ? Con espacio
    'Tipo de radicación': 'Electrónica', // ? Con espacios y acento
    'Estado General': 'cargue realizado' // ? Con espacio
});
```

En C#, no podemos usar espacios directamente en nombres de propiedades, por eso usamos `[JsonPropertyName]`.

---

## ?? Si Necesitas Cambiar los Nombres de Campos

Si en el futuro los nombres de los campos cambian en VisualVault:

1. **NO cambiar** los nombres de las propiedades C# (ej: `Nit_proveedor`)
2. **SÍ cambiar** el valor de `[JsonPropertyName("...")]`

**Ejemplo**:

Si en VisualVault cambian `"Nit proveedor"` a `"NIT_Proveedor"`:

```csharp
[JsonPropertyName("NIT_Proveedor")]        // ? Cambiar solo esto
public string Nit_proveedor { get; set; } // ? NO cambiar esto
```

---

## ? Ventajas de Este Enfoque

1. ? **Compatibilidad**: Los nombres en C# siguen las convenciones
2. ? **Flexibilidad**: Podemos mapear a cualquier nombre JSON
3. ? **Mantenibilidad**: Cambios en VisualVault solo requieren modificar el atributo
4. ? **Claridad**: El código documenta explícitamente el mapeo

---

**Última actualización**: 2024  
**Archivo**: `ExtraerDatosFacturasXML/Models/VisualVaultFormData.cs`
