using Contracts;
using Invoices.Extensions;
using Invoices.Requests;
using Invoices.Services;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[ApiController]
[Route("api/invoices")]
public class InvoiceController : ControllerBase
{
    private readonly ILogger<InvoiceController> _logger;
    private readonly IInvoiceService _invoiceService;

    public InvoiceController(ILogger<InvoiceController> logger, IInvoiceService invoiceService)
    {
        _logger = logger;
        _invoiceService = invoiceService;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Invoice))]
    public async Task<IActionResult> AddInvoice([FromBody] AddInvoiceApiRequest apiRequest, CancellationToken ct)
    {
        // TODO: возвращать результат действия и не завязываться на ексепшн. Ексепшн ловить только в случае ошибок
        try
        {
            var addInvoiceRequest = apiRequest.ToAddInvoiceRequest();
            var createdInvoice = await _invoiceService.CreateInvoiceAsync(addInvoiceRequest, ct);
            return Ok(createdInvoice);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Ошибка при добавлении документа");
            return BadRequest($"Ошибка при добавлении документа {e.Message}");
        }
    }

    [HttpPut]
    public async Task<IActionResult> UpdateInvoice([FromBody] UpdateInvoiceApiRequest request, CancellationToken ct)
    {
        try
        {
            var updateInvoiceRequest = request.ToUpdateInvoiceRequest();
            await _invoiceService.UpdateInvoiceAsync(updateInvoiceRequest, ct);
            return Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Ошибка при обновлении документа");
            return BadRequest($"Ошибка при обновлении документа {e.Message}");
        }
    }
}