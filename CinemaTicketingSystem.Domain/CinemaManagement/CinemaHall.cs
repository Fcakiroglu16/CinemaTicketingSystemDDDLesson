using CinemaTicketingSystem.Domain.Core;

namespace CinemaTicketingSystem.Domain.CinemaManagement;




public class CinemaHall : Entity<Guid>
{
    protected CinemaHall()
    {
    }

    public string Name { get; private set; }
    public HallTechnology SupportedTechnologies { get; private set; } = HallTechnology.Standard;
    private readonly List<Seat> seats = [];
    public virtual IReadOnlyList<Seat> Seats => seats.AsReadOnly();
    public bool IsOperational { get; private set; } = true;



    public int Capacity => Seats.Count;


    // Constructor
    public CinemaHall(string name, HallTechnology supportedTechnologies = HallTechnology.Standard)
    {
        Id = Guid.CreateVersion7();
        Name = name;
        SupportedTechnologies = supportedTechnologies;
    }





    public virtual Cinema Cinema { get; set; } = null!;




    // Technology management methods
    public void AddTechnology(HallTechnology technology)
    {
        SupportedTechnologies |= technology;
    }

    public void RemoveTechnology(HallTechnology technology)
    {
        SupportedTechnologies &= ~technology;
    }

    public void SetTechnologies(HallTechnology technologies)
    {
        SupportedTechnologies = technologies;
    }



    public bool SupportsTechnology(HallTechnology technology)
    {
        return SupportedTechnologies.HasFlag(technology);
    }

    public bool SupportsAnyOf(params HallTechnology[] technologies)
    {
        return technologies.Any(tech => SupportedTechnologies.HasFlag(tech));
    }

    public bool SupportsAllOf(params HallTechnology[] technologies)
    {
        return technologies.All(tech => SupportedTechnologies.HasFlag(tech));
    }




    public bool CanShowMovie(HallTechnology movieRequiredTechnology)
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