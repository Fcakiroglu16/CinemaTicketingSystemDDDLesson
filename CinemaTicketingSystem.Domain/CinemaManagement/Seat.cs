using CinemaTicketingSystem.Domain.Core;

namespace CinemaTicketingSystem.Domain.CinemaManagement;

public class Seat : Entity<Guid>
{
    protected Seat()
    {
    }

    // Constructor
    public Seat(string row, int number, SeatType type)
    {
        if (string.IsNullOrWhiteSpace(row))
            throw new ArgumentException("Row cannot be empty");
        if (number <= 0)
            throw new ArgumentException("Seat number must be positive");

        Row = row.ToUpper();
        Number = (short)number;
        Type = type;
        Id = Guid.CreateVersion7();
    }

    public string Row { get; } = null!;
    public short Number { get; }
    public SeatType Type { get; private set; }
    public bool IsAvailable { get; private set; } = true;

    public virtual CinemaHall CinemaHall { get; set; } = null!;


    // Business behavior methods
    public void ChangeSeatType(SeatType newType)
    {
        Type = newType;
    }

    public void SetAvailability(bool isAvailable)
    {
        IsAvailable = isAvailable;
    }

    public string GetSeatIdentifier()
    {
        return $"{Row}{Number:D2}";
    }

    public bool IsAccessible()
    {
        return Type == SeatType.Accessible;
    }

    public bool IsVIP()
    {
        return Type == SeatType.VIP;
    }

    public bool IsRegular()
    {
        return Type == SeatType.Regular;
    }
}