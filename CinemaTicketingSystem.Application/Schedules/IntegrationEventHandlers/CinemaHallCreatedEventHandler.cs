using CinemaTicketingSystem.Application.Abstraction.Contracts;
using CinemaTicketingSystem.Domain.BoundedContexts.Catalog.DomainEvents;
using CinemaTicketingSystem.Domain.BoundedContexts.Scheduling;
using CinemaTicketingSystem.Domain.Repositories;

namespace CinemaTicketingSystem.Application.Schedules.IntegrationEventHandlers;

public class CinemaHallCreatedEventHandler(
    IGenericRepository<Guid, CinemaHallSnapshot> cinemaHallScheduleRepository,
    IUnitOfWork unitOfWork) : IEventHandler<CinemaHallCreatedEvent>
{
    public async Task HandleAsync(CinemaHallCreatedEvent message, CancellationToken cancellationToken = default)
    {
        var cinemaHallSchedule = new CinemaHallSnapshot(
            message.HallId, message.SeatCount, message.hallTechnology);

        await cinemaHallScheduleRepository.AddAsync(cinemaHallSchedule, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}