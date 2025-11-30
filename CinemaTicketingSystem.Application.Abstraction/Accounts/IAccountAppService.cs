#region

#endregion

namespace CinemaTicketingSystem.Application.Contracts.Accounts;

public interface IAccountAppService
{
    Task<AppResult<SignInResponse>> SignInAsync(SignInRequest userId);
    Task<AppResult> SignUpAsync(SignUpRequest request);
}