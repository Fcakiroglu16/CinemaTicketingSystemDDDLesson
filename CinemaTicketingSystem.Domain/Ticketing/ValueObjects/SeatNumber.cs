namespace CinemaTicketingSystem.Domain.Ticketing.ValueObjects;

public class SeatNumber : ValueObject
{
    public SeatNumber(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Seat number cannot be empty.");

        Value = value.ToUpperInvariant();
    }

    public string Value { get; }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString()
    {
        return Value;
    }
}