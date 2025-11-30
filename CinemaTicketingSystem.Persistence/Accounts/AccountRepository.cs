#region

using CinemaTicketingSystem.Domain.BoundedContexts.Accounts;
using CinemaTicketingSystem.Domain.BoundedContexts.Accounts.ValueObjects;
using Microsoft.AspNetCore.Identity;

#endregion

namespace CinemaTicketingSystem.Infrastructure.Persistence.Accounts;

internal class AccountRepository(UserManager<AppUser> userManager) : IAccountRepository
{
    public async Task<bool> ExistEmailAsync(Email email)
    {
        AppUser? userFromDb = await userManager.FindByEmailAsync(email);
        return userFromDb is not null;
    }

    public async Task CreateAsync(User user)
    {
        AppUser newUser = new AppUser
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

    public async Task<User?> GetAsync(Email email, Password password)
    {
        AppUser? userFromDb = await userManager.FindByEmailAsync(email);
        if (userFromDb is null) return null;
        bool passwordCheck = await userManager.CheckPasswordAsync(userFromDb, password);
        if (!passwordCheck) return null;


        return new User(userFromDb.Id, userFromDb.UserName!, userFromDb.Email!,
            userFromDb.FirstName!, userFromDb.LastName!, userFromDb.CreatedAt);
    }

    public async Task<User?> GetAsync(UserId id)
    {
        AppUser? userFromDb = await userManager.FindByIdAsync(id.ToString());

        if (userFromDb is null) return null;

        return new User(userFromDb.Id, UserName.From(userFromDb.UserName!), Email.From(userFromDb.Email!),
            userFromDb.FirstName!, userFromDb.LastName!, userFromDb.CreatedAt);
    }
}