#region

using Ardalis.GuardClauses;
using CinemaTicketingSystem.SharedKernel.ValueObjects;

#endregion

namespace CinemaTicketingSystem.Domain.BoundedContexts.Catalog.ValueObjects;

public class ContentManagerId : ValueObject
{
    public ContentManagerId(Guid value)
    {
        Value = Guard.Against.Default(value, nameof(value), "ContentManagerId cannot be empty.");
    }

    protected ContentManagerId()
    {
    }

    public Guid Value { get; }

    public static ContentManagerId New()
    {
        return new ContentManagerId(Guid.CreateVersion7());
    }

    public static ContentManagerId From(Guid value)
    {
        return new ContentManagerId(value);
    }

    public static ContentManagerId From(string value)
    {
        if (!Guid.TryParse(value, out Guid guid))
            throw new ArgumentException($"Invalid ContentManagerId format: {value}");

        return new ContentManagerId(guid);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString()
    {
        return Value.ToString();
    }

    public static implicit operator Guid(ContentManagerId userId)
    {
        return userId.Value;
    }

    public static implicit operator ContentManagerId(Guid value)
    {
        return new ContentManagerId(value);
    }
}