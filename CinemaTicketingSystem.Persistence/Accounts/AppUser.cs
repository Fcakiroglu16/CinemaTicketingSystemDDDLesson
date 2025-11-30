#region

using Microsoft.AspNetCore.Identity;

#endregion

namespace CinemaTicketingSystem.Infrastructure.Persistence.Accounts;

public class AppUser : IdentityUser<Guid>
{
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public bool IsActive { get; set; }

    public string? FirstName { get; set; } = null!;
    public string? LastName { get; set; } = null!;
}