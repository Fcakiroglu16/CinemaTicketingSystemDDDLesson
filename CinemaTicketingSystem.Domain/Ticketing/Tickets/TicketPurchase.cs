using CinemaTicketingSystem.Domain.Core;
using CinemaTicketingSystem.Domain.Core.Exceptions;
using CinemaTicketingSystem.Domain.Ticketing.Tickets.DomainEvents;
using CinemaTicketingSystem.Domain.ValueObjects;

namespace CinemaTicketingSystem.Domain.Ticketing.Tickets;

public class TicketPurchase : AggregateRoot<Guid>
{
    private const int MaxTicketsPerPurchase = 10;

    private readonly List<Ticket> _ticketList = [];

    protected TicketPurchase()
    {
    }

    public TicketPurchase(Guid scheduleId, Guid customerId)
    {
        Id = Guid.CreateVersion7();
        ScheduleId = scheduleId;
        CustomerId = customerId;
    }


    public Guid? CustomerId { get; }
    public Guid ScheduleId { get; private set; }
    public bool IsDiscountApplied { get; private set; }

    public virtual IReadOnlyCollection<Ticket> TicketList => _ticketList.AsReadOnly();

    public void AddTicket(Ticket ticket)
    {
        if (_ticketList.Count >= MaxTicketsPerPurchase)
            throw new BusinessException(ErrorCodes.MaxTicketsExceeded).AddData(MaxTicketsPerPurchase);

        if (_ticketList.Any(t => t.SeatNumber == ticket.SeatNumber))
            throw new BusinessException(ErrorCodes.DuplicateSeat).AddData(ticket.SeatNumber.Row)
                .AddData(ticket.SeatNumber.Number);
        _ticketList.Add(ticket);
        ApplyBulkDiscountIfEligible();
        AddDomainEvent(new TicketPurchasedEvent(ticket.Id, CustomerId!.Value, ticket.Price));
    }

    public void RemoveTicket(SeatNumber seatNumber)
    {
        var ticket = _ticketList.FirstOrDefault(t => t.SeatNumber == seatNumber);
        if (ticket is null)
            throw new BusinessException(ErrorCodes.TicketNotFound).AddData(seatNumber.Row).AddData(seatNumber.Number);

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

    public bool HasTicketForSeat(SeatNumber seatNumber)
    {
        return _ticketList.Any(t => t.SeatNumber == seatNumber);
    }
}