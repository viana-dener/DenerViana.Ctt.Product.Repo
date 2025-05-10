using System.Net;

namespace DenerViana.Ctt.Product.Api.Tools;

public class DomainException : Exception
{
    public HttpStatusCode StatusCode { get; }

    public DomainException()
    {
        StatusCode = HttpStatusCode.BadRequest;
    }
    public DomainException(string message) : base(message)
    {
        StatusCode = HttpStatusCode.BadRequest;
    }
    public DomainException(string message, int statusCode) : base(message)
    {
        StatusCode = (HttpStatusCode)statusCode;
    }

    public DomainException(string message, Exception exception, int statusCode) : base(message, exception)
    {
        StatusCode = (HttpStatusCode)statusCode;
    }

    public DomainException(int statusCode) : base("An error occurred with the status " + statusCode.ToString())
    {
        StatusCode = (HttpStatusCode)statusCode;
    }
}
