using DenerViana.Ctt.Product.Api.Application.Models.Request;
using DenerViana.Ctt.Product.Api.Base.Interfaces;
using DenerViana.Ctt.Product.Api.Base;
using DenerViana.Ctt.Product.Api.Tools;
using FluentValidation;
using Microsoft.AspNetCore.ResponseCompression;
using Serilog;
using DenerViana.Ctt.Product.Api.Presentation.Validations;
using DenerViana.Ctt.Product.Api.Presentation.Endpoints;
using DenerViana.Ctt.Product.Api.Infra.Context;

namespace DenerViana.Ctt.Product.Api.Presentation.Configuration;

public static class EndpointsApiSetup
{
    public static IServiceCollection AddEndpointsApiSetup(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<IMainEndpoints, MainEndpoints>();
        services.AddTransient<IValidator<ProductRequest>, ProductRouteValidator>();

        services.Configure<MongoDbSettings>(configuration.GetSection("MongoDbSettings"));
        services.AddSingleton<MongoDbContext>();

        services.AddResponseCompression(options =>
        {
            options.EnableForHttps = true;
            options.Providers.Add<GzipCompressionProvider>();
        });
        services.Configure<GzipCompressionProviderOptions>(options =>
        {
            options.Level = System.IO.Compression.CompressionLevel.Fastest;
        });


        services.Configure<AppSettings>(configuration.GetSection("AppSettings"));
        services.Configure<SwaggerSettings>(configuration.GetSection("SwaggerSettings"));

        return services;
    }

    /// <summary>
    /// Método de extensão que estende configurações da WebApplication
    /// </summary>
    public static void UseEndpointsApiConfiguration(this WebApplication app)
    {
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Exercício CTT");
                c.DocumentTitle = "Exercício CTT";
            });
        }

        app.UseSerilogRequestLogging(options =>
        {
            options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
            {
                diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
                diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
                diagnosticContext.Set("UserAgent", httpContext.Request.Headers["User-Agent"].ToString());
                diagnosticContext.Set("CorrelationId", httpContext.Request.Headers["x-correlation-id"].ToString());
            };
        });

        app.UseMiddleware<RequestContextMiddleware>();
        app.UseMiddleware<ExceptionMiddleware>();

        app.UseResponseCompression();
        app.UseHttpsRedirection();
        app.MapProductEndpoints();
    }
}
