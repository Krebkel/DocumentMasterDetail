using Web.Requests;
using Invoices.Services;

namespace Web.Extensions;

public static class InvoiceExtension
{
    public static CreateInvoiceRequest ToAddInvoiceRequest(this AddInvoiceApiRequest apiRequest)
    {
        return new CreateInvoiceRequest();
    }

    public static UpdateInvoiceRequest ToUpdateInvoiceRequest(this UpdateInvoiceApiRequest request)
    {
        return new UpdateInvoiceRequest();
    }
}