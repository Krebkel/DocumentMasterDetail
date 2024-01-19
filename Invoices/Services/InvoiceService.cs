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
            Note = request.Note
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
            return InvoiceCreationResult.Error("Ошибка добавления");
        }

        return InvoiceCreationResult.Success(createdInvoice);
    }

    public async Task UpdateInvoiceAsync(UpdateInvoiceRequest request, CancellationToken cancellationToken)
    {
        var existingInvoice = await _dbContext.Invoices.FindAsync(request.Number);

        if (existingInvoice == null)
        {
            _logger.LogWarning("Документ с номером {InvoiceNumber} не найден.", request.Number);
            return;
        }

        if (request.Number != null)
            existingInvoice.Number = request.Number;

        if (request.Date.HasValue)
            existingInvoice.Date = request.Date.Value;

        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Документ успешно обновлен: {@InvoiceNumber}", existingInvoice.Number);
    }

    public async Task<Invoice> GetInvoiceAsync(string number)
    {
        return await _dbContext.Invoices.FindAsync(number);
    }

    public async Task<List<Invoice>> GetAllInvoicesAsync()
    {
        return await _dbContext.Invoices.ToListAsync();
    }

    public async Task<List<Position>> GetAllPositionsForInvoiceAsync(string invoiceNumber)
    {
        var positions = await _dbContext.Positions.Where(p => p.Invoice.Number.Equals(invoiceNumber)).ToListAsync();
        return positions;
    }

    public async Task DeleteInvoiceAsync(string number, CancellationToken cancellationToken)
    {
        var existingInvoice = await _dbContext.Invoices.FindAsync(number);

        if (existingInvoice == null)
        {
            _logger.LogWarning("Документ с номером {InvoiceNumber} не найден.", number);
            return;
        }

        _dbContext.Invoices.Remove(existingInvoice);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Документ успешно удален: {@InvoiceNumber}", existingInvoice.Number);
    }
}