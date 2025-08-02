using CinemaTicketingSystem.Domain.BoundedContexts.Accounts.ValueObjects;

namespace CinemaTicketingSystem.Domain.BoundedContexts.Accounts
{
    public interface IAccountRepository
    {

        Task CreateAsync(User user);
        Task<User?> GetAsync(UserId id);
        Task<User?> GetAsync(UserName UserName, Password Password);

    }
}
