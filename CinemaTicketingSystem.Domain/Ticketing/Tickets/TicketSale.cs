using CinemaTicketingSystem.Domain.Ticketing.Tickets.Exceptions;
using CinemaTicketingSystem.Domain.Ticketing.ValueObjects;

namespace CinemaTicketingSystem.Domain.Ticketing.Tickets;

public class TicketSale : Entity<Guid>
{
    internal TicketSale(SeatNumber seatNumber, Price price)
    {
        Id = Guid.CreateVersion7();
        SeatNumber = seatNumber;
        Price = price;
        TicketCode = GenerateTicketCode();
        IsUsed = false;
    }

    private TicketSale()
    {
    }


    // TicketSale tek başına eklenmesin diye
    //public Guid MovieTicketId { get; set; }


    public virtual MovieTicket MovieTicket { get; set; } = null!;

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
            throw new TicketAlreadyUsedException(TicketCode);

        IsUsed = true;
        UsedAt = DateTime.UtcNow;
    }
}