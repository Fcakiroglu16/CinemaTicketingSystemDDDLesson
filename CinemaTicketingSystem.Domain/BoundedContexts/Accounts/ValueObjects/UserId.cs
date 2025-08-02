namespace CinemaTicketingSystem.Domain.BoundedContexts.Accounts.ValueObjects;

public class UserId : ValueObject
{
    public UserId(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("UserId cannot be empty.");

        Value = value;
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
        if (!Guid.TryParse(value, out var guid))
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