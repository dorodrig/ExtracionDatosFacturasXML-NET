using ExtraerDatosFacturasXML.Models;

namespace ExtraerDatosFacturasXML.Services;

/// <summary>
/// Servicio orquestador para el procesamiento completo de facturas
/// </summary>
public class FacturaProcessingService
{
    private readonly XmlParserService _xmlParser;
    private readonly VisualVaultService _visualVault;
    private readonly ILogger<FacturaProcessingService> _logger;
    private readonly int _maxConcurrentProcessing;

    public FacturaProcessingService(
        XmlParserService xmlParser,
        VisualVaultService visualVault,
        IConfiguration configuration,
        ILogger<FacturaProcessingService> logger)
    {
        _xmlParser = xmlParser;
        _visualVault = visualVault;
        _logger = logger;
        _maxConcurrentProcessing = configuration.GetValue<int>("FileUpload:MaxConcurrentProcessing", 5);
    }

    /// <summary>
    /// Procesa múltiples archivos XML de facturas
    /// </summary>
    public async Task<ProcessingResult> ProcessInvoicesAsync(IEnumerable<IFormFile> files)
    {
        var result = new ProcessingResult
        {
            TotalArchivos = files.Count()
        };

        if (!files.Any())
        {
            result.Success = false;
            result.Message = "No se recibieron archivos para procesar";
            return result;
        }

        _logger.LogInformation("Iniciando procesamiento de {Count} archivo(s)", result.TotalArchivos);

        // Procesar archivos con límite de concurrencia
        var semaphore = new SemaphoreSlim(_maxConcurrentProcessing);
        var tasks = files.Select(async file =>
        {
            await semaphore.WaitAsync();
            try
            {
                return await ProcessSingleFileAsync(file);
            }
            finally
            {
                semaphore.Release();
            }
        });

        var processingResults = await Task.WhenAll(tasks);

        // Consolidar resultados
        foreach (var fileResult in processingResults)
        {
            if (fileResult.Success)
            {
                result.ArchivosExitosos++;
                if (!string.IsNullOrEmpty(fileResult.NumeroFactura))
                {
                    result.FacturasProcesadas.Add(fileResult.NumeroFactura);
                }
            }
            else
            {
                result.ArchivosFallidos++;
                if (!string.IsNullOrEmpty(fileResult.ErrorMessage))
                {
                    result.Errores.Add($"{fileResult.FileName}: {fileResult.ErrorMessage}");
                }
            }
        }

        result.Success = result.ArchivosExitosos > 0;
        result.Message = result.Success
            ? $"Procesamiento completado: {result.ArchivosExitosos} exitoso(s), {result.ArchivosFallidos} fallido(s)"
            : "No se pudo procesar ningún archivo";

        _logger.LogInformation(
            "Procesamiento finalizado: {Exitosos} exitosos, {Fallidos} fallidos",
            result.ArchivosExitosos,
            result.ArchivosFallidos
        );

        return result;
    }

    /// <summary>
    /// Procesa un único archivo XML
    /// </summary>
    private async Task<FileProcessingResult> ProcessSingleFileAsync(IFormFile file)
    {
        var result = new FileProcessingResult
        {
            FileName = file.FileName
        };

        try
        {
            // Validar extensión
            if (!file.FileName.EndsWith(".xml", StringComparison.OrdinalIgnoreCase))
            {
                result.Success = false;
                result.ErrorMessage = "El archivo no tiene extensión .xml";
                _logger.LogWarning("Archivo rechazado: {FileName} - No es XML", file.FileName);
                return result;
            }

            // Validar tamaño (opcional, por si lo necesitas)
            if (file.Length == 0)
            {
                result.Success = false;
                result.ErrorMessage = "El archivo está vacío";
                _logger.LogWarning("Archivo rechazado: {FileName} - Vacío", file.FileName);
                return result;
            }

            // Parsear XML sin guardar en disco
            FacturaElectronica? factura;
            using (var stream = file.OpenReadStream())
            {
                factura = await _xmlParser.ExtractInvoiceDataAsync(stream, file.FileName);
            }

            if (factura == null)
            {
                result.Success = false;
                result.ErrorMessage = "No se pudo extraer información del XML";
                return result;
            }

            // Enviar a VisualVault
            var enviado = await _visualVault.CreateFormAsync(factura);
            
            if (enviado)
            {
                result.Success = true;
                result.NumeroFactura = factura.NumeroFactura;
                _logger.LogInformation("Factura {NumeroFactura} procesada exitosamente", factura.NumeroFactura);
            }
            else
            {
                result.Success = false;
                result.ErrorMessage = "Error al enviar datos a VisualVault";
            }

            return result;
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.ErrorMessage = $"Excepción: {ex.Message}";
            _logger.LogError(ex, "Error procesando archivo {FileName}", file.FileName);
            return result;
        }
    }

    /// <summary>
    /// Resultado del procesamiento de un archivo individual
    /// </summary>
    private class FileProcessingResult
    {
        public string FileName { get; set; } = string.Empty;
        public bool Success { get; set; }
        public string? NumeroFactura { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
