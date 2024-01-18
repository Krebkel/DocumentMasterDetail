using Web.Requests;
using ErrorLogs.Services;

namespace Web.Extensions;

public static class ErrorLogExtension
{
    public static CreateErrorLogRequest ToAddErrorLogRequest(this AddErrorLogApiRequest apiRequest)
    {
        return new CreateErrorLogRequest();
    }
}