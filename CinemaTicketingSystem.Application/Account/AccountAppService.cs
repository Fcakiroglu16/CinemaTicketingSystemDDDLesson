using CinemaTicketingSystem.Application.Abstraction;
using CinemaTicketingSystem.Application.Abstraction.Accounts;
using CinemaTicketingSystem.Application.Abstraction.DependencyInjections;
using CinemaTicketingSystem.Domain.BoundedContexts.Accounts;

namespace CinemaTicketingSystem.Application.Account;

public class AccountAppService(AppDependencyService appDependencyService, IAccountRepository accountRepository, ITokenService tokenService) : IScopedDependency, IAccountAppService
{

    public async Task<AppResult> SignUpAsync(SignUpRequest request)
    {

        var newUser = new User(request.Email, request.Password, request.FirstName, request.LastName);

        await accountRepository.CreateAsync(newUser);

        return AppResult.SuccessAsNoContent();



    }

    public async Task<AppResult?> SignInAsync(SignInRequest userId)
    {
        var user = await accountRepository.GetAsync(userId.Email, userId.Password);









        return null;





    }
}