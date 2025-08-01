using System.Text.RegularExpressions;

namespace CinemaTicketingSystem.Domain.BoundedContexts.Accounts.ValueObjects;

public class UserName : ValueObject
{
    private const int MinLength = 3;
    private const int MaxLength = 20;
    private static readonly Regex UserNameRegex = new(@"^[a-zA-Z0-9_.-]+$", RegexOptions.Compiled);

    public string Value { get; }

    public UserName(string value)
    {
        if (!IsValid(value))
            throw new ArgumentException("Username must be 3-20 characters and contain only letters, numbers, underscores, dots or hyphens.", nameof(value));

        Value = value;
    }

    public static UserName From(string value) => new(value);

    public static UserName GenerateFromEmail(string email)
    {
        var localPart = email.Split('@')[0];
        var cleaned = Regex.Replace(localPart, @"[^a-zA-Z0-9_.-]", "");
        if (cleaned.Length < MinLength)
            cleaned = cleaned.PadRight(MinLength, 'x');
        if (cleaned.Length > MaxLength - 4)
            cleaned = cleaned.Substring(0, MaxLength - 4);
        var random = new Random();
        var suffix = random.Next(1000, 9999).ToString();
        var username = cleaned + suffix;
        return new UserName(username);
    }

    public static implicit operator string(UserName userName) => userName.Value;

    public static implicit operator UserName(string value) => new(value);

    private static bool IsValid(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) return false;
        if (value.Length < MinLength || value.Length > MaxLength) return false;
        if (!UserNameRegex.IsMatch(value)) return false;
        return true;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
