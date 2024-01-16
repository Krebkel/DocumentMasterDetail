namespace Contracts;

/// <summary>
/// Документ Master
/// </summary>
public class Invoice : DatabaseEntity
{
    /// <summary>
    /// Номер документа
    /// </summary>
    public string? Number { get; set; }

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