using ExtraerDatosFacturasXML.Models;
using ExtraerDatosFacturasXML.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ExtraerDatosFacturasXML.Pages;

public class IndexModel : PageModel
{
    private readonly FacturaProcessingService _processingService;
    private readonly ILogger<IndexModel> _logger;

    [BindProperty]
    public List<IFormFile> Files { get; set; } = new();

    public IndexModel(
        FacturaProcessingService processingService,
        ILogger<IndexModel> logger)
    {
        _processingService = processingService;
        _logger = logger;
    }

    public void OnGet()
    {
        // Mostrar formulario
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (Files == null || Files.Count == 0)
        {
            ModelState.AddModelError(string.Empty, "Por favor seleccione al menos un archivo XML");
            return Page();
        }

        try
        {
            var result = await _processingService.ProcessInvoicesAsync(Files);

            if (result.Success)
            {
                return RedirectToPage("Success", new
                {
                    message = result.Message,
                    facturas = string.Join(",", result.FacturasProcesadas),
                    errores = string.Join("|", result.Errores)
                });
            }
            else
            {
                ModelState.AddModelError(string.Empty, result.Message);
                foreach (var error in result.Errores)
                {
                    ModelState.AddModelError(string.Empty, error);
                }
                return Page();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error inesperado al procesar archivos");
            ModelState.AddModelError(string.Empty, "Ocurrió un error inesperado al procesar los archivos");
            return Page();
        }
    }
}
