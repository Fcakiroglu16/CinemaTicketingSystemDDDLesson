namespace CinemaTicketingSystem.Application.Contracts.Accounts;

public record SignUpRequest(string Email, string Password, string FirstName, string LastName);