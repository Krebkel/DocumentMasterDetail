using Web.Requests;
using Positions.Services;

namespace Web.Extensions;

public static class PositionExtension
{
    public static CreatePositionRequest ToAddPositionRequest(this AddPositionApiRequest apiRequest, int invoiceId)
    {
        return new CreatePositionRequest
        {
            Name = apiRequest.Name,
            Number = apiRequest.Number,
            Sum = apiRequest.Sum,
            InvoiceId = invoiceId
        };
    }

    public static UpdatePositionRequest ToUpdatePositionRequest(this UpdatePositionApiRequest apiRequest, int id)
    {
        return new UpdatePositionRequest
        {
            Id = id,
            Name = apiRequest.Name,
            Number = apiRequest.Number,
            Value = apiRequest.Sum
        };
    }
}