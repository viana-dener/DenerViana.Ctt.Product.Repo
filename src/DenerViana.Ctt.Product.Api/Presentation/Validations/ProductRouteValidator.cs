using DenerViana.Ctt.Product.Api.Application.Models.Request;
using DenerViana.Ctt.Product.Api.Tools;
using FluentValidation;

namespace DenerViana.Ctt.Product.Api.Presentation.Validations;

public class ProductRouteValidator : AbstractValidator<ProductRequest>
{
    #region Constructors

    public ProductRouteValidator(IHttpContextAccessor context)
    {
        ValidateRoute(context);
        ValidateFields();
    }

    #endregion

    #region Private Methods

    private void ValidateRoute(IHttpContextAccessor context)
    {
        if (context.HttpContext?.Request.Method == HttpMethod.Get.Method ||
            context.HttpContext?.Request.Method == HttpMethod.Put.Method ||
            context.HttpContext?.Request.Method == HttpMethod.Patch.Method ||
            context.HttpContext?.Request.Method == HttpMethod.Delete.Method)
        {
            var routeData = context.HttpContext.GetRouteData();
            var id = routeData.Values["id"]?.ToString();

            RuleFor(x => id)
                .NotEmpty()
                .WithMessage("The field id is required.")
                .MaximumLength(36)
                .WithMessage("The id field must have a maximum of 36 characters.")
                .Must(ValidateGuid)
                .WithMessage("The value entered in the field is not a valid Guid.");
        }
    }

    private void ValidateFields()
    {
        RuleFor(model => model.Description)
            .NotEmpty()
            .WithMessage("The field description is required.")
            .MinimumLength(3)
            .WithMessage("The description field must have at least 3 characters.")
            .MaximumLength(255)
            .WithMessage("The description field must have a maximum of 255 characters.");

        RuleFor(model => model.Stock)
            .GreaterThanOrEqualTo(0)
            .WithMessage("The stock must be greater than or equal to 0.");

        RuleFor(model => model.Price)
            .GreaterThan(0)
            .WithMessage("The price must be greater than 0.");

        RuleFor(model => model.Categories)
            .NotEmpty()
            .WithMessage("At least one category is required.")
            .Must(list => list.All(g => g != Guid.Empty))
            .WithMessage("All categories must be valid GUIDs.");
    }

    private static bool ValidateGuid(string id)
    {
        return id.IsGuid();
    }

    #endregion
}
