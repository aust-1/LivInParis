using System.Text.Json;
using System.Xml.Serialization;
using LivInParisRoussilleTeynier.Domain.Models.Order;

namespace LivInParisRoussilleTeynier.Services.Services;

public static class ExportService
{
    public static string ExportOrdersToJson(IEnumerable<OrderTransaction> txs) =>
        JsonSerializer.Serialize(txs);

    public static string ExportOrdersToXml(IEnumerable<OrderTransaction> txs)
    {
        var xs = new XmlSerializer(typeof(List<OrderTransaction>));
        using var sw = new StringWriter();
        xs.Serialize(sw, txs.ToList());
        return sw.ToString();
    }
}

//TODO: faire tous les services pour les graphes et le reste de la logique métier
//TODO: normaliser l'export
