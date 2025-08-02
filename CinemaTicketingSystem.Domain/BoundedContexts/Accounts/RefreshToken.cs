using CinemaTicketingSystem.Domain.BoundedContexts.Accounts.ValueObjects;

namespace CinemaTicketingSystem.Domain.BoundedContexts.Accounts
{
    public class RefreshToken : SharedKernel.AggregateRoot.AggregateRoot
    {

        public string Token { get; set; } = null!;
        public DateTime Expires { get; set; }
        public bool IsExpired => DateTime.UtcNow >= Expires;
        public UserId UserId { get; set; }


        protected RefreshToken() { }

        public RefreshToken(string token, DateTime expires, UserId userId)
        {
            Token = token;
            Expires = expires;
            UserId = userId;
        }


        public override object?[] GetKeys()
        {
            return [UserId];
        }
    }
}
