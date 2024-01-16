using Positions.Requests;
using Positions.Services;

namespace Positions.Extensions;

public static class PositionExtension
{
    public static CreatePositionRequest ToAddPositionRequest(this AddPositionApiRequest apiRequest)
    {
        return new CreatePositionRequest();
    }

    public static UpdatePositionRequest ToUpdatePositionRequest(this UpdatePositionApiRequest request)
    {
        return new UpdatePositionRequest();
    }
}