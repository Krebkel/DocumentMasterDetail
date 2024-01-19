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
    /// Получить все документы
    /// </summary>
    Task<Invoice[]> GetAllInvoicesAsync();
    
    /// <summary>
    /// Удалить документ
    /// </summary>
    Task DeleteInvoiceAsync(string number, CancellationToken cancellationToken);
}

public class UpdateInvoiceRequest
{
    public int Id { get; }
    public string? Number { get; set; }
    public DateTimeOffset? Date { get; set; }
    public string? Note { get; set; }

}

public class CreateInvoiceRequest
{
    public int Id { get; set; }
    public string Number { get; set; }
    public DateTimeOffset Date { get; set; }
    public string Note { get; set; }
}