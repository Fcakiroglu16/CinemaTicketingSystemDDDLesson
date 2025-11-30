#region

using CinemaTicketingSystem.Application.Contracts.Contracts;
using CinemaTicketingSystem.Domain.BoundedContexts.Catalog.IntegrationEvents;
using CinemaTicketingSystem.Domain.BoundedContexts.Scheduling;
using CinemaTicketingSystem.Domain.Repositories;

#endregion

namespace CinemaTicketingSystem.Application.Schedules.IntegrationEventHandlers;

public class CinemaHallCreatedIntegrationEventHandler(
    IGenericRepository<CinemaHallSnapshot> cinemaHallScheduleRepository,
    IUnitOfWork unitOfWork) : IIntegrationEventHandler<CinemaHallCreatedIntegrationEvent>
{
    public async Task HandleAsync(CinemaHallCreatedIntegrationEvent message,
        CancellationToken cancellationToken = default)
    {
        CinemaHallSnapshot cinemaHallSchedule = new CinemaHallSnapshot(
            message.HallId, message.SeatCount, message.hallTechnology);

        await cinemaHallScheduleRepository.AddAsync(cinemaHallSchedule, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}