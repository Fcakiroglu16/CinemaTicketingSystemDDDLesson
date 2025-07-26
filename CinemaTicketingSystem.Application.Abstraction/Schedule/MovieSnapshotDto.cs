namespace CinemaTicketingSystem.Application.Abstraction.Schedule;

internal record MovieSnapshotDto(Guid MovieId, TimeOnly Start, TimeOnly End);