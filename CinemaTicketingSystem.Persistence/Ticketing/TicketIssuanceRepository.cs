#region

using CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Issuance;

#endregion

namespace CinemaTicketingSystem.Infrastructure.Persistence.Ticketing;

internal class TicketIssuanceRepository(AppDbContext context)
    : GenericRepository<TicketIssuance>(context), ITicketIssuanceRepository;