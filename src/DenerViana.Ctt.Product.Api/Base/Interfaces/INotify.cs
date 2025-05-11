namespace DenerViana.Ctt.Product.Api.Base.Interfaces;

public interface INotify
{
    ICollection<string> GetErrors();
    int GetStatusCode();
    void AddError(string error);
    void AddError(string error, int statusCode);
    void ClearErrors();
}
