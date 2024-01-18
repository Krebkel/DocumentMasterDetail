using Contracts;
using Data;
using ErrorLogs.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Invoices.Services;

public class InvoiceService : IInvoiceService
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<InvoiceService> _logger;
    private readonly IErrorLogService _errorLogService;

    public InvoiceService(AppDbContext dbContext, ILogger<InvoiceService> logger, IErrorLogService errorLogService)
    {
        _dbContext = dbContext;
        _logger = logger;
        _errorLogService = errorLogService;
    }

    public async Task<InvoiceCreationResult> CreateInvoiceAsync(CreateInvoiceRequest request,
        CancellationToken cancellationToken)
    {
        var createdInvoice = new Invoice
        {
            Number = request.Number,
            Date = request.Date,
            TotalAmount = request.TotalAmount,
            Positions = request.Positions
        };

        await _dbContext.Invoices.AddAsync(createdInvoice, cancellationToken);

        try
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException e)
            when (e.InnerException is PostgresException { SqlState: PostgresErrorCodes.UniqueViolation })
        {
            var errorRequest = new CreateErrorLogRequest
            {
                Date = request.Date,
                Note = $"{createdInvoice.Id} - {createdInvoice.Number}"
            };
            await _errorLogService.CreateErrorLogAsync(errorRequest, cancellationToken);
            return InvoiceCreationResult.Error("Причина ошибки добавления");
        }

        return InvoiceCreationResult.Success(createdInvoice);
    }

    public async Task UpdateInvoiceAsync(UpdateInvoiceRequest request, CancellationToken cancellationToken)
    {
        var existingInvoice = await _dbContext.Invoices.FindAsync(request.Id);

        if (existingInvoice == null)
        {
            _logger.LogWarning("Документ с Id {InvoiceId} не найден.", request.Id);
            return;
        }

        if (request.Number != null)
            existingInvoice.Number = request.Number;

        if (request.Date.HasValue)
            existingInvoice.Date = request.Date.Value;

        if (request.TotalAmount.HasValue)
            existingInvoice.TotalAmount = request.TotalAmount.Value;

        if (request.Positions != null)
            existingInvoice.Positions = request.Positions;

        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Документ успешно обновлен: {@Invoice}", existingInvoice);
    }
}