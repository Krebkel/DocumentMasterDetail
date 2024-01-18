using Contracts;

namespace Web.Requests;

public class UpdatePositionApiRequest
{
    /// <summary>
    /// Идентификатор позиции
    /// </summary>
    public int Id { get; set; }

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