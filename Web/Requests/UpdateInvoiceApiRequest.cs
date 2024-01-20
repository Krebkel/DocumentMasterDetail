using Contracts;

namespace Web.Requests;

public class UpdateInvoiceApiRequest
{
    /// <summary>
    /// Идентификатор документа
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Номер документа
    /// </summary>
    public string Number { get; set; }

    /// <summary>
    /// Дата проведения
    /// </summary>
    public DateTimeOffset? Date { get; set; }

    /// <summary>
    /// Примечание
    /// </summary>
    public string? Note { get; set; }

    /// <summary>
    /// Позиции документа
    /// </summary>
    public Position[] Positions { get; set; }
}