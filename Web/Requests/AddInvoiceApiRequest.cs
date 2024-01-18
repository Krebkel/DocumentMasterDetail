using Contracts;

namespace Web.Requests;

public class AddInvoiceApiRequest
{
    /// <summary>
    /// Идентификатор документа
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Номер документа
    /// </summary>
    public required string Number { get; set; }

    /// <summary>
    /// Дата проведения
    /// </summary>
    public required DateTime Date { get; set; }

    /// <summary>
    /// Сумма по документу
    /// </summary>
    public required decimal TotalAmount { get; set; }

    /// <summary>
    /// Примечание
    /// </summary>
    public required string Note { get; set; }

    /// <summary>
    /// Позиции документа
    /// </summary>
    public required List<Position> Positions { get; set; }
}