namespace CinemaTicketingSystem.Domain;

public abstract class Entity<TKey>
{
    protected Entity()
    {
    }

    protected Entity(TKey id)
    {
        Id = id;
    }

    public TKey Id { get; protected set; } = default!;
}