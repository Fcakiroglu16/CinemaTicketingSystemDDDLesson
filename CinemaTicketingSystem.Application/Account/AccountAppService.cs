#region

using CinemaTicketingSystem.Application.Contracts;
using CinemaTicketingSystem.Application.Contracts.Accounts;
using CinemaTicketingSystem.Application.Contracts.DependencyInjections;
using CinemaTicketingSystem.Domain.BoundedContexts.Accounts;
using CinemaTicketingSystem.Domain.BoundedContexts.Accounts.ValueObjects;
using CinemaTicketingSystem.SharedKernel;
using CinemaTicketingSystem.SharedKernel.Identities;

#endregion

namespace CinemaTicketingSystem.Application.Account;

public class AccountAppService(
    AppDependencyService appDependencyService,
    IAccountRepository accountRepository,
    ITokenService tokenService,
    IRefreshTokenRepository refreshTokenRepository) : IScopedDependency, IAccountAppService
{
    //public async Task<AppResult> CheckEmailAsync(Email email)
    //{
    //    var user = await accountRepository.GetByEmailAsync(email);
    //    if (user is not null)
    //        return appDependencyService.LocalizeError.Error(ErrorCodes.EmailAlreadyExists, ErrorCodes.EmailAlreadyExists);
    //    return AppResult.SuccessAsNoContent();
    //}


    public async Task<AppResult> SignUpAsync(SignUpRequest request)
    {
        bool emailExists = await accountRepository.ExistEmailAsync(request.Email);

        if (emailExists)
            return appDependencyService.LocalizeError.Error(ErrorCodes.UserAlreadyExists);

        User newUser = new User(request.Email, request.Password, request.FirstName, request.LastName);

        await accountRepository.CreateAsync(newUser);

        return AppResult.SuccessAsNoContent();
    }

    public async Task<AppResult<SignInResponse>> SignInAsync(SignInRequest request)
    {
        User? user = await accountRepository.GetAsync(request.Email, request.Password);


        if (user is null)
            return appDependencyService.LocalizeError.Error<SignInResponse>(ErrorCodes.InvalidCredentials,
                ErrorCodes.EmailOrPasswordWrong);

        CreateTokenResponse tokenResponse = tokenService.CreateToken(new CreateTokenRequest(user.Id, user.UserName, user.Email));


        RefreshToken? hasRefreshToken = await refreshTokenRepository.GetByIdAsync(UserId.From(user.Id));

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

    Task<AppResult<SignInResponse>> IAccountAppService.SignInAsync(SignInRequest userId)
    {
        throw new NotImplementedException();
    }

    Task<AppResult> IAccountAppService.SignUpAsync(SignUpRequest request)
    {
        throw new NotImplementedException();
    }
}