using DenerViana.Ctt.Product.Api.Base;
using DenerViana.Ctt.Product.Api.Presentation.Configuration;
using Serilog;
using Serilog.Sinks.Elasticsearch;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

var settings = builder.Configuration.GetSection("AppSettings").Get<AppSettings>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerSetup(builder.Configuration);
builder.Services.AddEndpointsApiSetup(builder.Configuration);

builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration)
        .Enrich.FromLogContext()
        .Enrich.WithMachineName()
        .Enrich.WithThreadId()
        .Enrich.WithProperty("App", "Ctt Product")
        .Enrich.WithProperty("v1", "DenerViana.Ctt.Product")
        .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
        .MinimumLevel.Information()
        .WriteTo.Console()
        .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(settings.Elasticsearch))
        {
            AutoRegisterTemplate = true,
            IndexFormat = $"denerviana-ctt-product-{DateTime.UtcNow:yyyy-MM}",
            ModifyConnectionSettings = conn =>
                conn.BasicAuthentication("elastic", "VianaHub2023")
                    .ServerCertificateValidationCallback((o, cert, chain, errors) => true)
        });
});

Log.Error("Aplicação iniciada.");

builder.Services.AddCorsSetup(builder.Configuration);
builder.Services.AddIocSetup();

var app = builder.Build();

app.UseEndpointsApiConfiguration();

app.Run();
