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
    Task<InvoiceUpdateResult> UpdateInvoiceAsync(UpdateInvoiceRequest invoice, CancellationToken cancellationToken);
    
    /// <summary>
    /// Получить все документы
    /// </summary>
    Task<Invoice[]> GetAllInvoicesAsync();
    
    /// <summary>
    /// Удалить документ
    /// </summary>
    Task DeleteInvoiceAsync(int id, CancellationToken cancellationToken);

    Task<Invoice> GetInvoiceAsync(int id);

}

public class UpdateInvoiceRequest
{
    public int Id { get; set; }
    public string? Number { get; set; }
    public DateTimeOffset? Date { get; set; }
    public string? Note { get; set; }
}

public class CreateInvoiceRequest
{
    public string Number { get; set; }
    public DateTimeOffset Date { get; set; }
    public string Note { get; set; }
}