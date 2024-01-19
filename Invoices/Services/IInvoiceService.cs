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
    Task<Invoice> GetInvoiceAsync(string number);

    /// <summary>
    /// Получить все позиции для документа
    /// </summary>
    Task<List<Position>> GetAllPositionsForInvoiceAsync(string invoiceNumber);

    /// <summary>
    /// Получить все документы
    /// </summary>
    Task<List<Invoice>> GetAllInvoicesAsync();
    
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