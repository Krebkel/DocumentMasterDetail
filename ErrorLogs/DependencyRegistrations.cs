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
            services.AddDbContext<AppDbContext>((provider, opt) =>
            {
                var options = provider.GetRequiredService<IOptions<DataOptions>>().Value;

                opt.UseNpgsql(options.ConnectionString,
                    builder => builder.MigrationsHistoryTable("__EFMigrationsHistory", options.ServiceSchema));
            });
            
            services.AddScoped<IErrorLogService, ErrorLogService>();

            return services;
        }
    }
}