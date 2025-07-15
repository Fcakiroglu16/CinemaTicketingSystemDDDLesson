namespace CinemaTicketingSystem.Domain.CinemaManagement;

public class CinemaHall : Entity<Guid>
{


    public string? Name { get; private set; }
    public HallTechnology SupportedTechnologies { get; private set; } = HallTechnology.Standard;

    private readonly List<Seat> seats = [];
    public virtual IReadOnlyList<Seat> Seats => seats.AsReadOnly();
    public bool IsOperational { get; private set; } = true;


    public Guid MovieId { get; set; }

    public int Capacity => Seats.Count;


    public virtual Cinema Cinema { get; set; } = null!;
    private CinemaHall() { }

    // Constructor
    public CinemaHall(string name, HallTechnology supportedTechnologies = HallTechnology.Standard)
    {
        Name = name;
        SupportedTechnologies = supportedTechnologies;
    }

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

    // Technology query methods
    public bool SupportsIMAX()
    {
        return SupportedTechnologies.HasFlag(HallTechnology.IMAX);
    }

    public bool Supports3D()
    {
        return SupportedTechnologies.HasFlag(HallTechnology.ThreeD);
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

    public IEnumerable<HallTechnology> GetSupportedTechnologies()
    {
        return Enum.GetValues<HallTechnology>()
            .Where(tech => tech != HallTechnology.None && SupportedTechnologies.HasFlag(tech));
    }

    public string GetTechnologyDescription()
    {
        var techs = GetSupportedTechnologies().ToList();
        return techs.Any() ? string.Join(", ", techs) : "Standard";
    }

    // Compatibility check methods
    public bool IsCompatibleWith(HallTechnology requiredTechnology)
    {
        return SupportedTechnologies.HasFlag(requiredTechnology);
    }

    public bool CanShowMovie(HallTechnology movieRequiredTechnology)
    {

        if (movieRequiredTechnology == HallTechnology.None || movieRequiredTechnology == HallTechnology.Standard)
            return true;

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