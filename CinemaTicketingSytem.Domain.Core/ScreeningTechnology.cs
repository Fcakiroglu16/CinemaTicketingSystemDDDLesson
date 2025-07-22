namespace CinemaTicketingSystem.Domain.Core;

[Flags]
public enum ScreeningTechnology
{
    Standard = 1,
    IMAX = 2,
    ThreeD = 4,
    FourDX = 8
}