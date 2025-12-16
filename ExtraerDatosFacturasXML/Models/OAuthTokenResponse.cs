namespace ExtraerDatosFacturasXML.Models;

/// <summary>
/// Respuesta del token OAuth de VisualVault
/// </summary>
public class OAuthTokenResponse
{
    public string access_token { get; set; } = string.Empty;
    public string token_type { get; set; } = string.Empty;
    public int expires_in { get; set; }
}
