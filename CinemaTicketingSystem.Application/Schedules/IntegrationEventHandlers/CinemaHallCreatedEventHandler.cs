using CinemaTicketingSystem.Application.Contracts;
using CinemaTicketingSystem.Domain.Catalog.DomainEvents;
using CinemaTicketingSystem.Domain.Repositories;
using CinemaTicketingSystem.Domain.Scheduling;

namespace CinemaTicketingSystem.Application.Schedules.IntegrationEventHandlers
{
    public class CinemaHallCreatedEventHandler(IGenericRepository<Guid, CinemaHallSnapshot> cinemaHallScheduleRepository, IUnitOfWork unitOfWork) : IEventHandler<CinemaHallCreatedEvent>
    {
        public async Task HandleAsync(CinemaHallCreatedEvent message, CancellationToken cancellationToken = default)
        {


            CinemaHallSnapshot cinemaHallSchedule = new CinemaHallSnapshot(
                message.HallId, message.SeatCount, message.hallTechnology);

            await cinemaHallScheduleRepository.AddAsync(cinemaHallSchedule, cancellationToken);

            await unitOfWork.SaveChangesAsync(cancellationToken);



        }
    }
}
