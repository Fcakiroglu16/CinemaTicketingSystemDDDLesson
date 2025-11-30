#region

#endregion

namespace CinemaTicketingSystem.Application.Contracts.Catalog.Cinema;

public record CreateCinemaRequest(string Name, AddressDto Address);