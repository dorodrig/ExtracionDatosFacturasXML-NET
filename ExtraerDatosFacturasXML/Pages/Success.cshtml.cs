using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using ExtraerDatosFacturasXML.Models;

namespace ExtraerDatosFacturasXML.Pages;

public class SuccessModel : PageModel
{
    private readonly VisualVaultSettings _settings;

    public string Message { get; set; } = string.Empty;
    public List<string> FacturasProcesadas { get; set; } = new();
    public List<string> Errores { get; set; } = new();
    public string RedirectUrl { get; set; } = string.Empty;

    public SuccessModel(IOptions<VisualVaultSettings> settings)
    {
        _settings = settings.Value;
    }

    public void OnGet(string? message, string? facturas, string? errores)
    {
        Message = message ?? "Archivos procesados correctamente";
        RedirectUrl = _settings.SuccessRedirectUrl;

        if (!string.IsNullOrEmpty(facturas))
        {
            FacturasProcesadas = facturas.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        if (!string.IsNullOrEmpty(errores))
        {
            Errores = errores.Split('|', StringSplitOptions.RemoveEmptyEntries).ToList();
        }
    }
}
