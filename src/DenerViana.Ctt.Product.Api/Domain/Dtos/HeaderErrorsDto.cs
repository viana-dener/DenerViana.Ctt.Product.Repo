namespace DenerViana.Ctt.Product.Api.Domain.Dtos;

public class HeaderErrorsDto
{
    public bool Result { get; set; }

    public Dictionary<string, string> Errors { get; set; }
}
