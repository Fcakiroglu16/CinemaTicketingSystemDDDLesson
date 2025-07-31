namespace CinemaTicketingSystem.SharedKernel;

public interface IUserContext
{
    Guid UserId { get; }
    string UserName { get; }
    string Email { get; }
}