using CinemaTicketingSystem.Domain.Core;
using CinemaTicketingSystem.Domain.Core.Exceptions;
using CinemaTicketingSystem.Domain.ValueObjects;

namespace CinemaTicketingSystem.Domain.Ticketing.Tickets;

public class Ticket : Entity<Guid>
{
    public Ticket(SeatNumber seatNumber, Price price)
    {
        Id = Guid.CreateVersion7();
        SeatNumber = seatNumber;
        Price = price;
        TicketCode = GenerateTicketCode();
        IsUsed = false;
    }

    protected Ticket()
    {
    }

    public virtual TicketPurchase TicketPurchase { get; private set; } = null!;

    public SeatNumber SeatNumber { get; } = null!;
    public Price Price { get; } = null!;
    public string TicketCode { get; } = null!;
    public bool IsUsed { get; private set; }
    public DateTime? UsedAt { get; private set; }

    public bool CanBeUsed()
    {
        return !IsUsed;
    }

    public string GetTicketInfo()
    {
        return $"Ticket: {TicketCode}, Seat: {SeatNumber}, Price: {Price}";
    }

    private static string GenerateTicketCode()
    {
        const string chars = "abcdefghijklmnopqrstuvwxyz";
        var random = new Random();

        return new string(Enumerable.Range(0, 6)
            .Select(_ => chars[random.Next(chars.Length)])
            .ToArray());
    }

    public void MarkAsUsed()
    {
        if (IsUsed)
            throw new BusinessException(ErrorCodes.TicketAlreadyUsed)
                .AddData(TicketCode);

        IsUsed = true;
        UsedAt = DateTime.UtcNow;
    }
}