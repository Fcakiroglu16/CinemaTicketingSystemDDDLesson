using CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Holds;

namespace CinemaTicketingSystem.Persistence.Ticketing;

public class SeatHoldRepository(AppDbContext context) : GenericRepository<Guid, SeatHold>(context), ISeatHoldRepository;