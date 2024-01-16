using Contracts;

namespace ErrorLogs.Services;

public interface IErrorLogService
{
    /// <summary>
    /// Создать лог ошибки
    /// </summary>
    Task<ErrorLog> CreateErrorLogAsync(CreateErrorLogRequest errorLog, CancellationToken cancellationToken);
}

public class CreateErrorLogRequest
{
    public DateTime? Date { get; set; }
    public string? Note { get; set; }
}