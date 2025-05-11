using DenerViana.Ctt.Product.Api.Application.AutoMapper;
using DenerViana.Ctt.Product.Api.Application.Interfaces;
using DenerViana.Ctt.Product.Api.Application.Services;
using DenerViana.Ctt.Product.Api.Base.Interfaces;
using DenerViana.Ctt.Product.Api.Domain.Interfaces;
using DenerViana.Ctt.Product.Api.Domain.Services;
using DenerViana.Ctt.Product.Api.Infra.Context;
using DenerViana.Ctt.Product.Api.Infra.Repository;

namespace DenerViana.Ctt.Product.Api.Base;

public static class BootStrapper
{
    public static IServiceCollection AddIocSetup(this IServiceCollection services)
    {

        // Applications
        services.AddAutoMapper(typeof(MappingProfile));
        services.AddScoped<IProductAppServices, ProductAppServices>();

        // Domain
        services.AddScoped<INotify, Notify>();
        services.AddScoped<ILogInformation, LogInformation>();
        services.AddScoped<IProductServices, ProductServices>();

        // Repository
        services.AddScoped<IMongoDbContext, MongoDbContext>();
        services.AddScoped<IProductRepository, ProductRepository>();

        // Integration

        return services;
    }
}
