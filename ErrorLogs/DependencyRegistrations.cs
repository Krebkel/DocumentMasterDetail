using Data;
using ErrorLogs.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ErrorLogs
{
    public static class DependencyRegistrations
    {
        public static IServiceCollection AddPostgresErrorLogs(this IServiceCollection services)
        {
            services.AddScoped<IErrorLogService, ErrorLogService>();

            return services;
        }
    }
}