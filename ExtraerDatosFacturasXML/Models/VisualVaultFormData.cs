using System.Text.Json.Serialization;

namespace ExtraerDatosFacturasXML.Models;

/// <summary>
/// Datos del formulario que se envía a VisualVault
/// IMPORTANTE: Los nombres de las propiedades JSON deben coincidir EXACTAMENTE con los campos de VisualVault
/// </summary>
public class VisualVaultFormData
{
    [JsonPropertyName("txt_idfactura")]
    public string txt_idfactura { get; set; } = string.Empty;

    [JsonPropertyName("txt_numerodocumento")]
    public string txt_numerodocumento { get; set; } = string.Empty;

    [JsonPropertyName("txt_numvalorf")]
    public decimal txt_numvalorf { get; set; }

    [JsonPropertyName("txt_facturavalor")]
    public string txt_facturavalor { get; set; } = string.Empty;

    [JsonPropertyName("Nit proveedor")]
    public string Nit_proveedor { get; set; } = string.Empty;

    [JsonPropertyName("Fecha del documento")]
    public string Fecha_del_documento { get; set; } = string.Empty;

    [JsonPropertyName("Observaciones")]
    public string Observaciones { get; set; } = string.Empty;

    [JsonPropertyName("ddl_tipo de documetno")]
    public string ddl_tipo_de_documetno { get; set; } = "Factura";

    [JsonPropertyName("Tipo de radicación")]
    public string Tipo_de_radicacion { get; set; } = "Electrónica";

    [JsonPropertyName("Estado General")]
    public string Estado_General { get; set; } = "cargue realizado";
}
