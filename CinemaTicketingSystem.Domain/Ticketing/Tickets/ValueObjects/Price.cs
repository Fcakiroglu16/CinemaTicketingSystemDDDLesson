namespace CinemaTicketingSystem.Domain.Ticketing.Tickets.ValueObjects;

public class Price : ValueObject
{
    public Price(decimal amount, string currency)
    {
        if (amount < 0)
            throw new ArgumentException("Amount cannot be negative.");

        if (string.IsNullOrWhiteSpace(currency))
            throw new ArgumentException("Currency is required.");

        Amount = amount;
        Currency = currency.ToUpperInvariant(); // Örn: "TRY", "USD"
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