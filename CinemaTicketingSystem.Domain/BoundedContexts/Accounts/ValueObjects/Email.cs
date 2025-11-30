#region

using Ardalis.GuardClauses;
using CinemaTicketingSystem.SharedKernel.ValueObjects;
using System.Text.RegularExpressions;

#endregion

namespace CinemaTicketingSystem.Domain.BoundedContexts.Accounts.ValueObjects;

public class Email : ValueObject
{
    private static readonly Regex EmailRegex = new(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public Email(string value)
    {
        Guard.Against.NullOrWhiteSpace(value, nameof(value), "Email cannot be empty.");

        string normalizedEmail = value.Trim().ToLowerInvariant();

        Guard.Against.InvalidInput(normalizedEmail, nameof(value), IsValidEmail,
            $"Invalid email format: {value}");


        Guard.Against.InvalidInput(normalizedEmail, nameof(value), email => email.Length < 254,
            "Email address is too long."); // RFC 5321 limit

        Value = normalizedEmail;
    }

    public string Value { get; }

    public string Domain => Value.Split('@')[1];
    public string LocalPart => Value.Split('@')[0];

    public static Email From(string value)
    {
        return new Email(value);
    }

    private static bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;


        return EmailRegex.IsMatch(email);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString()
    {
        return Value;
    }

    public static implicit operator string(Email email)
    {
        return email.Value;
    }

    public static implicit operator Email(string value)
    {
        return new Email(value);
    }
}