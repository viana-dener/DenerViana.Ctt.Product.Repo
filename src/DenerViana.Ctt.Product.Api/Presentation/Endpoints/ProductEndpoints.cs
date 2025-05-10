using DenerViana.Ctt.Product.Api.Application.Interfaces;
using DenerViana.Ctt.Product.Api.Application.Models.Request;
using DenerViana.Ctt.Product.Api.Application.Models.Response;
using DenerViana.Ctt.Product.Api.Base.Interfaces;
using DenerViana.Ctt.Product.Api.Domain.Dtos;
using DenerViana.Ctt.Product.Api.Presentation.Configuration;
using DenerViana.Ctt.Product.Api.Tools;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DenerViana.Ctt.Product.Api.Presentation.Endpoints;

public static class ProductEndpoints
{
    #region Public Methods

    /// <summary>
    /// Mapeia os endpoints relacionados ao produtor na aplicação, incluindo operações de listagem e cadastro de contas.
    /// Define as rotas, filtros, metadados do Swagger, cache de resposta e validação de cabeçalhos.
    /// </summary>
    public static void MapProductEndpoints(this WebApplication app)
    {
        app.MapGet("Product", async (IMainEndpoints endpoint, IProductAppServices app) =>
        {
            var result = await app.GetAllAsync();

            return endpoint.CustomResponse(result);

        }).WithName("GetProducts")
          .WithOpenApi()
          .Produces<IEnumerable<ProductResponse>>(200)
          .Produces<ErrorResponse>(400)
          .WithMetadata(new SwaggerOperationAttribute("Get all products")
          {
              OperationId = "GetProducts",
              Tags = new[] { "Products" }
          })
          .WithMetadata(new ResponseCacheAttribute
          {
              Duration = 60,
              Location = ResponseCacheLocation.Any
          });

        app.MapGet("Product/{id}", async (IMainEndpoints endpoint, IProductAppServices app, Guid id) =>
        {
            var result = await app.GetByIdAsync(id);

            return endpoint.CustomResponse(result);

        }).WithName("GetProductById")
          .WithOpenApi()
          .Produces<IEnumerable<ProductResponse>>(200)
          .Produces<ErrorResponse>(400)
          .WithMetadata(new SwaggerOperationAttribute("Get products by id")
          {
              OperationId = "GetProductById",
              Tags = new[] { "Products" }
          })
          .WithMetadata(new ResponseCacheAttribute
          {
              Duration = 60,
              Location = ResponseCacheLocation.Any
          });

        app.MapPost("Product", async (INotify notify, IMainEndpoints endpoint, IProductAppServices app, ProductRequest Product) =>
        {
            var headers = endpoint.HttpContext.Request.HttpContext.Request.Headers;
            var requiredHeaders = new Dictionary<string, bool>
            {
                { "x-userid", true },
                { "x-correlationid", true }
            };

            if (!HeadersValidate(notify, headers, requiredHeaders)) return endpoint.CustomResponse();

            var userInfo = CreateUserInfo(headers);

            var result = await app.AddAsync(Product, userInfo);

            return endpoint.CustomResponse(result);

        }).AddEndpointFilter<ValidationFilter<ProductRequest>>()
          .WithName("PostProduct")
          .WithOpenApi()
          .Produces<GenericResponse>(200)
          .Produces<ErrorResponse>(400)
          .WithMetadata(new SwaggerOperationAttribute("Register a new product")
          {
              OperationId = "PostProduct",
              Tags = new[] { "Products" }
          })
          .WithMetadata(new ResponseCacheAttribute
          {
              Duration = 60,
              Location = ResponseCacheLocation.Any
          });
    }

    #endregion

    #region Private Methods

    private static UserInfoDto CreateUserInfo(IHeaderDictionary headers)
    {
        return new UserInfoDto()
        {
            Origin = headers["x-origin"].ToString(),
            UserId = headers["x-userid"].ToString(),
            CorrelationId = headers["x-correlationid"].ToString()
        };
    }
    private static bool HeadersValidate(INotify notify, IHeaderDictionary headers, Dictionary<string, bool> requiredHeaders)
    {
        var headerValidate = ExtendedMethods.ValidateHeaders(headers, requiredHeaders);
        if (!headerValidate.Result)
        {
            foreach (var item in headerValidate.Errors)
            {
                notify.AddError(item.Key + ", " + item.Value);
            }
        }

        return headerValidate.Result;
    }

    #endregion
}
