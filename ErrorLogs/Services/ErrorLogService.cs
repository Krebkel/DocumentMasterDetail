using Contracts;
using Data;
using Microsoft.Extensions.Logging;

namespace ErrorLogs.Services;

public class ErrorLogService : IErrorLogService
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<ErrorLogService> _logger;

    public ErrorLogService(AppDbContext dbContext, ILogger<ErrorLogService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<ErrorLog> CreateErrorLogAsync(CreateErrorLogRequest request, CancellationToken cancellationToken)
    {
        var createdErrorLog = new ErrorLog
        {
            Date = DateTimeOffset.UtcNow,
            Note = request.Note
        };

        _dbContext.ErrorLogs.Add(createdErrorLog);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Лог ошибки успешно добавлен: {@ErrorLog}", createdErrorLog);
        return createdErrorLog;
    }
}