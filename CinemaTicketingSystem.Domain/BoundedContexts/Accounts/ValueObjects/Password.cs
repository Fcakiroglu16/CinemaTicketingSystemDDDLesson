using System.Text.RegularExpressions;

namespace CinemaTicketingSystem.Domain.BoundedContexts.Accounts.ValueObjects;

public class Password : ValueObject
{
    private const int MinLength = 8;
    private const int MaxLength = 12;

    private static readonly Regex UpperCaseRegex = new(@"[A-Z]", RegexOptions.Compiled);
    private static readonly Regex LowerCaseRegex = new(@"[a-z]", RegexOptions.Compiled);
    private static readonly Regex DigitRegex = new(@"\d", RegexOptions.Compiled);
    private static readonly Regex NonAlphaNumericRegex = new(@"[^a-zA-Z0-9]", RegexOptions.Compiled);

    public string Value { get; }

    public Password(string value)
    {
        if (!IsValid(value))
            throw new ArgumentException("Password must be 8-12 characters and include at least one uppercase letter, one lowercase letter, one digit, and one non-alphanumeric character.", nameof(value));

        Value = value;
    }

    public static Password From(string value) => new(value);

    public static implicit operator string(Password password) => password.Value;

    public static implicit operator Password(string value) => new(value);

    private static bool IsValid(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) return false;
        if (value.Length < MinLength || value.Length > MaxLength) return false;
        if (!UpperCaseRegex.IsMatch(value)) return false;
        if (!LowerCaseRegex.IsMatch(value)) return false;
        if (!DigitRegex.IsMatch(value)) return false;
        if (!NonAlphaNumericRegex.IsMatch(value)) return false;
        return true;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
