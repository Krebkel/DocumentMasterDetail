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
            services.AddScoped<IInvoiceService, InvoiceService>();

            return services;
        }
    }
}