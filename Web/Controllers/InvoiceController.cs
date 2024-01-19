using Contracts;
using Web.Extensions;
using Web.Requests;
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

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateInvoice(int id, [FromBody] UpdateInvoiceApiRequest request, CancellationToken ct)
    {
        try
        {
            var updateInvoiceRequest = request.ToUpdateInvoiceRequest(id);
            await _invoiceService.UpdateInvoiceAsync(updateInvoiceRequest, ct);
            return Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Ошибка при обновлении документа");
            return BadRequest($"Ошибка при обновлении документа {e.Message}");
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllInvoices()
    {
        try
        {
            var invoice = await _invoiceService.GetAllInvoicesAsync();

            return Ok(invoice);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Ошибка при получении документа");
            return BadRequest($"Ошибка при получении документа {e.Message}");
        }
    }
    
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteInvoice(string number, CancellationToken ct)
    {
        try
        {
            await _invoiceService.DeleteInvoiceAsync(number, ct);
            return Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Ошибка при удалении документа");
            return BadRequest($"Ошибка при удалении документа {e.Message}");
        }
    }
}