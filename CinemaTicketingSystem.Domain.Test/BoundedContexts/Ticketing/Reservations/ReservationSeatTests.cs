#region

using CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Reservations;
using CinemaTicketingSystem.SharedKernel.ValueObjects;

#endregion

namespace CinemaTicketingSystem.Domain.Test.BoundedContexts.Ticketing.Reservations;

public class ReservationSeatTests
{
    private readonly SeatPosition _validSeatPosition = new("A", 1);

    [Fact]
    public void Constructor_ShouldCreateReservationSeat_WithValidSeatPosition()
    {
        // Act
        ReservationSeat reservationSeat = new ReservationSeat(_validSeatPosition);

        // Assert
        Assert.Equal(_validSeatPosition, reservationSeat.SeatPosition);
        Assert.NotEqual(Guid.Empty, reservationSeat.Id);
    }

    [Fact]
    public void GetSeatInfo_ShouldReturnFormattedSeatInfo()
    {
        // Arrange
        ReservationSeat reservationSeat = new ReservationSeat(_validSeatPosition);

        // Act
        string seatInfo = reservationSeat.GetSeatInfo();

        // Assert
        Assert.Contains(_validSeatPosition.ToString(), seatInfo);
        Assert.Contains("Seat:", seatInfo);
    }
}