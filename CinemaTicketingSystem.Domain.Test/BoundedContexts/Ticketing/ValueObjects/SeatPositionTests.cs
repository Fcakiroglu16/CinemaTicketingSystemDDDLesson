#region

using System;
using CinemaTicketingSystem.SharedKernel.ValueObjects;
using Xunit;

#endregion

namespace CinemaTicketingSystem.Domain.Test.BoundedContexts.Ticketing.ValueObjects;

public class SeatPositionTests
{
    [Fact]
    public void Constructor_ShouldCreateSeatPosition_WithValidParameters()
    {
        // Arrange
        var row = "A";
        var number = 1;
        
        // Act
        var seatPosition = new SeatPosition(row, number);
        
        // Assert
        Assert.Equal(row, seatPosition.Row);
        Assert.Equal(number, seatPosition.Number);
    }
    
    [Fact]
    public void Equals_ShouldReturnTrue_WhenSeatPositionsAreEqual()
    {
        // Arrange
        var seatPosition1 = new SeatPosition("A", 1);
        var seatPosition2 = new SeatPosition("A", 1);
        
        // Act & Assert
        Assert.Equal(seatPosition1, seatPosition2);
        Assert.True(seatPosition1.Equals(seatPosition2));
        Assert.False(!seatPosition1.Equals(seatPosition2));
    }
    
    [Fact]
    public void Equals_ShouldReturnFalse_WhenSeatPositionsAreDifferent()
    {
        // Arrange
        var seatPosition1 = new SeatPosition("A", 1);
        var seatPosition2 = new SeatPosition("A", 2);
        var seatPosition3 = new SeatPosition("B", 1);
        
        // Act & Assert
        Assert.NotEqual(seatPosition1, seatPosition2);
        Assert.NotEqual(seatPosition1, seatPosition3);
        Assert.False(seatPosition1.Equals(seatPosition2));
        Assert.False(seatPosition1.Equals(seatPosition3));
    }
    
    [Fact]
    public void ToString_ShouldReturnFormattedString()
    {
        // Arrange
        var row = "A";
        var number = 1;
        var seatPosition = new SeatPosition(row, number);
        
        // Act
        var result = seatPosition.ToString();
        
        // Assert
        Assert.Contains(row, result);
        Assert.Contains(number.ToString(), result);
    }
    
    [Fact]
    public void GetHashCode_ShouldReturnSameValue_ForEqualSeatPositions()
    {
        // Arrange
        var seatPosition1 = new SeatPosition("A", 1);
        var seatPosition2 = new SeatPosition("A", 1);
        
        // Act
        var hashCode1 = seatPosition1.GetHashCode();
        var hashCode2 = seatPosition2.GetHashCode();
        
        // Assert
        Assert.Equal(hashCode1, hashCode2);
    }
    
    [Fact]
    public void GetHashCode_ShouldReturnDifferentValues_ForDifferentSeatPositions()
    {
        // Arrange
        var seatPosition1 = new SeatPosition("A", 1);
        var seatPosition2 = new SeatPosition("B", 2);
        
        // Act
        var hashCode1 = seatPosition1.GetHashCode();
        var hashCode2 = seatPosition2.GetHashCode();
        
        // Assert
        Assert.NotEqual(hashCode1, hashCode2);
    }
}