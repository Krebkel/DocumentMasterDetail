using Contracts;
using Web.Extensions;
using Web.Requests;
using Invoices.Services;
using Microsoft.AspNetCore.Mvc;
using Positions.Services;
using Web.Responses;

namespace Web.Controllers;

[ApiController]
[Route("api/invoices")]
public class InvoiceController : ControllerBase
{
    private readonly ILogger<InvoiceController> _logger;
    private readonly IInvoiceService _invoiceService;
    private readonly IPositionService _positionService;

    public InvoiceController(ILogger<InvoiceController> logger, 
        IInvoiceService invoiceService, 
        IPositionService positionService)
    {
        _logger = logger;
        _invoiceService = invoiceService;
        _positionService = positionService;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Invoice))]
    public async Task<IActionResult> AddInvoice([FromBody] AddInvoiceApiRequest apiRequest, CancellationToken ct)
    {
        try
        {
            var addInvoiceRequest = apiRequest.ToAddInvoiceRequest();
            var result = await _invoiceService.CreateInvoiceAsync(addInvoiceRequest, ct);
            
            if (result.IsSuccess)
            {
                var invoicePositions = await _positionService.GetPositionsByInvoiceIdAsync(result.Result.Id);

                var apiResult = new ApiInvoice
                {
                    Id = result.Result.Id,
                    Number = result.Result.Number,
                    Date = result.Result.Date,
                    Note = result.Result.Note,
                    Sum = invoicePositions.Select(p => p.Value).Sum()
                };
                return Ok(apiResult);
            }
            
            return BadRequest(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Ошибка при добавлении документа");
            return Problem($"Ошибка при добавлении документа {e.Message}");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateInvoice(int id, [FromBody] UpdateInvoiceApiRequest request, CancellationToken ct)
    {
        try
        {
            var updateInvoiceRequest = request.ToUpdateInvoiceRequest(id);
            var result = await _invoiceService.UpdateInvoiceAsync(updateInvoiceRequest, ct);
            
            if (result.IsSuccess)
            {
                var invoicePositions = await _positionService.GetPositionsByInvoiceIdAsync(id);

                var apiResult = new ApiInvoice
                {
                    Id = result.Result.Id,
                    Number = result.Result.Number,
                    Date = result.Result.Date,
                    Note = result.Result.Note,
                    Sum = invoicePositions.Select(p => p.Value).Sum()
                };
                return Ok(apiResult);
            }

            return BadRequest(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Ошибка при обновлении документа");
            return BadRequest($"Ошибка при обновлении документа {e.Message}");
        }
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetInvoice(int id)
    {
        try
        {
            var invoice = await _invoiceService.GetInvoiceAsync(id);
            var positions = await _positionService.GetPositionsByInvoiceIdAsync(invoice.Id);
            var result = new ApiInvoice
            {
                Id = invoice.Id,
                Number = invoice.Number,
                Date = invoice.Date,
                Note = invoice.Note,
                Sum = positions.Select(p => p.Value).Sum()
            };

            return Ok(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Ошибка при получении документа");
            return BadRequest($"Ошибка при получении документа {e.Message}");
        }
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllInvoices()
    {
        try
        {
            var result = new List<ApiInvoice>();
            var invoices = await _invoiceService.GetAllInvoicesAsync();

            foreach (var invoice in invoices)
            {
                var positions = await _positionService.GetPositionsByInvoiceIdAsync(invoice.Id);
                result.Add(new ApiInvoice
                {
                    Id = invoice.Id,
                    Number = invoice.Number,
                    Date = invoice.Date,
                    Note = invoice.Note,
                    Sum = positions.Select(p => p.Value).Sum()
                });
            }

            return Ok(result.ToArray());
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Ошибка при получении документа");
            return BadRequest($"Ошибка при получении документа {e.Message}");
        }
    }
    
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteInvoice(int id, CancellationToken ct)
    {
        try
        {
            await _invoiceService.DeleteInvoiceAsync(id, ct);
            return Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Ошибка при удалении документа");
            return BadRequest($"Ошибка при удалении документа {e.Message}");
        }
    }
}