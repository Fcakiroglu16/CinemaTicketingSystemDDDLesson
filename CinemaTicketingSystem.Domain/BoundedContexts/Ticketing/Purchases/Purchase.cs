using CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Purchases.DomainEvents;
using CinemaTicketingSystem.Domain.Core;
using CinemaTicketingSystem.Domain.Core.Exceptions;
using CinemaTicketingSystem.Domain.Ticketing.DomainEvents;
using CinemaTicketingSystem.Domain.ValueObjects;
using CinemaTicketingSystem.SharedKernel.AggregateRoot;

namespace CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Purchases;

public class Purchase : AggregateRoot<Guid>
{
    private const int MaxTicketsPerPurchase = 10;

    private readonly List<Ticket> _ticketList = [];

    protected Purchase()
    {
    }

    public Purchase(Guid scheduleId, Guid customerId)
    {
        Id = Guid.CreateVersion7();
        ScheduledMovieShowId = scheduleId;
        CustomerId = customerId;
    }


    public Guid? CustomerId { get; }
    public Guid ScheduledMovieShowId { get; }
    public bool IsDiscountApplied { get; private set; }

    public virtual IReadOnlyCollection<Ticket> TicketList => _ticketList.AsReadOnly();

    public void AddTicket(Ticket ticket)
    {
        if (_ticketList.Count >= MaxTicketsPerPurchase)
            throw new BusinessException(ErrorCodes.MaxTicketsExceeded).AddData(MaxTicketsPerPurchase);

        if (_ticketList.Any(t => t.SeatPosition == ticket.SeatPosition))
            throw new BusinessException(ErrorCodes.DuplicateSeat).AddData(ticket.SeatPosition.Row)
                .AddData(ticket.SeatPosition.Number);
        _ticketList.Add(ticket);
        ApplyBulkDiscountIfEligible();
        AddDomainEvent(new TicketPurchasedEvent(ticket.Id, ScheduledMovieShowId, CustomerId!.Value, ticket.SeatPosition,
            ticket.Price));
    }

    public void RemoveTicket(SeatPosition seatPosition)
    {
        var ticket = _ticketList.FirstOrDefault(t => t.SeatPosition == seatPosition);
        if (ticket is null)
            throw new BusinessException(ErrorCodes.TicketNotFound).AddData(seatPosition.Row)
                .AddData(seatPosition.Number);

        _ticketList.Remove(ticket);
        AddDomainEvent(new TicketReleasedEvent(ticket.Id));

        ApplyBulkDiscountIfEligible();
    }

    public void AddTickets(IEnumerable<Ticket> tickets)
    {
        foreach (var ticket in tickets) AddTicket(ticket);
        ApplyBulkDiscountIfEligible();
    }

    private void ApplyBulkDiscountIfEligible()
    {
        if (_ticketList.Count >= 3 && !IsDiscountApplied) IsDiscountApplied = true;
    }

    public Price GetTotalPrice()
    {
        var baseTotal = _ticketList
            .Select(t => t.Price)
            .Aggregate((total, next) => total + next);

        if (!IsDiscountApplied) return baseTotal;

        var discountMultiplier = 0.9m; // 10% off
        return new Price(baseTotal.Amount * discountMultiplier, baseTotal.Currency);
    }

    public void MarkTicketsAsUsed()
    {
        foreach (var ticket in _ticketList)
        {
            ticket.MarkAsUsed();
            AddDomainEvent(new TicketUsedEvent(ticket.Id, CustomerId!.Value, DateTime.UtcNow));
        }
    }

    public bool HasTicketForSeat(SeatPosition seatPosition)
    {
        return _ticketList.Any(t => t.SeatPosition == seatPosition);
    }
}