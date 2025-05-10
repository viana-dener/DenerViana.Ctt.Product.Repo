using DenerViana.Ctt.Product.Api.Base.Interfaces;

namespace DenerViana.Ctt.Product.Api.Base;

public class Notify : INotify
{
    private readonly ICollection<string> Errors = [];

    public ICollection<string> GetErrors() => Errors;

    public void AddError(string error)
    {
        Errors.Add(error);
    }
    public void ClearErrors()
    {
        Errors.Clear();
    }
}
