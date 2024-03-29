using Contracts;
using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Positions.Services;

public class PositionService : IPositionService
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<PositionService> _logger;

    public PositionService(AppDbContext dbContext, ILogger<PositionService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<PositionCreationResult> CreatePositionAsync(CreatePositionRequest request, 
        CancellationToken cancellationToken)
    {
        var invoice = await _dbContext.Invoices.FirstOrDefaultAsync(i => i.Id == request.InvoiceId, 
            cancellationToken: cancellationToken);
        
        if (invoice == null)
        {
            return PositionCreationResult.Error("Ошибка добавления позиции. Документ не найден!");
        }
        
        var createdPosition = new Position
        {
            Name = request.Name,
            Number = request.Number,
            Value = request.Sum,
            Invoice = invoice
        };

        await _dbContext.Positions.AddAsync(createdPosition, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return PositionCreationResult.Success(createdPosition);
    }

    public async Task<Position> UpdatePositionAsync(UpdatePositionRequest request, CancellationToken ct)
    {
        var existingPosition = await _dbContext.Positions.FindAsync(request.Id);

        if (existingPosition == null)
        {
            _logger.LogWarning("Позиция с Id {PositionId} не найдена.", request.Id);
            throw new ApplicationException($"Не найдена позиция с id: {request.Id}");
        }

        // Обновление свойств позиции
        if (!string.IsNullOrEmpty(request.Name))
            existingPosition.Name = request.Name;
        
        if (!string.IsNullOrEmpty(request.Number))
            existingPosition.Number = request.Number;
        
        if (request.Value.HasValue)
            existingPosition.Value = request.Value.Value;

        await _dbContext.SaveChangesAsync(ct);

        _logger.LogInformation("Позиция успешно обновлена: {@Position}", existingPosition);

        return existingPosition;
    }
    
    public async Task<Position> GetPositionAsync(int id)
    {
        return await _dbContext.Positions.FirstAsync(p => p.Id == id);
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

    public async Task<Position[]> GetPositionsByInvoiceIdAsync(int invoiceId)
    {
        return await _dbContext.Positions
            .Include(p => p.Invoice)
            .Where(p => p.Invoice.Id == invoiceId)
            .ToArrayAsync();
    }
}