#region

using System;
using System.Collections.Generic;
using System.Linq;
using CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Reservations;
using CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.ValueObjects;
using CinemaTicketingSystem.SharedKernel;
using CinemaTicketingSystem.SharedKernel.Exceptions;
using CinemaTicketingSystem.SharedKernel.ValueObjects;
using Xunit;

#endregion

namespace CinemaTicketingSystem.Domain.Test.BoundedContexts.Ticketing.Reservations;

public class ReservationTests
{
    private readonly Guid _validScheduleId = Guid.NewGuid();
    private readonly CustomerId _validCustomerId = CustomerId.New();
    private readonly DateOnly _validScreeningDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1));
    private readonly SeatPosition _validSeatPosition = new("A", 1);
    
    [Fact]
    public void Constructor_ShouldCreateReservation_WithValidParameters()
    {
        // Act
        var reservation = new Reservation(_validScheduleId, _validCustomerId, _validScreeningDate);
        
        // Assert
        Assert.Equal(_validScheduleId, reservation.ScheduledMovieShowId);
        Assert.Equal(_validCustomerId, reservation.CustomerId);
        Assert.Equal(_validScreeningDate, reservation.ScreeningDate);
        Assert.Equal(ReservationStatus.Created, reservation.Status);
        Assert.Empty(reservation.ReservationSeatList);
        Assert.Null(reservation.ReservationTime);
        Assert.Null(reservation.ExpirationTime);
    }
    
    [Fact]
    public void AddSeat_ShouldAddSeatToReservation_WithValidSeat()
    {
        // Arrange
        var reservation = new Reservation(_validScheduleId, _validCustomerId, _validScreeningDate);
        var reservationSeat = new ReservationSeat(_validSeatPosition);
        
        // Act
        reservation.AddSeat(reservationSeat);
        
        // Assert
        Assert.Single(reservation.ReservationSeatList);
        Assert.Contains(reservationSeat, reservation.ReservationSeatList);
        Assert.True(reservation.HasSeat(_validSeatPosition));
    }
    
    [Fact]
    public void AddSeat_ShouldThrowException_WhenSeatPositionAlreadyExists()
    {
        // Arrange
        var reservation = new Reservation(_validScheduleId, _validCustomerId, _validScreeningDate);
        var seat1 = new ReservationSeat(_validSeatPosition);
        var seat2 = new ReservationSeat(_validSeatPosition); // Same position
        
        reservation.AddSeat(seat1);
        
        // Act & Assert
        Assert.Throws<BusinessException>(() => reservation.AddSeat(seat2));
    }
    
    [Fact]
    public void AddSeat_ShouldThrowException_WhenExceedingMaximumSeatsPerReservation()
    {
        // Arrange
        var reservation = new Reservation(_validScheduleId, _validCustomerId, _validScreeningDate);
        
        // Add 10 seats (maximum allowed)
        for (int i = 1; i <= 10; i++)
        {
            var seat = new ReservationSeat(new SeatPosition("A", i));
            reservation.AddSeat(seat);
        }
        
        // Try to add one more seat
        var extraSeat = new ReservationSeat(new SeatPosition("B", 1));
        
        // Act & Assert
        Assert.Throws<BusinessException>(() => reservation.AddSeat(extraSeat));
    }
    
    [Fact]
    public void RemoveSeat_ShouldRemoveSeatFromReservation_WhenValidPositionProvided()
    {
        // Arrange
        var reservation = new Reservation(_validScheduleId, _validCustomerId, _validScreeningDate);
        var reservationSeat = new ReservationSeat(_validSeatPosition);
        
        reservation.AddSeat(reservationSeat);
        
        // Act
        reservation.RemoveSeat(_validSeatPosition);
        
        // Assert
        Assert.Empty(reservation.ReservationSeatList);
        Assert.False(reservation.HasSeat(_validSeatPosition));
    }
    
    [Fact]
    public void RemoveSeat_ShouldThrowException_WhenSeatPositionDoesNotExist()
    {
        // Arrange
        var reservation = new Reservation(_validScheduleId, _validCustomerId, _validScreeningDate);
        var nonExistentPosition = new SeatPosition("Z", 99);
        
        // Act & Assert
        Assert.Throws<BusinessException>(() => reservation.RemoveSeat(nonExistentPosition));
    }
    
    [Fact]
    public void AddSeats_ShouldAddMultipleSeatsToReservation()
    {
        // Arrange
        var reservation = new Reservation(_validScheduleId, _validCustomerId, _validScreeningDate);
        var seats = new List<ReservationSeat>
        {
            new ReservationSeat(new SeatPosition("A", 1)),
            new ReservationSeat(new SeatPosition("A", 2)),
            new ReservationSeat(new SeatPosition("A", 3))
        };
        
        // Act
        reservation.AddSeats(seats);
        
        // Assert
        Assert.Equal(3, reservation.ReservationSeatList.Count);
        Assert.True(reservation.HasSeat(new SeatPosition("A", 1)));
        Assert.True(reservation.HasSeat(new SeatPosition("A", 2)));
        Assert.True(reservation.HasSeat(new SeatPosition("A", 3)));
    }
    
    [Fact]
    public void HasSeat_ShouldReturnTrue_WhenSeatExists()
    {
        // Arrange
        var reservation = new Reservation(_validScheduleId, _validCustomerId, _validScreeningDate);
        var reservationSeat = new ReservationSeat(_validSeatPosition);
        
        reservation.AddSeat(reservationSeat);
        
        // Act
        var result = reservation.HasSeat(_validSeatPosition);
        
        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public void HasSeat_ShouldReturnFalse_WhenSeatDoesNotExist()
    {
        // Arrange
        var reservation = new Reservation(_validScheduleId, _validCustomerId, _validScreeningDate);
        var nonExistentPosition = new SeatPosition("Z", 99);
        
        // Act
        var result = reservation.HasSeat(nonExistentPosition);
        
        // Assert
        Assert.False(result);
    }
    
    [Fact]
    public void Confirm_ShouldSetStatusToConfirmed_WhenReservationHasSeats()
    {
        // Arrange
        var reservation = new Reservation(_validScheduleId, _validCustomerId, _validScreeningDate);
        var reservationSeat = new ReservationSeat(_validSeatPosition);
        reservation.AddSeat(reservationSeat);
        
        var movieStartTime = new TimeOnly(20, 0); // 8:00 PM
        
        // Act
        reservation.Confirm(movieStartTime);
        
        // Assert
        Assert.Equal(ReservationStatus.Confirmed, reservation.Status);
        Assert.NotNull(reservation.ReservationTime);
        Assert.NotNull(reservation.ExpirationTime);
        
        // Verify expiration time is 4 hours before movie start time
        var expectedExpirationTime = _validScreeningDate.ToDateTime(movieStartTime).AddHours(-4);
        Assert.Equal(expectedExpirationTime, reservation.ExpirationTime);
    }
    
    [Fact]
    public void Confirm_ShouldThrowException_WhenReservationHasNoSeats()
    {
        // Arrange
        var reservation = new Reservation(_validScheduleId, _validCustomerId, _validScreeningDate);
        var movieStartTime = new TimeOnly(20, 0);
        
        // Act & Assert
        Assert.Throws<BusinessException>(() => reservation.Confirm(movieStartTime));
    }
    
    [Fact]
    public void Cancel_ShouldSetStatusToCanceled_WhenReservationIsNotExpired()
    {
        // Arrange
        var reservation = new Reservation(_validScheduleId, _validCustomerId, _validScreeningDate);
        var reservationSeat = new ReservationSeat(_validSeatPosition);
        reservation.AddSeat(reservationSeat);
        reservation.Confirm(new TimeOnly(20, 0));
        
        // Act
        reservation.Cancel();
        
        // Assert
        Assert.Equal(ReservationStatus.Canceled, reservation.Status);
    }
    
    [Fact]
    public void Cancel_ShouldDoNothing_WhenReservationIsAlreadyCanceled()
    {
        // Arrange
        var reservation = new Reservation(_validScheduleId, _validCustomerId, _validScreeningDate);
        var reservationSeat = new ReservationSeat(_validSeatPosition);
        reservation.AddSeat(reservationSeat);
        reservation.Confirm(new TimeOnly(20, 0));
        reservation.Cancel(); // Cancel first time
        
        // Act
        reservation.Cancel(); // Cancel second time
        
        // Assert
        Assert.Equal(ReservationStatus.Canceled, reservation.Status);
    }
    
    [Fact]
    public void Cancel_ShouldThrowException_WhenReservationIsExpired()
    {
        // Arrange
        var reservation = new Reservation(_validScheduleId, _validCustomerId, _validScreeningDate);
        var reservationSeat = new ReservationSeat(_validSeatPosition);
        reservation.AddSeat(reservationSeat);
        reservation.Confirm(new TimeOnly(20, 0));
        reservation.Expire();
        
        // Act & Assert
        Assert.Throws<BusinessException>(() => reservation.Cancel());
    }
    
    [Fact]
    public void Expire_ShouldSetStatusToExpired()
    {
        // Arrange
        var reservation = new Reservation(_validScheduleId, _validCustomerId, _validScreeningDate);
        var reservationSeat = new ReservationSeat(_validSeatPosition);
        reservation.AddSeat(reservationSeat);
        reservation.Confirm(new TimeOnly(20, 0));
        
        // Act
        reservation.Expire();
        
        // Assert
        Assert.Equal(ReservationStatus.Expired, reservation.Status);
    }
    
    [Fact]
    public void IsExpired_ShouldReturnTrue_WhenCurrentTimeIsAfterExpirationTime()
    {
        // Arrange
        var reservation = new Reservation(_validScheduleId, _validCustomerId, _validScreeningDate);
        var reservationSeat = new ReservationSeat(_validSeatPosition);
        reservation.AddSeat(reservationSeat);
        
        // Set expiration time to the past
        typeof(Reservation).GetProperty(nameof(Reservation.ExpirationTime))!
            .SetValue(reservation, DateTime.UtcNow.AddHours(-1));
        
        // Act
        var result = reservation.IsExpired();
        
        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public void IsExpired_ShouldReturnFalse_WhenCurrentTimeIsBeforeExpirationTime()
    {
        // Arrange
        var reservation = new Reservation(_validScheduleId, _validCustomerId, _validScreeningDate);
        var reservationSeat = new ReservationSeat(_validSeatPosition);
        reservation.AddSeat(reservationSeat);
        
        // Set expiration time to the future
        typeof(Reservation).GetProperty(nameof(Reservation.ExpirationTime))!
            .SetValue(reservation, DateTime.UtcNow.AddHours(1));
        
        // Act
        var result = reservation.IsExpired();
        
        // Assert
        Assert.False(result);
    }
}