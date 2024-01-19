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
    Task UpdatePositionAsync(UpdatePositionRequest position, CancellationToken ct);

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
    public decimal? Quantity { get; set; }
    public decimal? Value { get; set; }
}

public class CreatePositionRequest
{
    public required string Name { get; set; }
    public required decimal Quantity { get; set; }
    public required decimal Value { get; set; }
    public required int InvoiceId { get; set; }
}