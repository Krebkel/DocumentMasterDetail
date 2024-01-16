using Data;
using ErrorLogs;
using Invoices;
using Positions;
using Microsoft.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>()!;

builder.Services
    .AddControllers();

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen(setup =>
    {
        // Swagger
    });

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


app.UseEndpoints(e => e.MapControllers());

// Swagger UI

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