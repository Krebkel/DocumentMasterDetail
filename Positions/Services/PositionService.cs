using Contracts;
using Data;
using ErrorLogs.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Positions.Services;

public class PositionService : IPositionService
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<PositionService> _logger;
    private readonly IErrorLogService _errorLogService;

    public PositionService(AppDbContext dbContext, ILogger<PositionService> logger, IErrorLogService errorLogService)
    {
        _dbContext = dbContext;
        _logger = logger;
        _errorLogService = errorLogService;
    }

    public async Task<PositionCreationResult> CreatePositionAsync(CreatePositionRequest request, 
        CancellationToken cancellationToken)
    {
        var createdPosition = new Position
        {
            Name = request.Name,
            Quantity = request.Quantity,
            Value = request.Value,
            Invoice = request.Invoice
        };

        await _dbContext.Positions.AddAsync(createdPosition, cancellationToken);

        try
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException e)
            when (e.InnerException is PostgresException { SqlState: PostgresErrorCodes.UniqueViolation })
        {
            var errorRequest = new CreateErrorLogRequest
            {
                Date = DateTimeOffset.Now,
                Note = $"{createdPosition.Id} - {createdPosition.Name}"
            };
            await _errorLogService.CreateErrorLogAsync(errorRequest, cancellationToken);
            return PositionCreationResult.Error("Ошибка добавления");
        }
        
        return PositionCreationResult.Success(createdPosition);
    }

    public async Task UpdatePositionAsync(UpdatePositionRequest request, CancellationToken ct)
    {
        var existingPosition = await _dbContext.Positions.FindAsync(request.Id);

        if (existingPosition == null)
        {
            _logger.LogWarning("Позиция с Id {PositionId} не найдена.", request.Id);
            return;
        }

        // Обновление свойств позиции
        if (request.Name != null)
            existingPosition.Name = request.Name;
        
        if (request.Quantity.HasValue)
            existingPosition.Quantity = request.Quantity.Value;
        
        if (request.Value.HasValue)
            existingPosition.Value = request.Value.Value;
        
        if (request.Invoice != null)
            existingPosition.Invoice = request.Invoice;

        await _dbContext.SaveChangesAsync(ct);

        _logger.LogInformation("Позиция успешно обновлена: {@Position}", existingPosition);
    }
    public async Task<Position> GetPositionAsync(int id)
    {
        return await _dbContext.Positions.FindAsync(id);
    }

    public async Task DeletePositionAsync(int id, CancellationToken cancellationToken)
    {
        var existingPosition = await _dbContext.Positions.FindAsync(id);

        if (existingPosition == null)
        {
            _logger.LogWarning("Позиция с Id {PositionId} не найдена.", id);
            return;
        }

        _dbContext.Positions.Remove(existingPosition);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Позиция успешно удалена: {@Position}", existingPosition);
    }

    public async Task<List<Position>> GetPositionsByInvoiceIdAsync(int invoiceId)
    {
        return await _dbContext.Positions.Where(p => p.Invoice.Id == invoiceId).ToListAsync();
    }
}