using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using ExtraerDatosFacturasXML.Models;
using Microsoft.Extensions.Options;

namespace ExtraerDatosFacturasXML.Services;

/// <summary>
/// Servicio para interactuar con la API de VisualVault
/// </summary>
public class VisualVaultService
{
    private readonly HttpClient _httpClient;
    private readonly VisualVaultSettings _settings;
    private readonly ILogger<VisualVaultService> _logger;
    private string? _cachedToken;
    private DateTime _tokenExpiration;

    public VisualVaultService(
        HttpClient httpClient, 
        IOptions<VisualVaultSettings> settings,
        ILogger<VisualVaultService> logger)
    {
        _httpClient = httpClient;
        _settings = settings.Value;
        _logger = logger;
        _httpClient.BaseAddress = new Uri(_settings.BaseUrl);
    }

    /// <summary>
    /// Obtiene el token de autenticación OAuth2 (con caché)
    /// </summary>
    public async Task<string?> GetAuthTokenAsync()
    {
        // Si el token está en caché y no ha expirado, retornarlo
        if (!string.IsNullOrEmpty(_cachedToken) && DateTime.UtcNow < _tokenExpiration)
        {
            return _cachedToken;
        }

        try
        {
            var credentials = Convert.ToBase64String(
                Encoding.UTF8.GetBytes($"{_settings.Username}:{_settings.Password}")
            );

            var request = new HttpRequestMessage(HttpMethod.Post, "/oauth/token");
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", credentials);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var formData = new Dictionary<string, string>
            {
                { "username", _settings.Username },
                { "password", _settings.Password },
                { "grant_type", "password" }
            };

            request.Content = new FormUrlEncodedContent(formData);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonSerializer.Deserialize<OAuthTokenResponse>(responseContent);

            if (tokenResponse != null && !string.IsNullOrEmpty(tokenResponse.access_token))
            {
                _cachedToken = tokenResponse.access_token;
                _tokenExpiration = DateTime.UtcNow.AddSeconds(tokenResponse.expires_in - 60); // Renovar 1 min antes
                
                _logger.LogInformation("Token OAuth obtenido exitosamente");
                return _cachedToken;
            }

            _logger.LogError("No se pudo obtener el token de autenticación");
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener el token OAuth");
            return null;
        }
    }

    /// <summary>
    /// Crea un formulario en VisualVault con los datos de la factura
    /// </summary>
    public async Task<bool> CreateFormAsync(FacturaElectronica factura)
    {
        try
        {
            var token = await GetAuthTokenAsync();
            if (string.IsNullOrEmpty(token))
            {
                _logger.LogError("No se pudo obtener el token para crear el formulario");
                return false;
            }

            var formData = new VisualVaultFormData
            {
                txt_idfactura = factura.NumeroFactura,
                txt_numerodocumento = factura.NumeroFactura,
                txt_numvalorf = factura.ValorTotal,
                txt_facturavalor = factura.ValorTotalFormateado,
                Nit_proveedor = factura.NitProveedor,
                Fecha_del_documento = factura.FechaEmision.ToString("yyyy-MM-dd"),
                Observaciones = factura.Descripcion
            };

            var url = $"/api/v1/{_settings.CustomerAlias}/{_settings.DatabaseAlias}/formtemplates/{_settings.FormTemplateId}/forms";
            
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            request.Content = new StringContent(
                JsonSerializer.Serialize(formData), 
                Encoding.UTF8, 
                "application/json"
            );

            var response = await _httpClient.SendAsync(request);
            
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                _logger.LogInformation(
                    "Formulario creado exitosamente para factura {NumeroFactura}. Respuesta: {Response}", 
                    factura.NumeroFactura,
                    responseContent
                );
                return true;
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError(
                    "Error al crear formulario para factura {NumeroFactura}. Status: {Status}, Error: {Error}", 
                    factura.NumeroFactura,
                    response.StatusCode,
                    errorContent
                );
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Excepción al crear formulario para factura {NumeroFactura}", factura.NumeroFactura);
            return false;
        }
    }
}
