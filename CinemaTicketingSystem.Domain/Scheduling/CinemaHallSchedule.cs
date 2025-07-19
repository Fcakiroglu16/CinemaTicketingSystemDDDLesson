using CinemaTicketingSystem.Domain.Core;

namespace CinemaTicketingSystem.Domain.Scheduling
{
    public class CinemaHallSchedule : AggregateRoot<Guid>
    {

        protected CinemaHallSchedule() { }

        public Guid CinemaHallId { get; set; }

        public HallTechnology SupportedTechnologies { get; private set; } = HallTechnology.Standard;




        private List<MovieSchedule> _movieSchedules = [];

        public virtual IReadOnlyCollection<MovieSchedule> MovieSchedules => _movieSchedules.AsReadOnly();
    }
}
