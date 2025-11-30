#region

using Ardalis.GuardClauses;
using CinemaTicketingSystem.SharedKernel.ValueObjects;

#endregion

namespace CinemaTicketingSystem.Domain.BoundedContexts.Accounts.ValueObjects;

public class UserId : ValueObject
{
    public UserId(Guid value)
    {
        Value = Guard.Against.Default(value, nameof(value), "UserId cannot be empty.");
    }

    protected UserId()
    {
    }


    public Guid Value { get; }

    public static UserId New()
    {
        return new UserId(Guid.CreateVersion7());
    }

    public static UserId From(Guid value)
    {
        return new UserId(value);
    }

    public static UserId From(string value)
    {
        if (!Guid.TryParse(value, out Guid guid))
            throw new ArgumentException($"Invalid UserId format: {value}");

        return new UserId(guid);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString()
    {
        return Value.ToString();
    }

    public static implicit operator Guid(UserId userId)
    {
        return userId.Value;
    }

    public static implicit operator UserId(Guid value)
    {
        return new UserId(value);
    }
}