using CinemaTicketingSystem.Application.Abstraction;
using CinemaTicketingSystem.Application.Abstraction.CinemaManagement.Cinema;
using CinemaTicketingSystem.Application.Abstraction.CinemaManagement.Cinema.Hall;
using CinemaTicketingSystem.Application.Abstraction.DependencyInjections;
using CinemaTicketingSystem.Domain.CinemaManagement;
using CinemaTicketingSystem.Domain.Repositories;
using System.Net;

namespace CinemaTicketingSystem.Application.CinemaManagement.Cinema
{
    public class CinemaAppService(ICinemaRepository cinemaRepository, IUnitOfWork unitOfWork) : IScopedDependency, ICinemaAppService
    {

        public async Task<AppResult> CreateAsync(CreateCinemaRequest request)
        {

            var addressDto = request.Address;

            var address = new Address(addressDto.Country,
                addressDto.City,
                addressDto.District,
                addressDto.Street,
                addressDto.PostalCode,
                addressDto.Description);


            var newCinema = new Domain.CinemaManagement.Cinema(request.Name, address);


            await cinemaRepository.AddAsync(newCinema);
            await unitOfWork.SaveChangesAsync();
            return AppResult.SuccessAsNoContent();
        }


        public async Task<AppResult> AddHallAsync(AddCinemaHallRequest request)
        {

            var cinema = await cinemaRepository.GetByIdAsync(request.CinemaId);

            if (cinema is null)
                return AppResult.Error("Cinema not found. The specified cinema ID does not exist in the system.",
                    HttpStatusCode.NotFound);


            var existCinemaHall = cinema.Halls
                .FirstOrDefault(x => x.Name.Equals(request.Name, StringComparison.OrdinalIgnoreCase));


            if (existCinemaHall is not null)
                return AppResult.Error($"Cinema hall with name '{request.Name}' already exists in this cinema",
                    HttpStatusCode.BadRequest);



            var cinemaHall = new CinemaHall(request.Name,
                request.Technologies.Cast<HallTechnology>().Aggregate((x, y) => x | y));





            request.SeatList.ForEach(seatDto =>
                cinemaHall.AddSeat(new Seat(seatDto.Row, seatDto.Number, seatDto.seatType)));

            cinema.AddHall(cinemaHall);
            await cinemaRepository.UpdateAsync(cinema);
            await unitOfWork.SaveChangesAsync();
            return AppResult.SuccessAsNoContent();


        }

        public async Task<AppResult> RemoveHallAsync(RemoveCinemaHallRequest request)
        {
            var cinema = await cinemaRepository.GetByIdAsync(request.CinemaId);
            if (cinema is null)


                return AppResult.Error("Cinema not found. The specified cinema ID does not exist in the system.",
                    HttpStatusCode.NotFound);


            var hall = cinema.GetHall(request.HallId);
            if (hall is null)
            {

                return AppResult.Error("Hall not found. The specified hall ID does not exist in the system.",
                    HttpStatusCode.NotFound);
            }


            cinema.RemoveHall(request.HallId);
            await cinemaRepository.UpdateAsync(cinema);
            await unitOfWork.SaveChangesAsync();
            return AppResult.SuccessAsNoContent();


        }

        public async Task<AppResult<List<CinemaHallDto>>> GetCinemaHallsAsync(Guid cinemaId)
        {


            var cinema = await cinemaRepository.GetByIdAsync(cinemaId);
            if (cinema is null)
                return AppResult<List<CinemaHallDto>>.Error("Cinema not found. The specified cinema ID does not exist in the system.",
                    HttpStatusCode.NotFound);



            if (!cinema.Halls.Any()) return AppResult<List<CinemaHallDto>>.SuccessAsOk([]);





            var cinemaHalls = cinema.Halls.Select(x => new CinemaHallDto(x.Name,
                    Enum.GetValues<HallTechnology>()
                        .Where(tech => x.SupportedTechnologies.HasFlag(tech))
                        .Select(tech => (int)tech)
                        .ToArray(),
                    x.Seats.Select(y => new SeatDto(y.Row, y.Number, y.Type)).ToList()))
                .ToList();



            return AppResult<List<CinemaHallDto>>.SuccessAsOk(cinemaHalls);



        }



        public async Task<AppResult<CinemaDto>> GetAllAsync(Guid cinemaId)
        {
            var cinema = await cinemaRepository.GetByIdAsync(cinemaId);
            if (cinema is null)
                return AppResult<CinemaDto>.Error("Cinema not found. The specified cinema ID does not exist in the system.",
                    HttpStatusCode.NotFound);



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
}
