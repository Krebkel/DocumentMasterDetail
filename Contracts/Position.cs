namespace Contracts;

/// <summary>
/// Позиция проведенного документа
/// </summary>
public class Position : DatabaseEntity
{
    /// <summary>
    /// Номер
    /// </summary>
    public required string Number { get; set; }

    /// <summary>
    /// Наименование
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Сумма
    /// </summary>
    public required decimal Value { get; set; }

    /// <summary>
    /// Документ
    /// </summary>
    public required Invoice Invoice { get; set; }
}