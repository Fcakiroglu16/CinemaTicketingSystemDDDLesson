using CinemaTicketingSystem.Application.Contracts;
using CinemaTicketingSystem.Domain.CinemaManagement.DomainEvents;
using CinemaTicketingSystem.Domain.Repositories;
using CinemaTicketingSystem.Domain.Scheduling;

namespace CinemaTicketingSystem.Application.Schedules.IntegrationEventHandlers
{


    public class MovieCreatedEventHandler(IGenericRepository<Guid, MovieSchedule> movieScheduleRepository, IUnitOfWork unitOfWork) : IEventHandler<MovieCreatedEvent>
    {
        public async Task HandleAsync(MovieCreatedEvent message, CancellationToken cancellationToken = default)
        {

            MovieSchedule movieSchedule = new MovieSchedule(message.MovieId, message.Duration, message.Technology);
            await movieScheduleRepository.AddAsync(movieSchedule, cancellationToken);

            await unitOfWork.SaveChangesAsync(cancellationToken);



        }
    }
}
