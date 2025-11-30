#region

using CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Issuance;
using CinemaTicketingSystem.SharedKernel;
using CinemaTicketingSystem.SharedKernel.Exceptions;
using CinemaTicketingSystem.SharedKernel.ValueObjects;

#endregion

namespace CinemaTicketingSystem.Domain.Test.BoundedContexts.Ticketing.Issuance;

public class TicketTests
{
    private readonly SeatPosition _validSeatPosition = new("A", 1);
    private readonly Price _validPrice = new(15.0m, "USD");

    [Fact]
    public void Constructor_ShouldCreateTicket_WithValidParameters()
    {
        // Act
        Ticket ticket = new Ticket(_validSeatPosition, _validPrice);

        // Assert
        Assert.Equal(_validSeatPosition, ticket.SeatPosition);
        Assert.Equal(_validPrice, ticket.Price);
        Assert.NotNull(ticket.TicketCode);
        Assert.NotEmpty(ticket.TicketCode);
        Assert.Equal(6, ticket.TicketCode.Length);
        Assert.False(ticket.IsUsed);
        Assert.Null(ticket.UsedAt);
        Assert.NotEqual(Guid.Empty, ticket.Id);
    }

    [Fact]
    public void CanBeUsed_ShouldReturnTrue_WhenTicketIsNotUsed()
    {
        // Arrange
        Ticket ticket = new Ticket(_validSeatPosition, _validPrice);

        // Act
        bool canBeUsed = ticket.CanBeUsed();

        // Assert
        Assert.True(canBeUsed);
    }

    [Fact]
    public void CanBeUsed_ShouldReturnFalse_WhenTicketIsUsed()
    {
        // Arrange
        Ticket ticket = new Ticket(_validSeatPosition, _validPrice);
        ticket.MarkAsUsed();

        // Act
        bool canBeUsed = ticket.CanBeUsed();

        // Assert
        Assert.False(canBeUsed);
    }

    [Fact]
    public void GetTicketInfo_ShouldReturnFormattedInfo()
    {
        // Arrange
        Ticket ticket = new Ticket(_validSeatPosition, _validPrice);

        // Act
        string ticketInfo = ticket.GetTicketInfo();

        // Assert
        Assert.Contains(ticket.TicketCode, ticketInfo);
        Assert.Contains(_validSeatPosition.ToString(), ticketInfo);
        Assert.Contains(_validPrice.ToString(), ticketInfo);
    }

    [Fact]
    public void MarkAsUsed_ShouldMarkTicketAsUsed_WhenTicketIsNotUsed()
    {
        // Arrange
        Ticket ticket = new Ticket(_validSeatPosition, _validPrice);

        // Act
        ticket.MarkAsUsed();

        // Assert
        Assert.True(ticket.IsUsed);
        Assert.NotNull(ticket.UsedAt);
    }

    [Fact]
    public void MarkAsUsed_ShouldThrowException_WhenTicketIsAlreadyUsed()
    {
        // Arrange
        Ticket ticket = new Ticket(_validSeatPosition, _validPrice);
        ticket.MarkAsUsed(); // Mark as used first time

        // Act & Assert
        BusinessException exception = Assert.Throws<BusinessException>(() => ticket.MarkAsUsed());
        Assert.Equal(ErrorCodes.TicketAlreadyUsed, exception.ErrorCode);
    }

    [Fact]
    public void GenerateTicketCode_ShouldCreateRandomCode_WithCorrectLength()
    {
        // Arrange
        Ticket ticket1 = new Ticket(_validSeatPosition, _validPrice);
        Ticket ticket2 = new Ticket(_validSeatPosition, _validPrice);

        // Act & Assert
        Assert.Equal(6, ticket1.TicketCode.Length);
        Assert.Equal(6, ticket2.TicketCode.Length);
        Assert.NotEqual(ticket1.TicketCode, ticket2.TicketCode); // Random codes should be different
    }
}