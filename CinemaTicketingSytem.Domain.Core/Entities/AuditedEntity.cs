namespace CinemaTicketingSystem.SharedKernel.Entities;

public class AuditedEntity<TKey> : Entity<TKey>
    where TKey : notnull
{
    public DateTime CreationTime { get; set; }

    public Guid CreatorId { get; set; }

    public DateTime? LastModificationTime { get; set; }

    public Guid? LastModifierId { get; set; }


    public virtual bool IsDeleted { get; set; }


    public virtual Guid? DeleterId { get; set; }


    public virtual DateTime? DeletionTime { get; set; }
}