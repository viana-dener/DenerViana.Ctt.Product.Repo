using DenerViana.Ctt.Product.Api.Base;
using Microsoft.Extensions.Options;

namespace DenerViana.Ctt.Product.Api.Presentation.Configuration;

public static class CorsSetup
{
    public static IServiceCollection AddCorsSetup(this IServiceCollection services, IConfiguration configuration)
    {
        var provider = services.BuildServiceProvider();
        var settings = provider.GetRequiredService<IOptions<AppSettings>>().Value;

        var allowedOrigins = settings.Origins;

        services.AddCors(options =>
        {
            options.AddPolicy("DominiosAceitos", policy =>
            {
                policy.WithOrigins(allowedOrigins)
                      .AllowAnyHeader()
                      .AllowAnyMethod();
            });
        });
        return services;
    }
}
