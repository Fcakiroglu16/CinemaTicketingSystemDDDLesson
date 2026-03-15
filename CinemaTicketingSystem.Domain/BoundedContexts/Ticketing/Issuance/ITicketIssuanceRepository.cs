#region

using CinemaTicketingSystem.Domain.Repositories;

#endregion

namespace CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Issuance;

public interface ITicketIssuanceRepository : IGenericRepository<TicketIssuance>;