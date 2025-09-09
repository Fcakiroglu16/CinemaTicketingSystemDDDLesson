#region

using CinemaTicketingSystem.Application.Contracts.Ticketing;

#endregion

namespace CinemaTicketingSystem.Application.Contracts.Purchase;

public record CreatePurchaseRequest(
    string CardNumber,
    string CardHolderName,
    string CardExpirationDate,
    string CardSecurityNumber,
    PriceDto Price,
    Guid TicketIssuanceId);