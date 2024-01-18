using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
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
}

public class UpdateInvoiceRequest
{
    public int Id { get; set; }
    public string? Number { get; set; }
    public DateTimeOffset? Date { get; set; }
    public decimal? TotalAmount { get; set; }
    public List<Position>? Positions { get; set; }
}

public class CreateInvoiceRequest
{
    public string Number { get; set; }
    public DateTimeOffset Date { get; set; }
    public decimal TotalAmount { get; set; }
    public List<Position> Positions { get; set; }
}