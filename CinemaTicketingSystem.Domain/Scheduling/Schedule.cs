namespace CinemaTicketingSystem.Domain.Scheduling
{
    public class Schedule : AggregateRoot<Guid>
    {
        protected Schedule()
        {
        }

        public Guid MovieId { get; set; }
        public Guid HallId { get; set; }

        public virtual ShowTime ShowTime { get; set; }

    }
}
