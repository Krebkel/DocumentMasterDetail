namespace Web.Responses;

public class ApiInvoice
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public int Id { get; set; }
    
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
    public string? Note { get; set; }
    
    /// <summary>
    /// Сумма
    /// </summary>
    public decimal Sum { get; set; }
}