#region

using CinemaTicketingSystem.Application.Contracts.Contracts;
using CinemaTicketingSystem.Domain.BoundedContexts.Catalog.IntegrationEvents;
using CinemaTicketingSystem.Domain.BoundedContexts.Scheduling;
using CinemaTicketingSystem.Domain.Repositories;

#endregion

namespace CinemaTicketingSystem.Application.Schedules.IntegrationEventHandlers;

public class MovieCreatedIntegrationEventHandler(
    IGenericRepository<MovieSnapshot> movieScheduleRepository,
    IUnitOfWork unitOfWork) : IIntegrationEventHandler<MovieCreatedIntegrationEvent>
{
    public async Task HandleAsync(MovieCreatedIntegrationEvent message, CancellationToken cancellationToken = default)
    {
        MovieSnapshot movieSnapshot = new MovieSnapshot(message.MovieId, message.Duration, message.Technology);
        await movieScheduleRepository.AddAsync(movieSnapshot, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}