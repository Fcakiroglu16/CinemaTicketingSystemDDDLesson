using CinemaTicketingSystem.Application.Abstraction.Contracts;
using CinemaTicketingSystem.Domain.Catalog.DomainEvents;
using CinemaTicketingSystem.Domain.Repositories;
using CinemaTicketingSystem.Domain.Scheduling;

namespace CinemaTicketingSystem.Application.Schedules.IntegrationEventHandlers;

public class MovieCreatedEventHandler(
    IGenericRepository<Guid, MovieSnapshot> movieScheduleRepository,
    IUnitOfWork unitOfWork) : IEventHandler<MovieCreatedEvent>
{
    public async Task HandleAsync(MovieCreatedEvent message, CancellationToken cancellationToken = default)
    {
        var movieSnapshot = new MovieSnapshot(message.MovieId, message.Duration, message.Technology);
        await movieScheduleRepository.AddAsync(movieSnapshot, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}