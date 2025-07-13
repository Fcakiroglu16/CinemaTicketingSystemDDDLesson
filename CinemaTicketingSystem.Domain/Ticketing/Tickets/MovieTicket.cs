using CinemaTicketingSystem.Domain.Ticketing.Tickets.DomainEvents;
using CinemaTicketingSystem.Domain.Ticketing.Tickets.Exceptions;
using CinemaTicketingSystem.Domain.Ticketing.ValueObjects;

namespace CinemaTicketingSystem.Domain.Ticketing.Tickets;

public class MovieTicket : AggregateRoot<Guid>
{
    private const int MaxTicketsPerPurchase = 10;

    // Existing properties
    public Guid? CustomerId { get; private set; }
    public Guid MovieSessionId { get; private set; }
    public bool IsDiscountApplied { get; private set; }

    private List<TicketSale> _purchasedTickets { get; } = [];

    public IReadOnlyCollection<TicketSale> PurchasedTickets => _purchasedTickets.AsReadOnly();

    public void Create(Guid movieSessionId, Guid customerId)
    {
        Id = Guid.CreateVersion7();
        MovieSessionId = movieSessionId;
        CustomerId = customerId;
    }

    public void AddTicket(TicketSale ticket)
    {
        if (_purchasedTickets.Count >= MaxTicketsPerPurchase)
            throw new MaxTicketLimitExceededException(MaxTicketsPerPurchase);

        if (_purchasedTickets.Any(t => t.SeatNumber == ticket.SeatNumber))
            throw new DuplicateSeatException(ticket.SeatNumber);

        _purchasedTickets.Add(ticket);
        ApplyBulkDiscountIfEligible();
        AddDomainEvent(new TicketPurchasedEvent(ticket.Id, CustomerId!.Value, ticket.Price));
    }

    public void RemoveTicket(SeatNumber seatNumber)
    {
        var ticket = _purchasedTickets.FirstOrDefault(t => t.SeatNumber == seatNumber);
        if (ticket is null)
            throw new TicketNotFoundException(seatNumber);

        _purchasedTickets.Remove(ticket);
        AddDomainEvent(new TicketReleasedEvent(ticket.Id));

        ApplyBulkDiscountIfEligible();
    }

    public void AddTickets(IEnumerable<TicketSale> tickets)
    {
        foreach (var ticket in tickets) AddTicket(ticket);
        ApplyBulkDiscountIfEligible();
    }

    private void ApplyBulkDiscountIfEligible()
    {
        if (_purchasedTickets.Count >= 3 && !IsDiscountApplied) IsDiscountApplied = true;
    }

    public Price GetTotalPrice()
    {
        var baseTotal = _purchasedTickets
            .Select(t => t.Price)
            .Aggregate((total, next) => total + next);

        if (!IsDiscountApplied) return baseTotal;

        var discountMultiplier = 0.9m; // 10% off
        return new Price(baseTotal.Amount * discountMultiplier, baseTotal.Currency);
    }

    public void MarkTicketsAsUsed()
    {
        foreach (var ticket in _purchasedTickets)
        {
            ticket.MarkAsUsed();
            AddDomainEvent(new TicketUsedEvent(ticket.Id, CustomerId!.Value, DateTime.UtcNow));
        }
    }

    public bool HasTicketForSeat(SeatNumber seatNumber)
    {
        return _purchasedTickets.Any(t => t.SeatNumber == seatNumber);
    }
}