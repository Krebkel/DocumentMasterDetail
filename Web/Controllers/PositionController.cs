using Contracts;
using Web.Extensions;
using Web.Requests;
using Positions.Services;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[ApiController]
[Route("api/positions")]
public class PositionController : ControllerBase
{
    private readonly ILogger<PositionController> _logger;
    private readonly IPositionService _positionService;

    public PositionController(ILogger<PositionController> logger, IPositionService positionService)
    {
        _logger = logger;
        _positionService = positionService;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Position))]
    public async Task<IActionResult> AddPosition([FromBody] AddPositionApiRequest apiRequest, CancellationToken ct)
    {
        // TODO: возвращать результат действия и не завязываться на ексепшн. Ексепшн ловить только в случае ошибок
        try
        {
            var addPositionRequest = apiRequest.ToAddPositionRequest();
            var createdPosition = await _positionService.CreatePositionAsync(addPositionRequest, ct);
            return Ok(createdPosition);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Ошибка при добавлении позиции");
            return BadRequest($"Ошибка при добавлении позиции {e.Message}");
        }
    }

    [HttpPut]
    public async Task<IActionResult> UpdatePosition([FromBody] UpdatePositionApiRequest request, CancellationToken ct)
    {
        try
        {
            var updatePositionRequest = request.ToUpdatePositionRequest();
            await _positionService.UpdatePositionAsync(updatePositionRequest, ct);
            return Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Ошибка при обновлении позиции");
            return BadRequest($"Ошибка при обновлении позиции {e.Message}");
        }
    }
}