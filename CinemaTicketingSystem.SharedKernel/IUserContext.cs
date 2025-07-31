namespace CinemaTicketingSystem.SharedKernel;

public interface IUserContext
{
    public Guid UserId { get; }
    public string UserName { get; }
    public string Email { get; }
    public string? FirstName { get; }
    public string? LastName { get; }
    public string? PhoneNumber { get; }
}