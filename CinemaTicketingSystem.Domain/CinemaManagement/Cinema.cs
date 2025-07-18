namespace CinemaTicketingSystem.Domain.CinemaManagement;

public class Cinema : AuditedAggregateRoot<Guid>
{
    private readonly List<CinemaHall> cinemaHalls = [];


    public Cinema()
    {
    }


    // Constructor
    public Cinema(string name, Address address)
    {
        Id = Guid.NewGuid();
        Name = name;
        Address = address;
    }

    public string Name { get; private set; }
    public Address Address { get; private set; }
    public virtual IReadOnlyCollection<CinemaHall> Halls => cinemaHalls.AsReadOnly();

    // Business behavior methods
    public void UpdateName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new ArgumentException("Cinema name cannot be empty");

        Name = newName;
    }

    public void UpdateAddress(Address newAddress)
    {
        Address = newAddress ?? throw new ArgumentNullException(nameof(newAddress));
    }

    public void AddHall(CinemaHall hall)
    {
        if (hall == null)
            throw new ArgumentNullException(nameof(hall));

        if (cinemaHalls.Any(h => h.Name == hall.Name))
            throw new InvalidOperationException($"Hall with name '{hall.Name}' already exists");

        cinemaHalls.Add(hall);
    }

    public void RemoveHall(Guid hallId)
    {
        var hall = cinemaHalls.FirstOrDefault(h => h.Id == hallId);
        if (hall == null)
            throw new InvalidOperationException($"Hall with ID '{hallId}' not found");

        cinemaHalls.Remove(hall);
    }

    public CinemaHall? GetHall(Guid hallId)
    {
        return cinemaHalls.FirstOrDefault(h => h.Id == hallId);
    }

    public int GetTotalCapacity()
    {
        return cinemaHalls.Sum(h => h.Capacity);
    }

    public IEnumerable<CinemaHall> GetAvailableHalls()
    {
        return cinemaHalls.Where(h => h.IsOperational);
    }
}