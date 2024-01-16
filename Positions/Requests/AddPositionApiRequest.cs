using Contracts;

namespace Positions.Requests;

public class AddPositionApiRequest
{
    /// <summary>
    /// Идентификатор позиции
    /// </summary>
    public int Id { get; set; }

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