#region

using CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Issuance.DomainEvents;
using CinemaTicketingSystem.SharedKernel;
using CinemaTicketingSystem.SharedKernel.AggregateRoot;
using CinemaTicketingSystem.SharedKernel.Exceptions;
using CinemaTicketingSystem.SharedKernel.ValueObjects;

#endregion

namespace CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Issuance;

public enum TicketIssuanceStatus
{
    Created,
    Confirmed,
    Cancelled
}

public class TicketIssuance : AggregateRoot<Guid>
{
    private const int MaxTicketsPerPurchase = 10;

    private List<Ticket> _ticketList = [];

    protected TicketIssuance()
    {
    }

    public TicketIssuance(Guid scheduleId, Guid customerId, DateOnly screeningDate)
    {
        Id = Guid.CreateVersion7();
        ScheduledMovieShowId = scheduleId;
        CustomerId = customerId;
        ScreeningDate = screeningDate;
        Status = TicketIssuanceStatus.Created;
    }

    public void Confirm()
    {
        Status = TicketIssuanceStatus.Confirmed;
    }

    public void Cancel()
    {
        Status = TicketIssuanceStatus.Cancelled;
    }


    public Guid? CustomerId { get; }
    public Guid ScheduledMovieShowId { get; }
    public DateOnly ScreeningDate { get; private set; }

    public Guid? PurchaseId { get; set; }

    public bool IsDiscountApplied { get; private set; }

    public TicketIssuanceStatus Status { get; private set; }

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