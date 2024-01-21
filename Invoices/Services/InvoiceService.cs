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
            Date = request.Date.ToUniversalTime(),
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
            _dbContext.Invoices.Remove(createdInvoice);
            var reason = $"Не удалось добавить новый документ. Документ №{createdInvoice.Number} уже существует!";
            await _errorLogService.CreateErrorLogAsync(new CreateErrorLogRequest
            {
                Note = reason
            });
            return InvoiceCreationResult.Error(reason);
        }

        return InvoiceCreationResult.Success(createdInvoice);
    }

    public async Task<InvoiceUpdateResult> UpdateInvoiceAsync(UpdateInvoiceRequest request, CancellationToken cancellationToken)
    {
        var existingInvoice = await _dbContext.Invoices.FirstOrDefaultAsync(i => i.Id == request.Id);

        if (existingInvoice == null)
        {
            _logger.LogWarning("Документ с Id: {Id} не найден.", request.Id);
            return InvoiceUpdateResult.Error($"Не найден документ с Id: {request.Id}");
        }

        if (!string.IsNullOrEmpty(request.Number))
            existingInvoice.Number = request.Number;

        if (request.Date.HasValue)
            existingInvoice.Date = request.Date.Value.ToUniversalTime();

        if (!string.IsNullOrEmpty(request.Note))
            existingInvoice.Note = request.Note;

        try
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException e)
            when (e.InnerException is PostgresException { SqlState: PostgresErrorCodes.UniqueViolation })
        {
            await _dbContext.Invoices.Entry(existingInvoice).ReloadAsync(cancellationToken);
            var reason = $"Не удалось обновить документ с Id:{existingInvoice.Id}. " +
                         $"Произведена попытка поменять номер документа на существующий в базе: {request.Number}";
            await _errorLogService.CreateErrorLogAsync(new CreateErrorLogRequest { Note = reason }, cancellationToken);
            return InvoiceUpdateResult.Error(reason);
        }
        
        _logger.LogInformation("Документ успешно обновлен: {@InvoiceNumber}", existingInvoice.Number);

        return InvoiceUpdateResult.Success(existingInvoice);
    }

    public async Task<Invoice> GetInvoiceAsync(int id)
    {
        return await _dbContext.Invoices.FirstAsync(i => i.Id == id);
    }

    public async Task<Invoice[]> GetAllInvoicesAsync()
    {
        return await _dbContext.Invoices.ToArrayAsync();
    }

    public async Task DeleteInvoiceAsync(int id, CancellationToken cancellationToken)
    {
        var existingInvoice = await _dbContext.Invoices.FirstOrDefaultAsync(i => i.Id == id);

        if (existingInvoice == null)
        {
            _logger.LogWarning("Документ с номером {InvoiceNumber} не найден.", id);
            return;
        }

        _dbContext.Invoices.Remove(existingInvoice);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Документ успешно удален: {@InvoiceNumber}", existingInvoice.Number);
    }
}