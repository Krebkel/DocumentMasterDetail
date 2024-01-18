using System.Threading;
using System.Threading.Tasks;
using Contracts;
using Data;
using Microsoft.Extensions.Logging;

namespace Invoices.Services;

public class InvoiceService : IInvoiceService
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<InvoiceService> _logger;

    public InvoiceService(AppDbContext dbContext, ILogger<InvoiceService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Invoice> CreateInvoiceAsync(CreateInvoiceRequest request, CancellationToken cancellationToken)
    {
        var createdInvoice = new Invoice
        {
            Number = request.Number,
            Date = request.Date,
            TotalAmount = request.TotalAmount,
            Positions = request.Positions
        };

        _dbContext.Invoices.Add(createdInvoice);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Документ успешно добавлен: {@Invoice}", createdInvoice);
        return createdInvoice;
    }

    public async Task UpdateInvoiceAsync(UpdateInvoiceRequest request, CancellationToken ct)
    {
        var existingInvoice = await _dbContext.Invoices.FindAsync(request.Id);

        if (existingInvoice == null)
        {
            _logger.LogWarning("Документ с Id {InvoiceId} не найден.", request.Id);
            return;
        }

        existingInvoice.Number = request.Number;
        existingInvoice.Date = request.Date;
        existingInvoice.TotalAmount = request.TotalAmount;
        existingInvoice.Positions = request.Positions;

        await _dbContext.SaveChangesAsync(ct);

        _logger.LogInformation("Документ успешно обновлен: {@Invoice}", existingInvoice);
    }
}