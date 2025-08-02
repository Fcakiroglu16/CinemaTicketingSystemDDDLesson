namespace CinemaTicketingSystem.Application.Abstraction.Accounts;

public record SignUpRequest(string Email, string Password, string FirstName, string LastName);