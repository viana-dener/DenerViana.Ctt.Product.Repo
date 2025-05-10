using DenerViana.Ctt.Product.Api.Domain.Dtos;
using System.Text.RegularExpressions;

namespace DenerViana.Ctt.Product.Api.Tools;

public static class ExtendedMethods
{
    public const int TaxNumberLength = 9;

    public static string OnlyNumbers(this string input)
    {
        return new string(input.Where(char.IsDigit).ToArray());
    }

    public static bool IsEmailValid(this string email)
    {
        if (string.IsNullOrWhiteSpace(email)) return false;

        var regex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
        return regex.IsMatch(email);
    }

    public static bool IsTaxNumberValid(this string nif)
    {
        nif = nif.OnlyNumbers();

        if (nif.Length != TaxNumberLength) return false;
        if ("1235689".IndexOf(nif[0]) == -1) return false;

        if (string.IsNullOrWhiteSpace(nif) || !Regex.IsMatch(nif, @"^\d{9}$"))
            return false;

        int sum = 0;
        for (int i = 0; i < 8; i++)
        {
            sum += (nif[i] - '0') * (9 - i);
        }

        int remainder = sum % 11;
        int checkDigit = remainder < 2 ? 0 : 11 - remainder;

        return checkDigit == (nif[8] - '0');
    }

    public static string GetCorrelationId(IHttpContextAccessor accessor)
    {
        var httpContext = accessor.HttpContext;
        if (httpContext != null && httpContext.Request.Headers.TryGetValue("x-correlation-id", out var value))
        {
            return value;
        }

        return Guid.NewGuid().ToString();
    }

    public static HeaderErrorsDto ValidateHeaders(this IHeaderDictionary headers, Dictionary<string, bool> requiredHeaders)
    {
        var result = new HeaderErrorsDto
        {
            Result = true,
            Errors = new Dictionary<string, string>()
        };

        foreach (var header in requiredHeaders)
        {
            var headerKey = header.Key;
            var isRequired = header.Value;
            var headerValue = headers[headerKey].ToString();

            if (isRequired && string.IsNullOrEmpty(headerValue))
            {
                result.Result = false;
                result.Errors[headerKey] = "required header.";
            }
        }

        return result;
    }

    public static bool IsGuid(this string input)
    {
        Guid result;
        return Guid.TryParseExact(input, "D", out result);
    }
}
