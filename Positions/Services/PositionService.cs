using Contracts;
using Data;
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

    public async Task<Position> CreatePositionAsync(CreatePositionRequest request, CancellationToken cancellationToken)
    {
        var createdPosition = new Position
        {
            Name = request.Name,
            Quantity = request.Quantity,
            Value = request.Value,
            Invoice = request.Invoice
        };

        _dbContext.Positions.Add(createdPosition);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Позиция успешно добавлена: {@Position}", createdPosition);
        return createdPosition;
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
}