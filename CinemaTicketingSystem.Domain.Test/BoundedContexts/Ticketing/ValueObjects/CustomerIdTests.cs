#region

using System;
using CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.ValueObjects;
using Xunit;

#endregion

namespace CinemaTicketingSystem.Domain.Test.BoundedContexts.Ticketing.ValueObjects;

public class CustomerIdTests
{
    [Fact]
    public void Constructor_ShouldCreateCustomerId_WithValidGuid()
    {
        // Arrange
        var guid = Guid.NewGuid();
        
        // Act
        var customerId = new CustomerId(guid);
        
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
        var customerId = CustomerId.New();
        
        // Assert
        Assert.NotEqual(Guid.Empty, customerId.Value);
    }
    
    [Fact]
    public void From_ShouldCreateCustomerId_FromValidGuid()
    {
        // Arrange
        var guid = Guid.NewGuid();
        
        // Act
        var customerId = CustomerId.From(guid);
        
        // Assert
        Assert.Equal(guid, customerId.Value);
    }
    
    [Fact]
    public void From_ShouldCreateCustomerId_FromValidGuidString()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var guidString = guid.ToString();
        
        // Act
        var customerId = CustomerId.From(guidString);
        
        // Assert
        Assert.Equal(guid, customerId.Value);
    }
    
    [Fact]
    public void From_ShouldThrowException_WhenStringIsNotValidGuid()
    {
        // Arrange
        var invalidGuidString = "not-a-guid";
        
        // Act & Assert
        Assert.Throws<ArgumentException>(() => CustomerId.From(invalidGuidString));
    }
    
    [Fact]
    public void Equals_ShouldReturnTrue_WhenCustomerIdsAreEqual()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var customerId1 = new CustomerId(guid);
        var customerId2 = new CustomerId(guid);
        
        // Act & Assert
        Assert.Equal(customerId1, customerId2);
        Assert.True(customerId1.Equals(customerId2));
        Assert.False(!customerId1.Equals(customerId2));
    }
    
    [Fact]
    public void Equals_ShouldReturnFalse_WhenCustomerIdsAreDifferent()
    {
        // Arrange
        var customerId1 = new CustomerId(Guid.NewGuid());
        var customerId2 = new CustomerId(Guid.NewGuid());
        
        // Act & Assert
        Assert.NotEqual(customerId1, customerId2);
        Assert.False(customerId1.Equals(customerId2));
    }
    
    [Fact]
    public void ToString_ShouldReturnGuidString()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var customerId = new CustomerId(guid);
        
        // Act
        var result = customerId.ToString();
        
        // Assert
        Assert.Equal(guid.ToString(), result);
    }
    
    [Fact]
    public void ImplicitOperator_ShouldConvertCustomerIdToGuid()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var customerId = new CustomerId(guid);
        
        // Act
        Guid result = customerId;
        
        // Assert
        Assert.Equal(guid, result);
    }
    
    [Fact]
    public void ImplicitOperator_ShouldConvertGuidToCustomerId()
    {
        // Arrange
        var guid = Guid.NewGuid();
        
        // Act
        CustomerId customerId = guid;
        
        // Assert
        Assert.Equal(guid, customerId.Value);
    }
    
    [Fact]
    public void GetHashCode_ShouldReturnSameValue_ForEqualCustomerIds()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var customerId1 = new CustomerId(guid);
        var customerId2 = new CustomerId(guid);
        
        // Act
        var hashCode1 = customerId1.GetHashCode();
        var hashCode2 = customerId2.GetHashCode();
        
        // Assert
        Assert.Equal(hashCode1, hashCode2);
    }
}