using Contracts;

namespace Invoices.Services;

public interface IInvoiceService
{
    /// <summary>
    /// Создать документ
    /// </summary>
    Task<InvoiceCreationResult> CreateInvoiceAsync(CreateInvoiceRequest invoice, CancellationToken cancellationToken);

    /// <summary>
    /// Обновить документ
    /// </summary>
    Task UpdateInvoiceAsync(UpdateInvoiceRequest invoice, CancellationToken cancellationToken);
    
    /// <summary>
    /// Получить документ
    /// </summary>
    Task<Invoice> GetInvoiceAsync(int id);
    
    /// <summary>
    /// Удалить документ
    /// </summary>
    Task DeleteInvoiceAsync(int id, CancellationToken cancellationToken);
}

public class UpdateInvoiceRequest
{
    public int Id { get; }
    public string? Number { get; set; }
    public DateTimeOffset? Date { get; set; }
    public decimal? TotalAmount { get; set; }
    public List<Position>? Positions { get; set; }
}

public class CreateInvoiceRequest
{
    public int Id { get; set; }
    public string Number { get; set; }
    public DateTimeOffset Date { get; set; }
    public decimal TotalAmount { get; set; }
    public List<Position> Positions { get; set; }
}