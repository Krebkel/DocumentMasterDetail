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
            Invoice = apiRequest.Invoice
        };
    }

    public static UpdatePositionRequest ToUpdatePositionRequest(this UpdatePositionApiRequest apiRequest)
    {
        return new UpdatePositionRequest
        {
            Name = apiRequest.Name,
            Quantity = apiRequest.Quantity,
            Value = apiRequest.Value,
            Invoice = apiRequest.Invoice
        };
    }
}