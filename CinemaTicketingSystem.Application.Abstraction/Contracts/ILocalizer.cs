namespace CinemaTicketingSystem.Application.Abstraction.Contracts;

public interface ILocalizer
{


    string L(string key, object[]? data);
}