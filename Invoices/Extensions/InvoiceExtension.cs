using Invoices.Requests;
using Invoices.Services;

namespace Invoices.Extensions;

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