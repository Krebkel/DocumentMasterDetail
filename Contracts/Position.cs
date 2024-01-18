namespace Contracts;

/// <summary>
/// Позиция проведенного документа
/// </summary>
public class Position : DatabaseEntity
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
    /// Документ
    /// </summary>
    public required Invoice Invoice { get; set; }
}