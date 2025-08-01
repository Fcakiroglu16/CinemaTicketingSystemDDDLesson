using CinemaTicketingSystem.Application.Abstraction;
using CinemaTicketingSystem.Application.Abstraction.Catalog.Cinema;
using CinemaTicketingSystem.Application.Abstraction.Catalog.Cinema.Hall;
using CinemaTicketingSystem.Application.Abstraction.CinemaManagement.Cinema;
using CinemaTicketingSystem.Application.Abstraction.CinemaManagement.Cinema.Hall;
using CinemaTicketingSystem.Application.Abstraction.DependencyInjections;
using CinemaTicketingSystem.Domain.Catalog;
using CinemaTicketingSystem.Domain.Catalog.Repositories;
using CinemaTicketingSystem.Domain.Core;
using CinemaTicketingSystem.Domain.ValueObjects;
using System.Net;

namespace CinemaTicketingSystem.Application.Catalog.Cinema;

public class CinemaAppService(
    ICinemaRepository cinemaRepository,
    AppDependencyService appDependencyService) : IScopedDependency, ICinemaAppService
{
    public async Task<AppResult> CreateAsync(CreateCinemaRequest request)
    {
        var existCinema = await cinemaRepository.ExistsAsync(c => c.Name.Equals(request.Name));
        if (existCinema)
            return appDependencyService.Error(ErrorCodes.CinemaAlreadyExists, [request.Name]);

        var addressDto = request.Address;

        var address = new Address(addressDto.Country,
            addressDto.City,
            addressDto.District,
            addressDto.Street,
            addressDto.PostalCode,
            addressDto.Description);


        var newCinema = new Domain.Catalog.Cinema(request.Name, address);


        await cinemaRepository.AddAsync(newCinema);
        await appDependencyService.UnitOfWork.SaveChangesAsync();
        return AppResult.SuccessAsNoContent();
    }


    public async Task<AppResult> AddHallAsync(Guid cinemaId, AddCinemaHallRequest request)
    {
        var cinema = await cinemaRepository.GetByIdAsync(cinemaId);

        if (cinema is null)
            return appDependencyService.Error(ErrorCodes.CinemaNotFound, [HttpStatusCode.NotFound]);

        var existCinemaHall = cinema.Halls
            .FirstOrDefault(x => x.Name.Equals(request.Name, StringComparison.OrdinalIgnoreCase));


        if (existCinemaHall is not null)
            return appDependencyService.Error(ErrorCodes.CinemaHallAlreadyExists, [request.Name]);


        var cinemaHall = new CinemaHall(request.Name,
            request.Technologies.Cast<ScreeningTechnology>().Aggregate((x, y) => x | y));


        request.SeatList.ForEach(seatDto =>
            cinemaHall.AddSeat(new Seat(new SeatPosition(seatDto.Row, seatDto.Number), seatDto.SeatType)));


        cinema.AddHall(cinemaHall);


        await cinemaRepository.UpdateAsync(cinema);
        await appDependencyService.UnitOfWork.SaveChangesAsync();
        return AppResult.SuccessAsNoContent();
    }

    public async Task<AppResult> RemoveHallAsync(RemoveCinemaHallRequest request)
    {
        var cinema = await cinemaRepository.GetByIdAsync(request.CinemaId);
        if (cinema is null)


            return appDependencyService.Error(ErrorCodes.CinemaNotFound, HttpStatusCode.NotFound);


        var hall = cinema.GetHall(request.HallId);
        if (hall is null)
            return appDependencyService.Error(ErrorCodes.CinemaHallNotFound, HttpStatusCode.NotFound);


        cinema.RemoveHall(request.HallId);
        await cinemaRepository.UpdateAsync(cinema);
        await appDependencyService.UnitOfWork.SaveChangesAsync();
        return AppResult.SuccessAsNoContent();
    }

    public async Task<AppResult<List<CinemaHallDto>>> GetCinemaHallsAsync(Guid cinemaId)
    {
        var cinema = await cinemaRepository.GetByIdAsync(cinemaId);
        if (cinema is null)
            return appDependencyService.Error<List<CinemaHallDto>>(ErrorCodes.CinemaNotFound);

        if (!cinema.Halls.Any()) return AppResult<List<CinemaHallDto>>.SuccessAsOk([]);


        var cinemaHalls = cinema.Halls.Select(x => new CinemaHallDto(x.Id, x.Name,
                Enum.GetValues<ScreeningTechnology>()
                    .Where(tech => x.SupportedTechnologies.HasFlag(tech))
                    .Select(tech => (int)tech)
                    .ToArray(),
                x.Seats.Select(y => new SeatDto(y.SeatPosition.Row, y.SeatPosition.Number, y.Type)).ToList()))
            .ToList();


        return AppResult<List<CinemaHallDto>>.SuccessAsOk(cinemaHalls);
    }


    public async Task<AppResult<CinemaDto>> GetAsync(Guid cinemaId)
    {
        var cinema = await cinemaRepository.GetByIdAsync(cinemaId);
        if (cinema is null)
            return appDependencyService.Error<CinemaDto>(ErrorCodes.CinemaNotFound);

        var cinemaDto = new CinemaDto(cinema.Id, cinema.Name,
            new AddressDto(cinema.Address.Country, cinema.Address.City, cinema.Address.District,
                cinema.Address.Street, cinema.Address.PostalCode, cinema.Address.Description));

        return AppResult<CinemaDto>.SuccessAsOk(cinemaDto);
    }

    public async Task<AppResult<List<CinemaDto>>> GetAllAsync()
    {
        var cinemas = (await cinemaRepository.GetAllAsync()).ToList();
        if (!cinemas.Any()) return AppResult<List<CinemaDto>>.SuccessAsOk([]);

        var cinemaDtos = cinemas.ToList().Select(x => new CinemaDto(x.Id, x.Name,
                new AddressDto(x.Address.Country, x.Address.City, x.Address.District, x.Address.Street,
                    x.Address.PostalCode, x.Address.Description)))
            .ToList();

        return AppResult<List<CinemaDto>>.SuccessAsOk(cinemaDtos);
    }
}