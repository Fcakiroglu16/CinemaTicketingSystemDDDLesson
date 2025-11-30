#region

using CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.ValueObjects;

#endregion

namespace CinemaTicketingSystem.Domain.Test.BoundedContexts.Ticketing.ValueObjects;

public class CustomerIdTests
{
    [Fact]
    public void Constructor_ShouldCreateCustomerId_WithValidGuid()
    {
        // Arrange
        Guid guid = Guid.NewGuid();

        // Act
        CustomerId customerId = new CustomerId(guid);

        // Assert
        Assert.Equal(guid, customerId.Value);
    }

    [Fact]
    public void Constructor_ShouldThrowException_WhenGuidIsEmpty()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new CustomerId(Guid.Empty));
    }

    [Fact]
    public void New_ShouldCreateCustomerId_WithNonEmptyGuid()
    {
        // Act
        CustomerId customerId = CustomerId.New();

        // Assert
        Assert.NotEqual(Guid.Empty, customerId.Value);
    }

    [Fact]
    public void From_ShouldCreateCustomerId_FromValidGuid()
    {
        // Arrange
        Guid guid = Guid.NewGuid();

        // Act
        CustomerId customerId = CustomerId.From(guid);

        // Assert
        Assert.Equal(guid, customerId.Value);
    }

    [Fact]
    public void From_ShouldCreateCustomerId_FromValidGuidString()
    {
        // Arrange
        Guid guid = Guid.NewGuid();
        string guidString = guid.ToString();

        // Act
        CustomerId customerId = CustomerId.From(guidString);

        // Assert
        Assert.Equal(guid, customerId.Value);
    }

    [Fact]
    public void From_ShouldThrowException_WhenStringIsNotValidGuid()
    {
        // Arrange
        string invalidGuidString = "not-a-guid";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => CustomerId.From(invalidGuidString));
    }

    [Fact]
    public void Equals_ShouldReturnTrue_WhenCustomerIdsAreEqual()
    {
        // Arrange
        Guid guid = Guid.NewGuid();
        CustomerId customerId1 = new CustomerId(guid);
        CustomerId customerId2 = new CustomerId(guid);

        // Act & Assert
        Assert.Equal(customerId1, customerId2);
        Assert.True(customerId1.Equals(customerId2));
        Assert.False(!customerId1.Equals(customerId2));
    }

    [Fact]
    public void Equals_ShouldReturnFalse_WhenCustomerIdsAreDifferent()
    {
        // Arrange
        CustomerId customerId1 = new CustomerId(Guid.NewGuid());
        CustomerId customerId2 = new CustomerId(Guid.NewGuid());

        // Act & Assert
        Assert.NotEqual(customerId1, customerId2);
        Assert.False(customerId1.Equals(customerId2));
    }

    [Fact]
    public void ToString_ShouldReturnGuidString()
    {
        // Arrange
        Guid guid = Guid.NewGuid();
        CustomerId customerId = new CustomerId(guid);

        // Act
        string result = customerId.ToString();

        // Assert
        Assert.Equal(guid.ToString(), result);
    }

    [Fact]
    public void ImplicitOperator_ShouldConvertCustomerIdToGuid()
    {
        // Arrange
        Guid guid = Guid.NewGuid();
        CustomerId customerId = new CustomerId(guid);

        // Act
        Guid result = customerId;

        // Assert
        Assert.Equal(guid, result);
    }

    [Fact]
    public void ImplicitOperator_ShouldConvertGuidToCustomerId()
    {
        // Arrange
        Guid guid = Guid.NewGuid();

        // Act
        CustomerId customerId = guid;

        // Assert
        Assert.Equal(guid, customerId.Value);
    }

    [Fact]
    public void GetHashCode_ShouldReturnSameValue_ForEqualCustomerIds()
    {
        // Arrange
        Guid guid = Guid.NewGuid();
        CustomerId customerId1 = new CustomerId(guid);
        CustomerId customerId2 = new CustomerId(guid);

        // Act
        int hashCode1 = customerId1.GetHashCode();
        int hashCode2 = customerId2.GetHashCode();

        // Assert
        Assert.Equal(hashCode1, hashCode2);
    }
}