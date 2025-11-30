namespace CinemaTicketingSystem.Application.Contracts.Accounts;

public record SignInResponse(
    string AccessToken,
    string RefreshToken,
    DateTime AccessTokenExpiration,
    DateTime RefreshTokenExpiration);