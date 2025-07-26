namespace CinemaTicketingSystem.Application.Abstraction.Contracts;

public interface ILocalizer
{
    string L(string key);

    string L(string key, params object[] data);
}