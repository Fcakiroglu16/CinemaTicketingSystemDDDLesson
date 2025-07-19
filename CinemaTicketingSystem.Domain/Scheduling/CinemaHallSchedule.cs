using CinemaTicketingSystem.Domain.Core;

namespace CinemaTicketingSystem.Domain.Scheduling
{
    public class CinemaHallSchedule : AggregateRoot<Guid>
    {

        protected CinemaHallSchedule() { }

        public Guid CinemaHallId { get; set; }

        public ScreeningTechnology SupportedTechnologies { get; private set; } = ScreeningTechnology.Standard;


        public short SeatCount { get; set; }


        private List<MovieSchedule> _movieSchedules = [];

        public virtual IReadOnlyCollection<MovieSchedule> MovieSchedules => _movieSchedules.AsReadOnly();



        public CinemaHallSchedule(Guid cinemaHallId, short seatCount, ScreeningTechnology supportedTechnologies)
        {
            if (cinemaHallId == Guid.Empty) throw new ArgumentException("Cinema hall ID cannot be empty", nameof(cinemaHallId));
            if (seatCount <= 0) throw new ArgumentOutOfRangeException(nameof(seatCount), "Seat count must be positive");

            Id = Guid.CreateVersion7();
            CinemaHallId = cinemaHallId;
            SeatCount = seatCount;
            SupportedTechnologies = supportedTechnologies;
        }
        public void AddMovieSchedule(MovieSchedule movieSchedule)
        {
            if (movieSchedule == null) throw new ArgumentNullException(nameof(movieSchedule));

            if (SupportedTechnologies != movieSchedule.SupportedTechnology)
                throw new InvalidOperationException(
                    $"Cinema hall does not support the technology required by movie {movieSchedule.MovieId}");

            _movieSchedules.Add(movieSchedule);
        }
    }
}
