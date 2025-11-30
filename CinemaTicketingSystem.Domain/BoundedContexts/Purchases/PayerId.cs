#region

using Ardalis.GuardClauses;
using CinemaTicketingSystem.SharedKernel.ValueObjects;

#endregion

namespace CinemaTicketingSystem.Domain.BoundedContexts.Purchases;

public class PayerId : ValueObject
{
    public PayerId(Guid value)
    {
        Value = Guard.Against.Default(value, nameof(value), "ContentManagerId cannot be empty.");
    }

    protected PayerId()
    {
    }

    public Guid Value { get; }

    public static PayerId New()
    {
        return new PayerId(Guid.CreateVersion7());
    }

    public static PayerId From(Guid value)
    {
        return new PayerId(value);
    }

    public static PayerId From(string value)
    {
        if (!Guid.TryParse(value, out Guid guid))
            throw new ArgumentException($"Invalid ContentManagerId format: {value}");

        return new PayerId(guid);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString()
    {
        return Value.ToString();
    }

    public static implicit operator Guid(PayerId payerId)
    {
        return payerId.Value;
    }

    public static implicit operator PayerId(Guid value)
    {
        return new PayerId(value);
    }
}