using CinemaTicketingSystem.Domain.Ticketing.Tickets.DomainEvents;
using CinemaTicketingSystem.Domain.Ticketing.ValueObjects;

namespace CinemaTicketingSystem.Domain.Ticketing.Tickets;

public class MovieTicket : AggregateRoot<Guid>
{
    private const int MaxTicketsPerPurchase = 10;

    private readonly List<TicketSale> ticketSales = [];

    private MovieTicket()
    {
    }

    public MovieTicket(Guid movieHallId, Guid customerId)
    {
        Id = Guid.CreateVersion7();
        MovieHallIdId = movieHallId;
        CustomerId = customerId;
    }


    public Guid? CustomerId { get; }
    public Guid MovieHallIdId { get; private set; }
    public bool IsDiscountApplied { get; private set; }

    public virtual IReadOnlyCollection<TicketSale> TicketSales => ticketSales.AsReadOnly();

    public void AddTicket(TicketSale ticket)
    {
        if (ticketSales.Count >= MaxTicketsPerPurchase)
            // throw new MaxTicketLimitExceededException(MaxTicketsPerPurchase);

            if (ticketSales.Any(t => t.SeatNumber == ticket.SeatNumber))
                // throw new DuplicateSeatException(ticket.SeatNumber);

                ticketSales.Add(ticket);
        ApplyBulkDiscountIfEligible();
        AddDomainEvent(new TicketPurchasedEvent(ticket.Id, CustomerId!.Value, ticket.Price));
    }

    public void RemoveTicket(SeatNumber seatNumber)
    {
        var ticket = ticketSales.FirstOrDefault(t => t.SeatNumber == seatNumber);
        if (ticket is null)
            // throw new TicketNotFoundException(seatNumber);

            ticketSales.Remove(ticket);
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
        if (ticketSales.Count >= 3 && !IsDiscountApplied) IsDiscountApplied = true;
    }

    public Price GetTotalPrice()
    {
        var baseTotal = ticketSales
            .Select(t => t.Price)
            .Aggregate((total, next) => total + next);

        if (!IsDiscountApplied) return baseTotal;

        var discountMultiplier = 0.9m; // 10% off
        return new Price(baseTotal.Amount * discountMultiplier, baseTotal.Currency);
    }

    public void MarkTicketsAsUsed()
    {
        foreach (var ticket in ticketSales)
        {
            ticket.MarkAsUsed();
            AddDomainEvent(new TicketUsedEvent(ticket.Id, CustomerId!.Value, DateTime.UtcNow));
        }
    }

    public bool HasTicketForSeat(SeatNumber seatNumber)
    {
        return ticketSales.Any(t => t.SeatNumber == seatNumber);
    }
}