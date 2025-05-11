using DenerViana.Ctt.Product.Api.Base.Interfaces;
using System.Net;

namespace DenerViana.Ctt.Product.Api.Base;

public class Notify : INotify
{
    private HttpStatusCode StatusCode { get; set; }
    private readonly ICollection<string> Errors = [];

    public ICollection<string> GetErrors() => Errors;

    public int GetStatusCode()
    {
        if (Errors.Count == 0)
            return (int)HttpStatusCode.OK;

        return (int)StatusCode;
    }
    public void AddError(string error)
    {
        Errors.Add(error);
    }
    public void AddError(string error, int statusCode)
    {
        StatusCode = (HttpStatusCode)statusCode;
        Errors.Add(error);
    }
    public void ClearErrors()
    {
        Errors.Clear();
    }
}
