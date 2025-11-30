#region

using Ardalis.GuardClauses;
using CinemaTicketingSystem.SharedKernel.ValueObjects;
using System.Text.RegularExpressions;

#endregion

namespace CinemaTicketingSystem.Domain.BoundedContexts.Accounts.ValueObjects;

public class Password : ValueObject
{
    private const int MinLength = 8;
    private const int MaxLength = 12;

    private static readonly Regex UpperCaseRegex = new(@"[A-Z]", RegexOptions.Compiled);
    private static readonly Regex LowerCaseRegex = new(@"[a-z]", RegexOptions.Compiled);
    private static readonly Regex DigitRegex = new(@"\d", RegexOptions.Compiled);
    private static readonly Regex NonAlphaNumericRegex = new(@"[^a-zA-Z0-9]", RegexOptions.Compiled);

    public Password(string value)
    {
        Guard.Against.NullOrWhiteSpace(value, nameof(value), "Password cannot be empty.");

        Guard.Against.InvalidInput(value, nameof(value),
            password => password.Length is > MinLength or < MaxLength,
            $"Password must be between {MinLength} and {MaxLength} characters.");

        Guard.Against.InvalidInput(value, nameof(value),
            password => UpperCaseRegex.IsMatch(password),
            "Password must contain at least one uppercase letter.");

        Guard.Against.InvalidInput(value, nameof(value),
            password => LowerCaseRegex.IsMatch(password),
            "Password must contain at least one lowercase letter.");

        Guard.Against.InvalidInput(value, nameof(value),
            password => DigitRegex.IsMatch(password),
            "Password must contain at least one digit.");

        Guard.Against.InvalidInput(value, nameof(value),
            password => NonAlphaNumericRegex.IsMatch(password),
            "Password must contain at least one special character.");

        Value = value;
    }

    public string Value { get; }

    public static Password From(string value)
    {
        return new Password(value);
    }

    public static implicit operator string(Password password)
    {
        return password.Value;
    }

    public static implicit operator Password(string value)
    {
        return new Password(value);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString()
    {
        return Value;
    }
}