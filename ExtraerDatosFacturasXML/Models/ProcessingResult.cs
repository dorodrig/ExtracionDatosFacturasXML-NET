namespace ExtraerDatosFacturasXML.Models;

/// <summary>
/// Resultado del procesamiento de facturas
/// </summary>
public class ProcessingResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public List<string> FacturasProcesadas { get; set; } = new();
    public List<string> Errores { get; set; } = new();
    public int TotalArchivos { get; set; }
    public int ArchivosExitosos { get; set; }
    public int ArchivosFallidos { get; set; }
}
