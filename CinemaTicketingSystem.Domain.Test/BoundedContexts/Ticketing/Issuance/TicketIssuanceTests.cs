#region

using CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Issuance;
using CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.ValueObjects;
using CinemaTicketingSystem.SharedKernel;
using CinemaTicketingSystem.SharedKernel.Exceptions;
using CinemaTicketingSystem.SharedKernel.ValueObjects;

#endregion

namespace CinemaTicketingSystem.Domain.Test.BoundedContexts.Ticketing.Issuance;

public class TicketIssuanceTests
{
    private readonly Guid _validScheduleId = Guid.NewGuid();
    private readonly CustomerId _validCustomerId = CustomerId.New();
    private readonly DateOnly _validScreeningDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1));
    private readonly SeatPosition _validSeatPosition = new("A", 1);
    private readonly Price _validPrice = new(15.0m, "USD");

    [Fact]
    public void Constructor_ShouldCreateTicketIssuance_WithValidParameters()
    {
        // Act
        TicketIssuance ticketIssuance = new TicketIssuance(_validScheduleId, _validCustomerId, _validScreeningDate);

        // Assert
        Assert.Equal(_validScheduleId, ticketIssuance.ScheduledMovieShowId);
        Assert.Equal(_validCustomerId, ticketIssuance.CustomerId);
        Assert.Equal(_validScreeningDate, ticketIssuance.ScreeningDate);
        Assert.Equal(TicketIssuanceStatus.Created, ticketIssuance.Status);
        Assert.False(ticketIssuance.IsDiscountApplied);
        Assert.Empty(ticketIssuance.TicketList);
    }

    [Fact]
    public void Confirm_ShouldChangeStatus_ToConfirmed()
    {
        // Arrange
        TicketIssuance ticketIssuance = new TicketIssuance(_validScheduleId, _validCustomerId, _validScreeningDate);

        // Act
        ticketIssuance.Confirm();

        // Assert
        Assert.Equal(TicketIssuanceStatus.Confirmed, ticketIssuance.Status);
    }

    [Fact]
    public void Cancel_ShouldChangeStatus_ToCancelled()
    {
        // Arrange
        TicketIssuance ticketIssuance = new TicketIssuance(_validScheduleId, _validCustomerId, _validScreeningDate);

        // Act
        ticketIssuance.Cancel();

        // Assert
        Assert.Equal(TicketIssuanceStatus.Cancelled, ticketIssuance.Status);
    }

    [Fact]
    public void AddTicket_ShouldAddTicket_WithValidParameters()
    {
        // Arrange
        TicketIssuance ticketIssuance = new TicketIssuance(_validScheduleId, _validCustomerId, _validScreeningDate);

        // Act
        ticketIssuance.AddTicket(_validSeatPosition, _validPrice);

        // Assert
        Assert.Single(ticketIssuance.TicketList);
        Ticket ticket = ticketIssuance.TicketList.First();
        Assert.Equal(_validSeatPosition, ticket.SeatPosition);
        Assert.Equal(_validPrice, ticket.Price);
    }

    [Fact]
    public void AddTicket_ShouldApplyDiscount_WhenThreeOrMoreTicketsAdded()
    {
        // Arrange
        TicketIssuance ticketIssuance = new TicketIssuance(_validScheduleId, _validCustomerId, _validScreeningDate);

        // Act
        ticketIssuance.AddTicket(new SeatPosition("A", 1), _validPrice);
        ticketIssuance.AddTicket(new SeatPosition("A", 2), _validPrice);
        ticketIssuance.AddTicket(new SeatPosition("A", 3), _validPrice);

        // Assert
        Assert.Equal(3, ticketIssuance.TicketList.Count);
        Assert.True(ticketIssuance.IsDiscountApplied);

        // The total price should be discounted by 10%
        Price expectedTotal = new Price(_validPrice.Amount * 3 * 0.9m, _validPrice.Currency);
        Assert.Equal(expectedTotal.Amount, ticketIssuance.GetTotalPrice().Amount);
    }

    [Fact]
    public void AddTicket_ShouldThrowException_WhenExceedingMaximumTicketsPerPurchase()
    {
        // Arrange
        TicketIssuance ticketIssuance = new TicketIssuance(_validScheduleId, _validCustomerId, _validScreeningDate);

        // Add 10 tickets (maximum allowed)
        for (int i = 1; i <= 10; i++)
        {
            ticketIssuance.AddTicket(new SeatPosition("A", i), _validPrice);
        }

        // Act & Assert
        BusinessException exception = Assert.Throws<BusinessException>(() =>
            ticketIssuance.AddTicket(new SeatPosition("B", 1), _validPrice));
        Assert.Equal(ErrorCodes.MaxTicketsExceeded, exception.ErrorCode);
    }

    [Fact]
    public void AddTicket_ShouldThrowException_WhenSeatPositionAlreadyExists()
    {
        // Arrange
        TicketIssuance ticketIssuance = new TicketIssuance(_validScheduleId, _validCustomerId, _validScreeningDate);
        SeatPosition seatPosition = new SeatPosition("A", 1);

        // Add a ticket for seat A1
        ticketIssuance.AddTicket(seatPosition, _validPrice);

        // Act & Assert
        BusinessException exception = Assert.Throws<BusinessException>(() =>
            ticketIssuance.AddTicket(seatPosition, _validPrice));
        Assert.Equal(ErrorCodes.DuplicateSeat, exception.ErrorCode);
    }

    [Fact]
    public void RemoveTicket_ShouldRemoveTicket_WhenValidSeatPositionProvided()
    {
        // Arrange
        TicketIssuance ticketIssuance = new TicketIssuance(_validScheduleId, _validCustomerId, _validScreeningDate);
        SeatPosition seatPosition = new SeatPosition("A", 1);

        ticketIssuance.AddTicket(seatPosition, _validPrice);

        // Act
        ticketIssuance.RemoveTicket(seatPosition);

        // Assert
        Assert.Empty(ticketIssuance.TicketList);
    }

    [Fact]
    public void RemoveTicket_ShouldThrowException_WhenInvalidSeatPositionProvided()
    {
        // Arrange
        TicketIssuance ticketIssuance = new TicketIssuance(_validScheduleId, _validCustomerId, _validScreeningDate);
        SeatPosition nonExistentSeatPosition = new SeatPosition("Z", 99);

        // Act & Assert
        BusinessException exception = Assert.Throws<BusinessException>(() =>
            ticketIssuance.RemoveTicket(nonExistentSeatPosition));
        Assert.Equal(ErrorCodes.TicketNotFound, exception.ErrorCode);
    }

    [Fact]
    public void RemoveTicket_ShouldRemoveDiscount_WhenDroppingBelowThreeTickets()
    {
        // Arrange
        TicketIssuance ticketIssuance = new TicketIssuance(_validScheduleId, _validCustomerId, _validScreeningDate);

        // Add 3 tickets to trigger the discount
        ticketIssuance.AddTicket(new SeatPosition("A", 1), _validPrice);
        ticketIssuance.AddTicket(new SeatPosition("A", 2), _validPrice);
        ticketIssuance.AddTicket(new SeatPosition("A", 3), _validPrice);

        Assert.True(ticketIssuance.IsDiscountApplied);

        // Act
        ticketIssuance.RemoveTicket(new SeatPosition("A", 3));

        // Assert
        Assert.False(ticketIssuance.IsDiscountApplied);
    }

    [Fact]
    public void GetTotalPrice_ShouldReturnCorrectAmount_WithoutDiscount()
    {
        // Arrange
        TicketIssuance ticketIssuance = new TicketIssuance(_validScheduleId, _validCustomerId, _validScreeningDate);
        Price price1 = new Price(10m, "USD");
        Price price2 = new Price(15m, "USD");

        ticketIssuance.AddTicket(new SeatPosition("A", 1), price1);
        ticketIssuance.AddTicket(new SeatPosition("A", 2), price2);

        // Act
        Price totalPrice = ticketIssuance.GetTotalPrice();

        // Assert
        Assert.Equal(25m, totalPrice.Amount);
        Assert.Equal("USD", totalPrice.Currency);
    }

    [Fact]
    public void GetTotalPrice_ShouldReturnDiscountedAmount_WhenDiscountApplied()
    {
        // Arrange
        TicketIssuance ticketIssuance = new TicketIssuance(_validScheduleId, _validCustomerId, _validScreeningDate);
        Price price = new Price(10m, "USD");

        // Add 3 tickets to trigger the discount
        ticketIssuance.AddTicket(new SeatPosition("A", 1), price);
        ticketIssuance.AddTicket(new SeatPosition("A", 2), price);
        ticketIssuance.AddTicket(new SeatPosition("A", 3), price);

        // Act
        Price totalPrice = ticketIssuance.GetTotalPrice();

        // Assert
        Assert.Equal(27m, totalPrice.Amount); // 30m with 10% discount = 27m
        Assert.Equal("USD", totalPrice.Currency);
    }

    [Fact]
    public void MarkTicketsAsUsed_ShouldMarkAllTicketsAsUsed()
    {
        // Arrange
        TicketIssuance ticketIssuance = new TicketIssuance(_validScheduleId, _validCustomerId, _validScreeningDate);

        ticketIssuance.AddTicket(new SeatPosition("A", 1), _validPrice);
        ticketIssuance.AddTicket(new SeatPosition("A", 2), _validPrice);

        // Act
        ticketIssuance.MarkTicketsAsUsed();

        // Assert
        foreach (Ticket ticket in ticketIssuance.TicketList)
        {
            Assert.True(ticket.IsUsed);
            Assert.NotNull(ticket.UsedAt);
        }
    }

    [Fact]
    public void HasTicketForSeat_ShouldReturnTrue_WhenTicketForSeatExists()
    {
        // Arrange
        TicketIssuance ticketIssuance = new TicketIssuance(_validScheduleId, _validCustomerId, _validScreeningDate);
        SeatPosition seatPosition = new SeatPosition("A", 1);

        ticketIssuance.AddTicket(seatPosition, _validPrice);

        // Act
        bool hasTicket = ticketIssuance.HasTicketForSeat(seatPosition);

        // Assert
        Assert.True(hasTicket);
    }

    [Fact]
    public void HasTicketForSeat_ShouldReturnFalse_WhenTicketForSeatDoesNotExist()
    {
        // Arrange
        TicketIssuance ticketIssuance = new TicketIssuance(_validScheduleId, _validCustomerId, _validScreeningDate);
        SeatPosition seatPosition = new SeatPosition("A", 1);
        SeatPosition otherSeatPosition = new SeatPosition("B", 2);

        ticketIssuance.AddTicket(seatPosition, _validPrice);

        // Act
        bool hasTicket = ticketIssuance.HasTicketForSeat(otherSeatPosition);

        // Assert
        Assert.False(hasTicket);
    }
}