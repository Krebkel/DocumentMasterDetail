using Data;
using Positions.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Positions
{
    public static class DependencyRegistrations
    {
        public static IServiceCollection AddPostgresPositions(this IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>((provider, opt) =>
            {
                var options = provider.GetRequiredService<IOptions<DataOptions>>().Value;

                opt.UseNpgsql(options.ConnectionString,
                    builder => builder.MigrationsHistoryTable("__EFMigrationsHistory", options.ServiceSchema));
            });
            
            services.AddScoped<IPositionService, PositionService>();

            return services;
        }
    }
}