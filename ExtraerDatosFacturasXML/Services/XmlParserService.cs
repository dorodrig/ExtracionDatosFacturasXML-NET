using System.Globalization;
using System.Xml.Linq;
using ExtraerDatosFacturasXML.Models;

namespace ExtraerDatosFacturasXML.Services;

/// <summary>
/// Servicio para extraer datos de facturas electrónicas colombianas en formato XML
/// </summary>
public class XmlParserService
{
    private readonly ILogger<XmlParserService> _logger;

    public XmlParserService(ILogger<XmlParserService> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Extrae la información de una factura desde un archivo XML
    /// </summary>
    public async Task<FacturaElectronica?> ExtractInvoiceDataAsync(Stream xmlStream, string fileName)
    {
        try
        {
            var document = await XDocument.LoadAsync(xmlStream, LoadOptions.None, CancellationToken.None);
            
            // Namespace para los elementos XML de facturación electrónica colombiana
            XNamespace cbc = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2";
            XNamespace cac = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2";

            var attachedDoc = document.Root;
            if (attachedDoc == null)
            {
                _logger.LogWarning("El archivo {FileName} no tiene un elemento raíz válido", fileName);
                return null;
            }

            // Extraer ID de factura
            var facturaId = attachedDoc.Element(cbc + "ID")?.Value ?? string.Empty;
            
            // Extraer fecha
            var fechaStr = attachedDoc.Element(cbc + "IssueDate")?.Value ?? string.Empty;
            DateTime.TryParse(fechaStr, out var fecha);

            // Extraer attachment y procesar el XML embebido
            var attachment = attachedDoc.Element(cac + "Attachment");
            var externalReference = attachment?.Element(cac + "ExternalReference");
            var descriptionElement = externalReference?.Element(cbc + "Description")?.Value;

            decimal valorTotal = 0;
            string descripcion = string.Empty;

            if (!string.IsNullOrEmpty(descriptionElement))
            {
                // Parsear el XML embebido en Description
                var innerXml = XDocument.Parse(descriptionElement);
                XNamespace invoiceNs = "urn:oasis:names:specification:ubl:schema:xsd:Invoice-2";

                // Extraer valor total
                var legalMonetaryTotal = innerXml.Descendants(cac + "LegalMonetaryTotal").FirstOrDefault();
                var payableAmountElement = legalMonetaryTotal?.Element(cbc + "PayableAmount");
                
                if (payableAmountElement != null && decimal.TryParse(
                    payableAmountElement.Value, 
                    NumberStyles.Any, 
                    CultureInfo.InvariantCulture, 
                    out var parsedValue))
                {
                    valorTotal = parsedValue;
                }

                // Extraer descripción de forma recursiva
                descripcion = FindDescriptionRecursive(innerXml.Root, cbc);
            }

            // Extraer NIT del proveedor
            var senderParty = attachedDoc.Element(cac + "SenderParty");
            var partyTaxScheme = senderParty?.Element(cac + "PartyTaxScheme");
            var nitProveedor = partyTaxScheme?.Element(cbc + "CompanyID")?.Value ?? string.Empty;

            // Formatear el valor en pesos colombianos
            var valorFormateado = valorTotal.ToString("C0", new CultureInfo("es-CO"));

            var factura = new FacturaElectronica
            {
                NumeroFactura = facturaId,
                FechaEmision = fecha,
                ValorTotal = valorTotal,
                ValorTotalFormateado = valorFormateado,
                NitProveedor = nitProveedor,
                Descripcion = descripcion,
                NombreArchivoOriginal = fileName
            };

            _logger.LogInformation(
                "Factura procesada: {NumeroFactura}, Valor: {Valor}, NIT: {NIT}", 
                factura.NumeroFactura, 
                factura.ValorTotalFormateado, 
                factura.NitProveedor
            );

            return factura;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al procesar el archivo XML {FileName}", fileName);
            return null;
        }
    }

    /// <summary>
    /// Busca recursivamente el elemento Description en el XML
    /// </summary>
    private string FindDescriptionRecursive(XElement? element, XNamespace cbc)
    {
        if (element == null) return string.Empty;

        var description = element.Element(cbc + "Description");
        if (description != null)
        {
            return description.Value;
        }

        foreach (var child in element.Elements())
        {
            var result = FindDescriptionRecursive(child, cbc);
            if (!string.IsNullOrEmpty(result))
            {
                return result;
            }
        }

        return string.Empty;
    }
}
