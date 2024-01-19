using Contracts;

namespace Web.Requests;

public class AddPositionApiRequest
{
    /// <summary>
    /// Наименование
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Количество
    /// </summary>
    public required decimal Quantity { get; set; }

    /// <summary>
    /// Сумма
    /// </summary>
    public required decimal Value { get; set; }

    /// <summary>
    /// ID документа
    /// </summary>
    public required int InvoiceId { get; set; }
}