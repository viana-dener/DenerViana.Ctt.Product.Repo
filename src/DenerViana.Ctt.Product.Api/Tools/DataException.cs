using System.Net;
using System;

namespace DenerViana.Ctt.Product.Api.Tools;

public class DataException : Exception
{
    public HttpStatusCode StatusCode { get; }

    public DataException()
    {
        StatusCode = HttpStatusCode.BadRequest;
    }
    public DataException(string message) : base(message)
    {
        StatusCode = HttpStatusCode.BadRequest;
    }
    public DataException(string message, int statusCode) : base(message)
    {
        StatusCode = (HttpStatusCode)statusCode;
    }

    public DataException(string message, Exception exception, int statusCode) : base(message, exception)
    {
        StatusCode = (HttpStatusCode)statusCode;
    }

    public DataException(int statusCode) : base("An error occurred with the status " + statusCode.ToString())
    {
        StatusCode = (HttpStatusCode)statusCode;
    }
}
