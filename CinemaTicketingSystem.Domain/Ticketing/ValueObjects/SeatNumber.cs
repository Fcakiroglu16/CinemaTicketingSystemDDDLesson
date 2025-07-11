using CinemaTicketingSystem.Domain;

public class SeatNumber : ValueObject
{
    public string Value { get; }

    public SeatNumber(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Seat number cannot be empty.");

        Value = value.ToUpperInvariant();
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}