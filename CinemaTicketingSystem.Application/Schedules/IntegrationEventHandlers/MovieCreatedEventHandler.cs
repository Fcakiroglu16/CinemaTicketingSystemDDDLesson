using CinemaTicketingSystem.Application.Abstraction.Contracts;
using CinemaTicketingSystem.Domain.BoundedContexts.Catalog.DomainEvents;
using CinemaTicketingSystem.Domain.BoundedContexts.Scheduling;
using CinemaTicketingSystem.Domain.Repositories;

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