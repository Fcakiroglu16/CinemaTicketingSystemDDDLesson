using CinemaTicketingSystem.Domain.BoundedContexts.Accounts;
using CinemaTicketingSystem.Domain.BoundedContexts.Accounts.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace CinemaTicketingSystem.Persistence.Accounts
{
    internal class AccountRepository(UserManager<AppUser> userManager) : IAccountRepository
    {
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

            var result = await userManager.CreateAsync(newUser, user.Password);


            if (!result.Succeeded)
            {
                throw new InvalidOperationException(string.Join(", ", result.Errors.Select(e => e.Description)));
            }



        }

        public async Task<User?> GetAsync(UserId id)
        {

            var userFromDb = await userManager.FindByIdAsync(id.ToString());

            if (userFromDb is null) return null;

            return new User(userFromDb.Id, UserName.From(userFromDb.UserName!), Email.From(userFromDb.Email!),
                userFromDb.FirstName!, userFromDb.LastName!, userFromDb.CreatedAt);


        }

        public async Task<User?> GetAsync(UserName UserName, Password Password)
        {
            var userFromDb = await userManager.FindByNameAsync(UserName);
            if (userFromDb is null) return null;
            var passwordCheck = await userManager.CheckPasswordAsync(userFromDb, Password);
            if (!passwordCheck) return null;
            return new User(userFromDb.Id, userFromDb.UserName!, userFromDb.Email!,
                userFromDb.FirstName!, userFromDb.LastName!, userFromDb.CreatedAt);
        }
    }
}
