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
           services.AddScoped<IPositionService, PositionService>();

            return services;
        }
    }
}