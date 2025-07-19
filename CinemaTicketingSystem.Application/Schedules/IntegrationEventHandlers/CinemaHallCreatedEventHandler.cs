using CinemaTicketingSystem.Application.Contracts;
using CinemaTicketingSystem.Domain.CinemaManagement.DomainEvents;
using CinemaTicketingSystem.Domain.Repositories;
using CinemaTicketingSystem.Domain.Scheduling;

namespace CinemaTicketingSystem.Application.Schedules.IntegrationEventHandlers
{
    public class CinemaHallCreatedEventHandler(IGenericRepository<Guid, CinemaHallSchedule> cinemaHallScheduleRepository, IUnitOfWork unitOfWork) : IEventHandler<CinemaHallCreatedEvent>
    {
        public async Task HandleAsync(CinemaHallCreatedEvent message, CancellationToken cancellationToken = default)
        {


            CinemaHallSchedule cinemaHallSchedule = new CinemaHallSchedule(
                message.HallId, message.SeatCount, message.hallTechnology);

            await cinemaHallScheduleRepository.AddAsync(cinemaHallSchedule, cancellationToken);

            await unitOfWork.SaveChangesAsync(cancellationToken);



        }
    }
}
