#region

using CinemaTicketingSystem.SharedKernel;
using CinemaTicketingSystem.SharedKernel.Entities;
using CinemaTicketingSystem.SharedKernel.Exceptions;
using CinemaTicketingSystem.SharedKernel.ValueObjects;

#endregion

namespace CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Issuance;

public class Ticket : Entity<Guid>
{
    public Ticket(SeatPosition seatPosition, Price price)
    {
        Id = Guid.CreateVersion7();
        SeatPosition = seatPosition;
        Price = price;
        TicketCode = GenerateTicketCode();
        IsUsed = false;
    }

    protected Ticket()
    {
    }

    public virtual TicketIssuance TicketIssuance { get; private set; }

    public SeatPosition SeatPosition { get; }
    public Price Price { get; }
    public string TicketCode { get; }
    public bool IsUsed { get; private set; }
    public DateTime? UsedAt { get; private set; }

    public bool CanBeUsed()
    {
        return !IsUsed;
    }

    public string GetTicketInfo()
    {
        return $"Ticket: {TicketCode}, Seat: {SeatPosition}, Price: {Price}";
    }

    private static string GenerateTicketCode()
    {
        const string chars = "abcdefghijklmnopqrstuvwxyz";
        Random random = new Random();

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