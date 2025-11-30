#region

using CinemaTicketingSystem.Application.Contracts;
using CinemaTicketingSystem.Application.Contracts.Catalog.Cinema;
using CinemaTicketingSystem.Application.Contracts.Catalog.Cinema.Hall;
using CinemaTicketingSystem.Application.Contracts.DependencyInjections;
using CinemaTicketingSystem.Domain.BoundedContexts.Catalog;
using CinemaTicketingSystem.Domain.BoundedContexts.Catalog.Repositories;
using CinemaTicketingSystem.SharedKernel;
using CinemaTicketingSystem.SharedKernel.ValueObjects;
using System.Net;

#endregion

namespace CinemaTicketingSystem.Application.Catalog.Cinema;

public class CinemaAppService(
    ICinemaRepository cinemaRepository,
    AppDependencyService appDependencyService) : IScopedDependency, ICinemaAppService
{
    public async Task<AppResult> CreateAsync(CreateCinemaRequest request)
    {
        bool existCinema = await cinemaRepository.ExistsAsync(c => c.Name.Equals(request.Name));
        if (existCinema)
            return appDependencyService.LocalizeError.Error(ErrorCodes.CinemaAlreadyExists, [request.Name]);

        AddressDto addressDto = request.Address;

        Address address = new Address(addressDto.Country,
            addressDto.City,
            addressDto.District,
            addressDto.Street,
            addressDto.PostalCode,
            addressDto.Description);


        Domain.BoundedContexts.Catalog.Cinema newCinema = new Domain.BoundedContexts.Catalog.Cinema(request.Name, address);


        await cinemaRepository.AddAsync(newCinema);
        await appDependencyService.UnitOfWork.SaveChangesAsync();
        return AppResult.SuccessAsNoContent();
    }


    public async Task<AppResult> AddHallAsync(Guid cinemaId, AddCinemaHallRequest request)
    {
        Domain.BoundedContexts.Catalog.Cinema? cinema = await cinemaRepository.GetByIdAsync(cinemaId);

        if (cinema is null)
            return appDependencyService.LocalizeError.Error(ErrorCodes.CinemaNotFound, [HttpStatusCode.NotFound]);

        CinemaHall? existCinemaHall = cinema.Halls
            .FirstOrDefault(x => x.Name.Equals(request.Name, StringComparison.OrdinalIgnoreCase));


        if (existCinemaHall is not null)
            return appDependencyService.LocalizeError.Error(ErrorCodes.CinemaHallAlreadyExists, [request.Name]);


        CinemaHall cinemaHall = new CinemaHall(request.Name,
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
        Domain.BoundedContexts.Catalog.Cinema? cinema = await cinemaRepository.GetByIdAsync(request.CinemaId);
        if (cinema is null)


            return appDependencyService.LocalizeError.Error(ErrorCodes.CinemaNotFound, HttpStatusCode.NotFound);


        CinemaHall? hall = cinema.GetHall(request.HallId);
        if (hall is null)
            return appDependencyService.LocalizeError.Error(ErrorCodes.CinemaHallNotFound, HttpStatusCode.NotFound);


        cinema.RemoveHall(request.HallId);
        await cinemaRepository.UpdateAsync(cinema);
        await appDependencyService.UnitOfWork.SaveChangesAsync();
        return AppResult.SuccessAsNoContent();
    }

    public async Task<AppResult<List<CinemaHallDto>>> GetCinemaHallsAsync(Guid cinemaId)
    {
        Domain.BoundedContexts.Catalog.Cinema? cinema = await cinemaRepository.GetByIdAsync(cinemaId);
        if (cinema is null)
            return appDependencyService.LocalizeError.Error<List<CinemaHallDto>>(ErrorCodes.CinemaNotFound);

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
        Domain.BoundedContexts.Catalog.Cinema? cinema = await cinemaRepository.GetByIdAsync(cinemaId);
        if (cinema is null)
            return appDependencyService.LocalizeError.Error<CinemaDto>(ErrorCodes.CinemaNotFound);

        var cinemaDto = new CinemaDto(cinema.Id, cinema.Name,
            new AddressDto(cinema.Address.Country, cinema.Address.City, cinema.Address.District,
                cinema.Address.Street, cinema.Address.PostalCode, cinema.Address.Description));

        return AppResult<CinemaDto>.SuccessAsOk(cinemaDto);
    }

    public async Task<AppResult<List<CinemaDto>>> GetAllAsync()
    {
        List<Domain.BoundedContexts.Catalog.Cinema> cinemas = (await cinemaRepository.GetAllAsync()).ToList();
        if (!cinemas.Any()) return AppResult<List<CinemaDto>>.SuccessAsOk([]);

        var cinemaDtos = cinemas.ToList().Select(x => new CinemaDto(x.Id, x.Name,
                new AddressDto(x.Address.Country, x.Address.City, x.Address.District, x.Address.Street,
                    x.Address.PostalCode, x.Address.Description)))
            .ToList();

        return AppResult<List<CinemaDto>>.SuccessAsOk(cinemaDtos);
    }
}