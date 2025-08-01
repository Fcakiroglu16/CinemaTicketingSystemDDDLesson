using CinemaTicketingSystem.Domain.Accounts.ValueObjects;
using CinemaTicketingSystem.Domain.BoundedContexts.Accounts.ValueObjects;

namespace CinemaTicketingSystem.Domain.BoundedContexts.Accounts;

public class User : Entity<UserId>
{
    public User(Email email, Password password, string firstName, string lastName)
    {
        Id = UserId.New();
        Email = email;
        Password = password;
        FirstName = firstName;
        LastName = lastName;
        CreatedAt = DateTime.UtcNow;
        IsActive = true;
        UserName = UserName.GenerateFromEmail(email);

    }
    public User(UserId userId, UserName userName, Email email, string firstName, string lastName, DateTime createdAt)
    {

        Id = userId;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        CreatedAt = createdAt;
        IsActive = true;
        UserName = userName;

    }

    public UserName UserName { get; set; }

    public Password Password { get; private set; }
    public Email Email { get; private set; }
    public string? FirstName { get; private set; }
    public string? LastName { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public string FullName => $"{FirstName} {LastName}";


    public void SetFirstName(string firstName)
    {

        FirstName = firstName;

    }

    public void SetLastName(string lastName)
    {
        LastName = lastName;
    }



    public void UpdateProfile(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateEmail(string email)
    {
        Email = Email.From(email);
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }
}