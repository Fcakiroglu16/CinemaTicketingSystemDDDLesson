using CinemaTicketingSystem.Application.Abstraction;
using CinemaTicketingSystem.Application.Abstraction.DependencyInjections;
using CinemaTicketingSystem.Domain.BoundedContexts.Accounts;

namespace CinemaTicketingSystem.Application.Account;

public class AccountService(AppDependencyService appDependencyService, IAccountRepository accountRepository) : IScopedDependency
{

    public async Task<AppResult> SignUpAsync(SignUpRequest request)
    {

        var newUser = new User(request.Email, request.Password, request.FirstName, request.LastName);

        await accountRepository.CreateAsync(newUser);

        return AppResult.SuccessAsNoContent();



    }

    public async Task<AppResult<User>> SignInRequest(SignInRequest userId)
    {
        var user = await accountRepository.GetAsync(userId.Email, userId.Password);



    }
}

public record SignInRequest(string Email, string Password);

public record SignUpRequest(string Email, string Password, string FirstName, string LastName);