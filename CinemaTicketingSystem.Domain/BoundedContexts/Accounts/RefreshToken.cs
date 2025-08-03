using CinemaTicketingSystem.Domain.BoundedContexts.Accounts.ValueObjects;

namespace CinemaTicketingSystem.Domain.BoundedContexts.Accounts;

public class RefreshToken : SharedKernel.AggregateRoot.AggregateRoot
{
    protected RefreshToken()
    {
    }

    public RefreshToken(string token, DateTime expiration, UserId userId)
    {
        Token = token;
        Expiration = expiration;
        UserId = userId;
    }

    public string Token { get; set; } = null!;
    public DateTime Expiration { get; set; }
    public bool IsExpired => DateTime.UtcNow >= Expiration;
    public UserId UserId { get; set; }


    protected override object?[] GetKeys()
    {
        return [UserId];
    }
}