using CinemaTicketingSystem.Domain.Scheduling;
using CinemaTicketingSystem.Domain.ValueObjects;

namespace CinemaTicketingSystem.Application.Schedules.ICL;

public record GetScheduleInfoResponse(Guid CinemaHallId, Guid MovieId, ShowTime showTime, Price TicketPrice);