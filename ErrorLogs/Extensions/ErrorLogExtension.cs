using ErrorLogs.Requests;
using ErrorLogs.Services;

namespace ErrorLogs.Extensions;

public static class ErrorLogExtension
{
    public static CreateErrorLogRequest ToAddErrorLogRequest(this AddErrorLogApiRequest apiRequest)
    {
        return new CreateErrorLogRequest();
    }
}