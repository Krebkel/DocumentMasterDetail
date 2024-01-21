using Contracts;

namespace Positions.Services;

public interface IPositionService
{
    /// <summary>
    /// Создать позицию
    /// </summary>
    Task<PositionCreationResult> CreatePositionAsync(CreatePositionRequest position, CancellationToken cancellationToken);

    /// <summary>
    /// Обновить позицию
    /// </summary>
    Task<Position> UpdatePositionAsync(UpdatePositionRequest position, CancellationToken ct);

    /// <summary>
    /// Удалить позицию
    /// </summary>
    Task DeletePositionAsync(int id, CancellationToken cancellationToken);

    /// <summary>
    /// Получить список позиций по Id документа
    /// </summary>
    Task<Position[]> GetPositionsByInvoiceIdAsync(int invoiceId);
}

public class UpdatePositionRequest
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Number { get; set; }
    public decimal? Value { get; set; }
}

public class CreatePositionRequest
{
    public required string Name { get; set; }
    public required string Number { get; set; }
    public required decimal Sum { get; set; }
    public required int InvoiceId { get; set; }
}