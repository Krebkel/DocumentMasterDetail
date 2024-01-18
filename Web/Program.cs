using Data;
using ErrorLogs;
using Invoices;
using Positions;
using Microsoft.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers();

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen();

builder.Services
    .Configure<DataOptions>(builder.Configuration.GetSection("Postgres"));

builder.Services
    .AddPostgresData();

builder.Services
    .AddPostgresErrorLogs();

builder.Services
    .AddPostgresInvoices();

builder.Services
    .AddPostgresPositions();

builder.Services.AddHealthChecks();

var app = builder.Build();

// Swagger UI и HealthChecks
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

// Swagger UI
app.UseSwagger();
app.UseSwaggerUI();

app.UseDefaultFiles()
    .UseStaticFiles(new StaticFileOptions
    {
        OnPrepareResponse = context =>
        {
            var headers = context.Context.Response.GetTypedHeaders();
            headers.CacheControl = new CacheControlHeaderValue { Public = true, MaxAge = TimeSpan.FromMinutes(1) };
        }
    });

app.Run();