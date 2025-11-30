namespace CinemaTicketingSystem.SharedKernel.Entities;

public abstract class Entity<TKey> : EntityBase
    where TKey : notnull
{
    public TKey Id { get; set; } = default!;

    protected override object?[] GetKeys()
    {
        return [Id];
    }
}

public abstract class Entity : EntityBase
{
    protected abstract override object?[] GetKeys();
}

public abstract class EntityBase
{
    public override bool Equals(object? obj)
    {
        if (obj is null || GetType() != obj.GetType())
            return false;

        return GetKeys().SequenceEqual(((EntityBase)obj).GetKeys());
    }

    public override int GetHashCode()
    {
        return GetKeys().Aggregate(17, (current, key) => (current * 31) + (key?.GetHashCode() ?? 0));
    }

    public static bool operator ==(EntityBase? left, EntityBase? right)
    {
        if (ReferenceEquals(left, right)) return true;
        if (left is null || right is null) return false;
        return left.Equals(right);
    }

    public static bool operator !=(EntityBase? left, EntityBase? right)
    {
        return !(left == right);
    }

    protected abstract object?[] GetKeys();
}