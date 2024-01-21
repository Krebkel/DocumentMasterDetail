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
    Task<ErrorLog> CreateErrorLogAsync(CreateErrorLogRequest errorLog, CancellationToken cancellationToken = default);
}

public class CreateErrorLogRequest
{
    public string? Note { get; set; }
}