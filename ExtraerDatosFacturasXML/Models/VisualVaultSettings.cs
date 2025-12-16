namespace ExtraerDatosFacturasXML.Models;

/// <summary>
/// Configuración para la conexión con VisualVault API
/// </summary>
public class VisualVaultSettings
{
    public string Environment { get; set; } = string.Empty;
    public string BaseUrl { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string CustomerAlias { get; set; } = string.Empty;
    public string DatabaseAlias { get; set; } = string.Empty;
    public string FormTemplateId { get; set; } = string.Empty;
    public string SuccessRedirectUrl { get; set; } = string.Empty;
}
