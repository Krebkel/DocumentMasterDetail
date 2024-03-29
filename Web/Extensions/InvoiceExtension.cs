using Web.Requests;
using Invoices.Services;

namespace Web.Extensions;

public static class InvoiceExtension
{
    public static CreateInvoiceRequest ToAddInvoiceRequest(this AddInvoiceApiRequest apiRequest)
    {
        return new CreateInvoiceRequest
        {
            Number = apiRequest.Number,
            Date = apiRequest.Date,
            Note = apiRequest.Note
        };
    }

    public static UpdateInvoiceRequest ToUpdateInvoiceRequest(this UpdateInvoiceApiRequest apiRequest, int id)
    {
        return new UpdateInvoiceRequest
        {
            Id = id,
            Number = apiRequest.Number,
            Date = apiRequest.Date,
            Note = apiRequest.Note
        };
    }
}