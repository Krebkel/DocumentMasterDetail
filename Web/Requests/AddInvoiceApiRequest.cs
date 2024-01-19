namespace Web.Requests;

public class AddInvoiceApiRequest
{
    /// <summary>
    /// Номер документа
    /// </summary>
    public required string Number { get; set; }

    /// <summary>
    /// Дата проведения
    /// </summary>
    public required DateTimeOffset Date { get; set; }

    /// <summary>
    /// Примечание
    /// </summary>
    public required string Note { get; set; }
}