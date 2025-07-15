using Microsoft.AspNetCore.Identity;

namespace CinemaTicketingSystem.Persistence.Accounts
{
    public class AppUser : IdentityUser<Guid>
    {
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        public bool IsActive { get; private set; }

        public string FirstName { get; private set; } = null!;
        public string LastName { get; private set; } = null!;
    }
}
