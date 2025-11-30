#region

using Microsoft.IdentityModel.Tokens;
using System.Text;

#endregion

namespace CinemaTicketingSystem.Infrastructure.Authentication;

public static class SignService
{
    public static SecurityKey GetSymmetricSecurityKey(string securityKey)
    {
        return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
    }
}