namespace ExtraerDatosFacturasXML.Models;

/// <summary>
/// Modelo que representa una factura electrónica colombiana
/// </summary>
public class FacturaElectronica
{
    public string NumeroFactura { get; set; } = string.Empty;
    public DateTime FechaEmision { get; set; }
    public decimal ValorTotal { get; set; }
    public string ValorTotalFormateado { get; set; } = string.Empty;
    public string NitProveedor { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string NombreArchivoOriginal { get; set; } = string.Empty;
}
