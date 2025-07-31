using CinemaTicketingSystem.Application.Abstraction.Contracts;
using CinemaTicketingSystem.SharedKernel;
using System.Security.Claims;

namespace CinemaTicketingSystem.Host.Identities
{
    internal class UserContext(IHttpContextAccessor httpContextAccessor, ILogger<UserContext> logger, ILocalizer localizer) : IUserContext
    {
        public Guid UserId
        {
            get
            {
                if (!httpContextAccessor.HttpContext!.User.Identity!.IsAuthenticated)
                {

                    throw new UnauthorizedAccessException("User is not authenticated.");
                }
                return Guid.Parse(httpContextAccessor.HttpContext.User.Claims
                    .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
                    ?.Value!);

            }
        }

        public string UserName
        {
            get
            {

                if (!httpContextAccessor.HttpContext!.User.Identity!.IsAuthenticated)
                {
                    throw new UnauthorizedAccessException("User is not authenticated.");
                }

                return httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)
                    ?.Value!;
            }
        }

        public string Email
        {
            get
            {

                if (!httpContextAccessor.HttpContext!.User.Identity!.IsAuthenticated)
                {
                    throw new UnauthorizedAccessException("User is not authenticated.");
                }

                return httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)
                    ?.Value!;
            }
        }
    }
}
