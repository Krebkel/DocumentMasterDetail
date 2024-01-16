using Contracts;

namespace Invoices.Requests;

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
    public DateTime? Date { get; set; }

    /// <summary>
    /// Сумма по документу
    /// </summary>
    public decimal? TotalAmount { get; set; }

    /// <summary>
    /// Примечание
    /// </summary>
    public string? Note { get; set; }

    /// <summary>
    /// Позиции документа
    /// </summary>
    public List<Position>? Positions { get; set; }
}