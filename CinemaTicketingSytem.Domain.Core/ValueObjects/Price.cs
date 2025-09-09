#region

using Ardalis.GuardClauses;

#endregion

namespace CinemaTicketingSystem.SharedKernel.ValueObjects;

public class Price : ValueObject
{
    public Price(decimal amount, string currency)
    {
        Amount = Guard.Against.Negative(amount, nameof(amount), "Amount cannot be negative.");
        Currency = Guard.Against.NullOrWhiteSpace(currency, nameof(currency), "Currency is required.")
            .ToUpperInvariant(); // Örn: "TRY", "USD"
    }

    protected Price()
    {
    }

    public decimal Amount { get; }
    public string Currency { get; }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Amount;
        yield return Currency;
    }

    public override string ToString()
    {
        return $"{Amount} {Currency}";
    }

    public static Price operator +(Price a, Price b)
    {
        if (a.Currency != b.Currency)
            throw new InvalidOperationException("Cannot add prices with different currencies.");

        return new Price(a.Amount + b.Amount, a.Currency);
    }

    public static Price operator *(Price price, int multiplier)
    {
        return new Price(price.Amount * multiplier, price.Currency);
    }
}