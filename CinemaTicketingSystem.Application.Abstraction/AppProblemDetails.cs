namespace CinemaTicketingSystem.Application.Abstraction;

public class AppProblemDetails
{
    public string? Title { get; set; }
    public string? Detail { get; set; }
    public int? Status { get; set; }
    public IDictionary<string, object?>? Extensions { get; set; }
}