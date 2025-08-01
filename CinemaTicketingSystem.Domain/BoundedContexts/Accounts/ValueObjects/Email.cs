using System.Text.RegularExpressions;

namespace CinemaTicketingSystem.Domain.BoundedContexts.Accounts.ValueObjects;

public class Email : ValueObject
{
    private static readonly Regex EmailRegex = new(
        @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidOperationException("Email cannot be empty.");

        var normalizedEmail = value.Trim().ToLowerInvariant();

        if (!IsValidEmail(normalizedEmail))
            throw new InvalidOperationException($"Invalid email format: {value}");

        if (normalizedEmail.Length > 254) // RFC 5321 limit
            throw new InvalidOperationException("Email address is too long.");

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

        try
        {
            return EmailRegex.IsMatch(email);
        }
        catch
        {
            return false;
        }
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