using Ardalis.GuardClauses;
using CinemaTicketingSystem.Domain.Core;
using CinemaTicketingSystem.Domain.Core.Exceptions;
using CinemaTicketingSystem.Domain.ValueObjects;
using CinemaTicketingSystem.SharedKernel.Entities;

namespace CinemaTicketingSystem.Domain.BoundedContexts.Catalog;

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
        Name = Guard.Against.NullOrWhiteSpace(name, nameof(name), "Hall name cannot be empty");
        SupportedTechnologies = supportedTechnologies;
    }

    public string Name { get; private set; }
    public ScreeningTechnology SupportedTechnologies { get; private set; }
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
        Name = Guard.Against.NullOrWhiteSpace(newName, nameof(newName), "Hall name cannot be empty");
    }

    public void AddSeat(Seat seat)
    {
        Guard.Against.Null(seat, nameof(seat));

        if (seats.Any(s => s.SeatPosition.Equals(seat.SeatPosition)))
            throw new SeatAlreadyExistsException(seat.SeatPosition.Row, seat.SeatPosition.Number);

        seats.Add(seat);
    }

    public void RemoveSeat(string row, int number)
    {
        var seat = seats.FirstOrDefault(s => s.SeatPosition.Equals(new SeatPosition(row, number)));
        Guard.Against.Null(seat, nameof(seat), $"Seat {row}{number} not found");

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
        return seats.Where(s => s.SeatPosition.Row == row);
    }

    public Seat GetSeat(string row, int number)
    {
        var seat = seats.FirstOrDefault(s => s.SeatPosition.Equals(new SeatPosition(row, number)));
        return Guard.Against.Null(seat, nameof(seat), $"Seat {row}{number} not found");
    }

    public bool HasSeat(string row, int number)
    {
        return seats.Any(s => s.SeatPosition.Equals(new SeatPosition(row, number)));
    }

    public int GetCapacityByType(SeatType seatType)
    {
        return seats.Count(s => s.Type == seatType);
    }
}