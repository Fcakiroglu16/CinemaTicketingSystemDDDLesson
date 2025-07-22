namespace CinemaTicketingSystem.Domain;

public abstract class Entity<TKey>
{
    public TKey Id { get; set; } = default!;

    public override bool Equals(object? obj)
    {
        if (obj is not Entity<TKey> other)
            return false;

        if (ReferenceEquals(this, other))
            return true;

        if (GetType() != other.GetType())
            return false;

        if (Id is null || other.Id is null || Id.Equals(default(TKey)))
            return false;

        return Id.Equals(other.Id);
    }

    public override int GetHashCode()
    {
        return Id!.GetHashCode();
    }

    public static bool operator ==(Entity<TKey>? left, Entity<TKey>? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Entity<TKey>? left, Entity<TKey>? right)
    {
        return !(left == right);
    }
}

public abstract class Entity
{
}