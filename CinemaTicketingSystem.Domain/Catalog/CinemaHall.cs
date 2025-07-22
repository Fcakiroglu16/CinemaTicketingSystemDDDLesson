using CinemaTicketingSystem.Domain.Core;

namespace CinemaTicketingSystem.Domain.Catalog;

public class CinemaHall : Entity<Guid>
{
    private readonly List<Seat> seats = [];

    protected CinemaHall()
    {
    }


    // Constructor
    public CinemaHall(string name, ScreeningTechnology supportedTechnologies = ScreeningTechnology.Standard)
    {
        Id = Guid.CreateVersion7();
        Name = name;
        SupportedTechnologies = supportedTechnologies;
    }

    public string Name { get; private set; }
    public ScreeningTechnology SupportedTechnologies { get; private set; } = ScreeningTechnology.Standard;
    public virtual IReadOnlyList<Seat> Seats => seats.AsReadOnly();
    public bool IsOperational { get; private set; } = true;


    public short Capacity => (short)Seats.Count;


    public virtual Cinema Cinema { get; set; } = null!;


    // Technology management methods
    public void AddTechnology(ScreeningTechnology technology)
    {
        SupportedTechnologies |= technology;
    }

    public void RemoveTechnology(ScreeningTechnology technology)
    {
        SupportedTechnologies &= ~technology;
    }

    public void SetTechnologies(ScreeningTechnology technologies)
    {
        SupportedTechnologies = technologies;
    }


    public bool SupportsTechnology(ScreeningTechnology technology)
    {
        return SupportedTechnologies.HasFlag(technology);
    }

    public bool SupportsAnyOf(params ScreeningTechnology[] technologies)
    {
        return technologies.Any(tech => SupportedTechnologies.HasFlag(tech));
    }

    public bool SupportsAllOf(params ScreeningTechnology[] technologies)
    {
        return technologies.All(tech => SupportedTechnologies.HasFlag(tech));
    }


    public bool CanShowMovie(ScreeningTechnology movieRequiredTechnology)
    {
        return SupportedTechnologies.HasFlag(movieRequiredTechnology);
    }


    public void UpdateName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new ArgumentException("Hall name cannot be empty");

        Name = newName;
    }

    public void AddSeat(Seat seat)
    {
        if (seat == null)
            throw new ArgumentNullException(nameof(seat));

        if (seats.Any(s => s.Row == seat.Row && s.Number == seat.Number))
            throw new InvalidOperationException($"Seat {seat.Row}{seat.Number} already exists");

        seats.Add(seat);
    }

    public void RemoveSeat(string row, int number)
    {
        var seat = seats.FirstOrDefault(s => s.Row == row && s.Number == number);
        if (seat == null)
            throw new InvalidOperationException($"Seat {row}{number} not found");

        seats.Remove(seat);
    }

    public void SetOperationalStatus(bool isOperational)
    {
        IsOperational = isOperational;
    }

    public IEnumerable<Seat> GetSeatsByType(SeatType seatType)
    {
        return seats.Where(s => s.Type == seatType);
    }

    public IEnumerable<Seat> GetSeatsByRow(string row)
    {
        return seats.Where(s => s.Row == row);
    }

    public Seat GetSeat(string row, int number)
    {
        return seats.FirstOrDefault(s => s.Row == row && s.Number == number)
               ?? throw new InvalidOperationException($"Seat {row}{number} not found");
    }

    public bool HasSeat(string row, int number)
    {
        return seats.Any(s => s.Row == row && s.Number == number);
    }

    public int GetCapacityByType(SeatType seatType)
    {
        return seats.Count(s => s.Type == seatType);
    }
}