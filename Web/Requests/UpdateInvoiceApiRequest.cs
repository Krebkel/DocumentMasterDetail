namespace Web.Requests;

public class UpdateInvoiceApiRequest
{
    /// <summary>
    /// Номер документа
    /// </summary>
    public string? Number { get; set; }

    /// <summary>
    /// Дата проведения
    /// </summary>
    public DateTimeOffset? Date { get; set; }

    /// <summary>
    /// Примечание
    /// </summary>
    public string? Note { get; set; }
}