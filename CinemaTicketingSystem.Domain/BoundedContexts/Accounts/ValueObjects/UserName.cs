#region

using Ardalis.GuardClauses;
using CinemaTicketingSystem.SharedKernel.ValueObjects;
using System.Text.RegularExpressions;

#endregion

namespace CinemaTicketingSystem.Domain.BoundedContexts.Accounts.ValueObjects;

public class UserName : ValueObject
{
    private const int MinLength = 3;
    private const int MaxLength = 20;
    private static readonly Regex UserNameRegex = new(@"^[a-zA-Z0-9_.-]+$", RegexOptions.Compiled);

    public UserName(string value)
    {
        Guard.Against.NullOrWhiteSpace(value, nameof(value), "Username cannot be empty.");

        Guard.Against.InvalidInput(value, nameof(value),
            username => username.Length is > MinLength or < MaxLength,
            $"Username must be between {MinLength} and {MaxLength} characters.");

        Guard.Against.InvalidInput(value, nameof(value),
            username => UserNameRegex.IsMatch(username),
            "Username must contain only letters, numbers, underscores, dots or hyphens.");

        Value = value;
    }

    public string Value { get; }

    public static UserName From(string value)
    {
        return new UserName(value);
    }

    public static UserName GenerateFromEmail(string email)
    {
        Guard.Against.NullOrWhiteSpace(email, nameof(email), "Email cannot be empty for username generation.");

        string localPart = email.Split('@')[0];
        string cleaned = Regex.Replace(localPart, @"[^a-zA-Z0-9_.]", "");
        if (cleaned.Length < MinLength)
            cleaned = cleaned.PadRight(MinLength, 'x');
        if (cleaned.Length > MaxLength - 4)
            cleaned = cleaned.Substring(0, MaxLength - 4);
        Random random = new Random();
        string suffix = random.Next(1000, 9999).ToString();
        string username = cleaned + suffix;
        return new UserName(username);
    }

    public static implicit operator string(UserName userName)
    {
        return userName.Value;
    }

    public static implicit operator UserName(string value)
    {
        return new UserName(value);
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