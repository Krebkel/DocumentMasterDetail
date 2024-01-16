using Contracts;

namespace Invoices.Services;

public interface IInvoiceService
{
    /// <summary>
    /// Создать документ
    /// </summary>
    Task<Invoice> CreateInvoiceAsync(CreateInvoiceRequest invoice, CancellationToken cancellationToken);

    /// <summary>
    /// Обновить документ
    /// </summary>
    Task UpdateInvoiceAsync(UpdateInvoiceRequest invoice, CancellationToken ct);
}

public class UpdateInvoiceRequest
{
    public int Id { get; set; }
    public string? Number { get; set; }
    public DateTime? Date { get; set; }
    public decimal? TotalAmount { get; set; }
    public List<Position>? Positions { get; set; }
}

public class CreateInvoiceRequest
{
    public string? Number { get; set; }
    public DateTime? Date { get; set; }
    public decimal? TotalAmount { get; set; }
    public List<Position>? Positions { get; set; }
}