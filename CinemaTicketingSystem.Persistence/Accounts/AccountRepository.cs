using CinemaTicketingSystem.Domain.BoundedContexts.Accounts;
using CinemaTicketingSystem.Domain.BoundedContexts.Accounts.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace CinemaTicketingSystem.Persistence.Accounts;

internal class AccountRepository(UserManager<AppUser> userManager) : IAccountRepository
{


    public async Task<bool> ExistEmailAsync(Email email)
    {

        var userFromDb = await userManager.FindByEmailAsync(email);
        return userFromDb is not null;
    }
    public async Task CreateAsync(User user)
    {
        var newUser = new AppUser
        {
            Email = user.Email,
            UserName = user.UserName,
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            CreatedAt = user.CreatedAt
        };

        await userManager.CreateAsync(newUser, user.Password);

    }

    public async Task<User?> GetAsync(UserId id)
    {
        var userFromDb = await userManager.FindByIdAsync(id.ToString());

        if (userFromDb is null) return null;

        return new User(userFromDb.Id, UserName.From(userFromDb.UserName!), Email.From(userFromDb.Email!),
            userFromDb.FirstName!, userFromDb.LastName!, userFromDb.CreatedAt);
    }

    public async Task<User?> GetAsync(Email email, Password password)
    {
        var userFromDb = await userManager.FindByEmailAsync(email);
        if (userFromDb is null) return null;
        var passwordCheck = await userManager.CheckPasswordAsync(userFromDb, password);
        if (!passwordCheck) return null;


        return new User(userFromDb.Id, userFromDb.UserName!, userFromDb.Email!,
            userFromDb.FirstName!, userFromDb.LastName!, userFromDb.CreatedAt);
    }

}