namespace CinemaTicketingSystem.SharedKernel.Entities;

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
    public abstract object?[] GetKeys();

    public override bool Equals(object? obj)
    {
        if (obj == null || obj.GetType() != GetType())
            return false;

        var other = (Entity)obj;
        var thisKeys = GetKeys();
        var otherKeys = other.GetKeys();

        if (thisKeys.Length != otherKeys.Length)
            return false;

        for (int i = 0; i < thisKeys.Length; i++)
        {
            if (thisKeys[i] is null ^ otherKeys[i] is null)
                return false;

            if (thisKeys[i] != null && !thisKeys[i]!.Equals(otherKeys[i]))
                return false;
        }

        return true;
    }

    public override int GetHashCode()
    {
        var keys = GetKeys();
        int hash = 17;
        foreach (var key in keys)
        {
            hash = (hash * 31) + (key?.GetHashCode() ?? 0);
        }
        return hash;
    }

    public static bool operator ==(Entity? left, Entity? right)
    {
        if (ReferenceEquals(left, right))
            return true;
        if (left is null || right is null)
            return false;
        return left.Equals(right);
    }

    public static bool operator !=(Entity? left, Entity? right)
    {
        return !(left == right);
    }
}