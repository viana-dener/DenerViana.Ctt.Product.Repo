using DenerViana.Ctt.Product.Api.Base;

namespace DenerViana.Ctt.Product.Test.Base;

public class NotifyTests
{
    [Fact(DisplayName = "Should add error to list")]
    [Trait("Base", "Notify")]
    public void AddError_ShouldAddErrorToList()
    {
        // Arrange
        var notify = new Notify();
        var errorMessage = "Test error message";

        // Act
        notify.AddError(errorMessage);
        var errors = notify.GetErrors();

        // Assert
        Assert.Single(errors);
        Assert.Contains(errorMessage, errors);
    }

    [Fact(DisplayName = "Should return all added errors")]
    [Trait("Base", "Notify")]
    public void GetErrors_ShouldReturnAllAddedErrors()
    {
        // Arrange
        var notify = new Notify();
        var errorMessages = new List<string> { "Error 1", "Error 2", "Error 3" };

        // Act
        foreach (var error in errorMessages)
        {
            notify.AddError(error);
        }
        var errors = notify.GetErrors();

        // Assert
        Assert.Equal(errorMessages.Count, errors.Count);
        Assert.All(errorMessages, error => Assert.Contains(error, errors));
    }

    [Fact(DisplayName = "Should remove all errors")]
    [Trait("Base", "Notify")]
    public void ClearErrors_ShouldRemoveAllErrors()
    {
        // Arrange
        var notify = new Notify();
        notify.AddError("Error 1");
        notify.AddError("Error 2");

        // Act
        notify.ClearErrors();
        var errors = notify.GetErrors();

        // Assert
        Assert.Empty(errors);
    }

    [Fact(DisplayName = "Should not throw when adding duplicate errors")]
    [Trait("Base", "Notify")]
    public void AddError_ShouldNotThrow_WhenAddingDuplicateErrors()
    {
        // Arrange
        var notify = new Notify();
        var errorMessage = "Duplicate error";

        // Act
        notify.AddError(errorMessage);
        notify.AddError(errorMessage);
        var errors = notify.GetErrors();

        // Assert
        Assert.Equal(2, errors.Count);
        Assert.Equal(errorMessage, errors.First());
    }
}