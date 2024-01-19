using Web.Requests;
using Positions.Services;

namespace Web.Extensions;

public static class PositionExtension
{
    public static CreatePositionRequest ToAddPositionRequest(this AddPositionApiRequest apiRequest)
    {
        return new CreatePositionRequest
        {
            Name = apiRequest.Name,
            Quantity = apiRequest.Quantity,
            Value = apiRequest.Value,
            InvoiceId = apiRequest.InvoiceId
        };
    }

    public static UpdatePositionRequest ToUpdatePositionRequest(this UpdatePositionApiRequest apiRequest, int id)
    {
        return new UpdatePositionRequest
        {
            Id = id,
            Name = apiRequest.Name,
            Quantity = apiRequest.Quantity,
            Value = apiRequest.Value
        };
    }
}