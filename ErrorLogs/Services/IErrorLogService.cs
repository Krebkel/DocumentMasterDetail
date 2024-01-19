using System;
using System.Threading;
using System.Threading.Tasks;
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
    public int Id { get; set; }
    public DateTimeOffset Date { get; set; }
    public string? Note { get; set; }
}