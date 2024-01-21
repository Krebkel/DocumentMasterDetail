using Contracts;

namespace Web.Requests;

public class AddPositionApiRequest
{
    /// <summary>
    /// Наименование
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Количество
    /// </summary>
    public required string Number { get; set; }

    /// <summary>
    /// Сумма
    /// </summary>
    public required decimal Sum { get; set; }
}