using CinemaTicketingSystem.Application.Abstraction;
using CinemaTicketingSystem.Application.Abstraction.Accounts;
using CinemaTicketingSystem.Application.Abstraction.DependencyInjections;
using CinemaTicketingSystem.Domain.BoundedContexts.Accounts;
using CinemaTicketingSystem.SharedKernel;
using CinemaTicketingSystem.SharedKernel.Identities;

namespace CinemaTicketingSystem.Application.Account;

public class AccountAppService(
    AppDependencyService appDependencyService,
    IAccountRepository accountRepository,
    ITokenService tokenService,
    IRefreshTokenRepository refreshTokenRepository) : IScopedDependency, IAccountAppService
{
    public async Task<AppResult> SignUpAsync(SignUpRequest request)
    {
        var newUser = new User(request.Email, request.Password, request.FirstName, request.LastName);

        await accountRepository.CreateAsync(newUser);

        return AppResult.SuccessAsNoContent();
    }

    public async Task<AppResult<SignInResponse>> SignInAsync(SignInRequest request)
    {
        var user = await accountRepository.GetAsync(request.Email, request.Password);


        if (user is null)
            return appDependencyService.Error<SignInResponse>(ErrorCodes.InvalidCredentials,
                ErrorCodes.EmailOrPasswordWrong);

        var tokenResponse = tokenService.CreateToken(new CreateTokenRequest(user.Id, user.UserName, user.Email));


        var hasRefreshToken = await refreshTokenRepository.GetByIdAsync(user.Id);

        if (hasRefreshToken is not null)
        {
            hasRefreshToken.Token = tokenResponse.RefreshToken;
            hasRefreshToken.Expiration = tokenResponse.RefreshTokenExpiration;
            await refreshTokenRepository.UpdateAsync(hasRefreshToken);
        }
        else
        {
            hasRefreshToken =
                new RefreshToken(tokenResponse.RefreshToken, tokenResponse.RefreshTokenExpiration, user.Id);
            await refreshTokenRepository.AddAsync(hasRefreshToken);
        }




        await appDependencyService.UnitOfWork.SaveChangesAsync();


        return AppResult<SignInResponse>.SuccessAsOk(new SignInResponse(tokenResponse.AccessToken,
            tokenResponse.RefreshToken, tokenResponse.AccessTokenExpiration, tokenResponse.RefreshTokenExpiration));
    }
}