using Ardalis.GuardClauses;
using CinemaTicketingSystem.SharedKernel.ValueObjects;

namespace CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.ValueObjects
{
    public class CustomerId : ValueObject
    {
        public CustomerId(Guid value)
        {
            Value = Guard.Against.Default(value, nameof(value), "UserId cannot be empty.");
        }

        protected CustomerId()
        {
        }

        public Guid Value { get; }

        public static CustomerId New()
        {
            return new CustomerId(Guid.CreateVersion7());
        }

        public static CustomerId From(Guid value)
        {
            return new CustomerId(value);
        }

        public static CustomerId From(string value)
        {
            if (!Guid.TryParse(value, out var guid))
                throw new ArgumentException($"Invalid CustomerId format: {value}");

            return new CustomerId(guid);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public static implicit operator Guid(CustomerId userId)
        {
            return userId.Value;
        }

        public static implicit operator CustomerId(Guid value)
        {
            return new CustomerId(value);
        }
    }
}