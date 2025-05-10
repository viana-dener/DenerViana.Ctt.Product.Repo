using System.Net;

namespace DenerViana.Ctt.Product.Api.Tools;

public class ApplicationException : Exception
{
    public HttpStatusCode StatusCode { get; }

    public ApplicationException()
    {
        StatusCode = HttpStatusCode.BadRequest;
    }
    public ApplicationException(string message) : base(message)
    {
        StatusCode = HttpStatusCode.BadRequest;
    }
    public ApplicationException(string message, int statusCode) : base(message)
    {
        StatusCode = (HttpStatusCode)statusCode;
    }

    public ApplicationException(string message, Exception exception, int statusCode) : base(message, exception)
    {
        StatusCode = (HttpStatusCode)statusCode;
    }

    public ApplicationException(int statusCode) : base("An error occurred with the status " + statusCode.ToString())
    {
        StatusCode = (HttpStatusCode)statusCode;
    }
}
