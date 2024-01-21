using Contracts;

namespace Web.Requests;

public class UpdatePositionApiRequest
{
    /// <summary>
    /// Наименование
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Количество
    /// </summary>
    public string? Number { get; set; }
    
    /// <summary>
    /// Сумма
    /// </summary>
    public decimal? Sum { get; set; }
}