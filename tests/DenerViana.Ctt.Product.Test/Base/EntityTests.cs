using DenerViana.Ctt.Product.Api.Base;

namespace DenerViana.Ctt.Product.Test.Base;

public class EntityTests
{
    private class TestEntity : Entity
    {
    }

    [Fact(DisplayName = "With sameid should be equal")]
    [Trait("Base", "Entity")]
    public void Entities_WithSameId_ShouldBeEqual()
    {
        // Arrange
        var id = Guid.NewGuid();
        var entity1 = new TestEntity { Id = id };
        var entity2 = new TestEntity { Id = id };

        // Act & Assert
        Assert.True(entity1 == entity2);
        Assert.False(entity1 != entity2);
        Assert.Equal(entity1, entity2);
    }

    [Fact(DisplayName = "With different ids should not be equal")]
    [Trait("Base", "Entity")]
    public void Entities_WithDifferentIds_ShouldNotBeEqual()
    {
        // Arrange
        var entity1 = new TestEntity { Id = Guid.NewGuid() };
        var entity2 = new TestEntity { Id = Guid.NewGuid() };

        // Act & Assert
        Assert.False(entity1 == entity2);
        Assert.True(entity1 != entity2);
        Assert.NotEqual(entity1, entity2);
    }

    [Fact(DisplayName = "Tostring should return expected format")]
    [Trait("Base", "Entity")]
    public void Entity_ToString_ShouldReturnExpectedFormat()
    {
        // Arrange
        var id = Guid.NewGuid();
        var entity = new TestEntity { Id = id };

        // Act
        var result = entity.ToString();

        // Assert
        Assert.Equal($"TestEntity [Id={id}]", result);
    }

    [Fact(DisplayName = "Get hashcode should return consistent value")]
    [Trait("Base", "Entity")]
    public void Entity_GetHashCode_ShouldReturnConsistentValue()
    {
        // Arrange
        var id = Guid.NewGuid();
        var entity = new TestEntity { Id = id };

        // Act
        var hashCode1 = entity.GetHashCode();
        var hashCode2 = entity.GetHashCode();

        // Assert
        Assert.Equal(hashCode1, hashCode2);
    }

    [Fact(DisplayName = "Should return false when compared to null")]
    [Trait("Base", "Entity")]
    public void Equals_ShouldReturnFalse_WhenComparedToNull()
    {
        // Arrange
        var entity = new TestEntity { Id = Guid.NewGuid() };

        // Act & Assert
        Assert.False(entity.Equals(null));
    }

    [Fact(DisplayName = "Should return false when different entity type")]
    [Trait("Base", "Entity")]
    public void Equals_ShouldReturnFalse_WhenDifferentEntityType()
    {
        // Arrange
        var entity = new TestEntity { Id = Guid.NewGuid() };
        var other = new { entity.Id };

        // Act & Assert
        Assert.False(entity.Equals(other));
    }
}