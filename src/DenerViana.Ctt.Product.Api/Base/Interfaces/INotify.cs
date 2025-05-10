namespace DenerViana.Ctt.Product.Api.Base.Interfaces;

public interface INotify
{
    ICollection<string> GetErrors();
    void AddError(string error);
    void ClearErrors();
}
