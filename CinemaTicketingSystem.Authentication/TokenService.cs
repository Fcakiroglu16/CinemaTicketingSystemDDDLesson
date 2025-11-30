#region

using CinemaTicketingSystem.Application.Contracts.Accounts;
using CinemaTicketingSystem.Application.Contracts.DependencyInjections;
using CinemaTicketingSystem.SharedKernel.Identities;
using CinemaTicketingSystem.SharedKernel.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

#endregion

namespace CinemaTicketingSystem.Infrastructure.Authentication;

public class TokenService(TokenOption tokenOption) : ITokenService, IScopedDependency
{
    public CreateTokenResponse CreateToken(CreateTokenRequest createToken)
    {
        DateTime accessTokenExpiration = DateTime.Now.AddMinutes(tokenOption.AccessTokenExpiration);
        DateTime refreshTokenExpiration = DateTime.Now.AddMinutes(tokenOption.RefreshTokenExpiration);
        SecurityKey securityKey = SignService.GetSymmetricSecurityKey(tokenOption.SecurityKey);

        SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

        JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
            tokenOption.Issuer,
            expires: accessTokenExpiration,
            notBefore: DateTime.Now,
            claims: GetClaims(createToken),
            signingCredentials: signingCredentials);

        JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

        string token = handler.WriteToken(jwtSecurityToken);

        string refreshToken = Guid.NewGuid().ToString();

        return new CreateTokenResponse(token, refreshToken, accessTokenExpiration, refreshTokenExpiration);
    }

    private IEnumerable<Claim> GetClaims(CreateTokenRequest createTokenModel)
    {
        List<Claim> userList = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, createTokenModel.UserId.ToString()),
            new(JwtRegisteredClaimNames.Email, createTokenModel.Email!),
            new(ClaimTypes.Name, createTokenModel.UserName!),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };


        return userList;
    }
}