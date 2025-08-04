using CinemaTicketingSystem.Domain.BoundedContexts.Accounts.ValueObjects;

namespace CinemaTicketingSystem.Domain.BoundedContexts.Accounts;

public interface IAccountRepository
{
    Task CreateAsync(User user);
    Task<User?> GetAsync(Email email, Password password);
    Task<bool> ExistEmailAsync(Email email);
}