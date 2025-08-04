using Ardalis.GuardClauses;

namespace CinemaTicketingSystem.SharedKernel.ValueObjects;

public class SeatPosition : ValueObject
{
    public SeatPosition(string row, int number)
    {
        Row = Guard.Against.NullOrWhiteSpace(row, nameof(row), "Row cannot be empty.")
            .ToUpper();
        Number = Guard.Against.NegativeOrZero(number, nameof(number), "Seat number must be positive.");
    }

    public string Row { get; private set; }
    public int Number { get; private set; }

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