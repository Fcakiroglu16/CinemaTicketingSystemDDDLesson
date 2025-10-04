#region

using System;
using CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Reservations;
using CinemaTicketingSystem.SharedKernel.ValueObjects;
using Xunit;

#endregion

namespace CinemaTicketingSystem.Domain.Test.BoundedContexts.Ticketing.Reservations;

public class ReservationSeatTests
{
    private readonly SeatPosition _validSeatPosition = new("A", 1);
    
    [Fact]
    public void Constructor_ShouldCreateReservationSeat_WithValidSeatPosition()
    {
        // Act
        var reservationSeat = new ReservationSeat(_validSeatPosition);
        
        // Assert
        Assert.Equal(_validSeatPosition, reservationSeat.SeatPosition);
        Assert.NotEqual(Guid.Empty, reservationSeat.Id);
    }
    
    [Fact]
    public void GetSeatInfo_ShouldReturnFormattedSeatInfo()
    {
        // Arrange
        var reservationSeat = new ReservationSeat(_validSeatPosition);
        
        // Act
        var seatInfo = reservationSeat.GetSeatInfo();
        
        // Assert
        Assert.Contains(_validSeatPosition.ToString(), seatInfo);
        Assert.Contains("Seat:", seatInfo);
    }
}