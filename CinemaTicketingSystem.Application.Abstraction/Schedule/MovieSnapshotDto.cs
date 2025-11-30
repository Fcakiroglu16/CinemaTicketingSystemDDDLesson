namespace CinemaTicketingSystem.Application.Contracts.Schedule;

internal record MovieSnapshotDto(Guid MovieId, TimeOnly Start, TimeOnly End);