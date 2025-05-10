using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DenerViana.Ctt.Product.Api.Base.Interfaces;

/// <summary>
/// Interface para manipulação dos Endipoints
/// </summary>
public interface IMainEndpoints
{
    /// <summary>
    /// Retorna o HttpContext
    /// </summary>
    HttpContext HttpContext { get; }

    /// <summary>
    /// Retorna um response customizado
    /// </summary>
    IResult CustomResponse(object result = null);

    /// <summary>
    /// Retorna um response customizado vindo na Model State
    /// </summary>
    IResult CustomResponse(ModelStateDictionary modelState);

    /// <summary>
    /// Verifica se é válido
    /// </summary>
    bool IsValid();
}
