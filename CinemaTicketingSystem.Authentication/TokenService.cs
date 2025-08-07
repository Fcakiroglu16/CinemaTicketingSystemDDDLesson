#region

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using CinemaTicketingSystem.Application.Abstraction.Accounts;
using CinemaTicketingSystem.Application.Contracts.DependencyInjections;
using CinemaTicketingSystem.SharedKernel.Identities;
using CinemaTicketingSystem.SharedKernel.Options;
using Microsoft.IdentityModel.Tokens;

#endregion

namespace CinemaTicketingSystem.Identity;

public class TokenService(TokenOption tokenOption) : ITokenService, IScopedDependency
{
    public CreateTokenResponse CreateToken(CreateTokenRequest createToken)
    {
        var accessTokenExpiration = DateTime.Now.AddMinutes(tokenOption.AccessTokenExpiration);
        var refreshTokenExpiration = DateTime.Now.AddMinutes(tokenOption.RefreshTokenExpiration);
        var securityKey = SignService.GetSymmetricSecurityKey(tokenOption.SecurityKey);

        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

        var jwtSecurityToken = new JwtSecurityToken(
            tokenOption.Issuer,
            expires: accessTokenExpiration,
            notBefore: DateTime.Now,
            claims: GetClaims(createToken),
            signingCredentials: signingCredentials);

        var handler = new JwtSecurityTokenHandler();

        var token = handler.WriteToken(jwtSecurityToken);

        var refreshToken = Guid.NewGuid().ToString();

        return new CreateTokenResponse(token, refreshToken, accessTokenExpiration, refreshTokenExpiration);
    }

    private IEnumerable<Claim> GetClaims(CreateTokenRequest createTokenModel)
    {
        var userList = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, createTokenModel.UserId.ToString()),
            new(JwtRegisteredClaimNames.Email, createTokenModel.Email!),
            new(ClaimTypes.Name, createTokenModel.UserName!),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };


        return userList;
    }
}