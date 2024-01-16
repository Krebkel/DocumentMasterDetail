using Data;
using Invoices.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Invoices
{
    public static class DependencyRegistrations
    {
        public static IServiceCollection AddPostgresInvoices(this IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>((provider, opt) =>
            {
                var options = provider.GetRequiredService<IOptions<DataOptions>>().Value;

                opt.UseNpgsql(options.ConnectionString,
                    builder => builder.MigrationsHistoryTable("__EFMigrationsHistory", options.ServiceSchema));
            });
            
            services.AddScoped<IInvoiceService, InvoiceService>();

            return services;
        }
    }
}