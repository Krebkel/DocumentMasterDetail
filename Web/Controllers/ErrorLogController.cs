using Contracts;
using ErrorLogs.Extensions;
using ErrorLogs.Requests;
using ErrorLogs.Services;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[ApiController]
[Route("api/errorLogs")]
public class ErrorLogController : ControllerBase
{
    private readonly ILogger<ErrorLogController> _logger;
    private readonly IErrorLogService _errorLogService;

    public ErrorLogController(ILogger<ErrorLogController> logger, IErrorLogService errorLogService)
    {
        _logger = logger;
        _errorLogService = errorLogService;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ErrorLog))]
    public async Task<IActionResult> AddErrorLog([FromBody] AddErrorLogApiRequest apiRequest, CancellationToken ct)
    {
        // TODO: возвращать результат действия и не завязываться на ексепшн. Ексепшн ловить только в случае ошибок
        try
        {
            var addErrorLogRequest = apiRequest.ToAddErrorLogRequest();
            var createdErrorLog = await _errorLogService.CreateErrorLogAsync(addErrorLogRequest, ct);
            return Ok(createdErrorLog);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Ошибка при добавлении лога ошибки");
            return BadRequest($"Ошибка при добавлении лога ошибки {e.Message}");
        }
    }
}