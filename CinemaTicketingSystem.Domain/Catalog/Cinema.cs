using Ardalis.GuardClauses;
using CinemaTicketingSystem.Domain.Catalog.DomainEvents;
using CinemaTicketingSystem.Domain.Core.Exceptions;

namespace CinemaTicketingSystem.Domain.Catalog;

public class Cinema : AuditedAggregateRoot<Guid>
{
    private readonly List<CinemaHall> cinemaHalls = [];


    protected Cinema()
    {
    }


    // Constructor
    public Cinema(string name, Address address)
    {
        Id = Guid.CreateVersion7();
        Name = Guard.Against.NullOrWhiteSpace(name, nameof(name));
        Address = Guard.Against.Null(address, nameof(address));
    }

    public string Name { get; private set; }
    public Address Address { get; private set; }
    public virtual IReadOnlyCollection<CinemaHall> Halls => cinemaHalls.AsReadOnly();

    // Business behavior methods
    public void UpdateName(string newName)
    {
        Name = Guard.Against.NullOrWhiteSpace(newName, nameof(newName));
    }

    public void UpdateAddress(Address newAddress)
    {
        Address = Guard.Against.Null(newAddress, nameof(newAddress));
    }

    public void AddHall(CinemaHall hall)
    {
        Guard.Against.Null(hall, nameof(hall));

        if (cinemaHalls.Any(h => h.Name == hall.Name)) throw new CinemaHallAlreadyExistsException(hall.Name);


        cinemaHalls.Add(hall);

        AddDomainEvent(new CinemaHallCreatedEvent(hall.Id, hall.SupportedTechnologies, hall.Capacity));
    }

    public void RemoveHall(Guid hallId)
    {
        var hall = cinemaHalls.FirstOrDefault(h => h.Id == hallId);

        if (hall is null)
            throw new CinemaHallNotFoundException(hallId);

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