namespace CinemaTicketingSystem.Domain.ValueObjects;

public class SeatNumber : ValueObject
{
    public SeatNumber(string row, int number)
    {
        if (string.IsNullOrWhiteSpace(row))
            throw new ArgumentException("Row cannot be empty.", nameof(row));
        if (number <= 0)
            throw new ArgumentException("Seat number must be positive.", nameof(number));

        Row = row.ToUpper();
        Number = number;
    }

    public string Row { get; }
    public int Number { get; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Row;
        yield return Number;
    }

    public override string ToString()
    {
        return $"{Row}{Number}";
    }
}