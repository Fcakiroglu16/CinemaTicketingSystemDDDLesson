#region

using CinemaTicketingSystem.SharedKernel.Identities;

#endregion

namespace CinemaTicketingSystem.Application.Contracts.Accounts;

public interface ITokenService
{
    CreateTokenResponse CreateToken(CreateTokenRequest createToken);
}