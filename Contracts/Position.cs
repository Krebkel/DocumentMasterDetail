namespace Contracts;

/// <summary>
/// Позиция проведенного документа
/// </summary>
public class Position : DatabaseEntity
{
    /// <summary>
    /// Наименование
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Количество
    /// </summary>
    public decimal? Quantity { get; set; }

    /// <summary>
    /// Сумма
    /// </summary>
    public decimal? Value { get; set; }

    /// <summary>
    /// Документ
    /// </summary>
    public Invoice? Invoice { get; set; }
}