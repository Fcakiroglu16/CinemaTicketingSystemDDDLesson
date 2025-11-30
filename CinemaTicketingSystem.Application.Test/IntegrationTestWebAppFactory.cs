using CinemaTicketingSystem.Domain.BoundedContexts.Catalog;
using CinemaTicketingSystem.Domain.BoundedContexts.Scheduling;
using CinemaTicketingSystem.Infrastructure.Persistence;
using CinemaTicketingSystem.SharedKernel;
using CinemaTicketingSystem.SharedKernel.ValueObjects;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Testcontainers.MsSql;

namespace CinemaTicketingSystem.Application.Test
{
    public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private const string Password = "yourStrong(!)Password";
        private const ushort MsSqlPort = 1438;

        private readonly MsSqlContainer _mssqlContainer = new MsSqlBuilder()
            .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
            .WithPortBinding(MsSqlPort)
            .WithEnvironment("ACCEPT_EULA", "Y")
            .WithEnvironment("MSSQL_SA_PASSWORD", Password)
            .Build();


        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.RemoveAll(typeof(DbContextOptions<AppDbContext>));


                services.AddDbContext<AppDbContext>(options =>
                    options.UseSqlServer(_mssqlContainer.GetConnectionString()));

                Dictionary<string, string> inMemorySettings = new Dictionary<string, string>();

                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .AddInMemoryCollection(inMemorySettings)
                    .Build();


                //services.RegisterServiceBus(configuration);


                //services.AddMediatR(configuration =>
                //{
                //    configuration.RegisterServicesFromAssemblies(typeof(ApplicationAssembly).Assembly,
                //        typeof(DomainAssembly).Assembly,
                //        typeof(PersistenceAssembly).Assembly);
                //});
            });


            base.ConfigureWebHost(builder);
        }

        public async Task InitializeAsync()
        {
            await _mssqlContainer.StartAsync();
            string connectionString = _mssqlContainer.GetConnectionString();
            DbContextOptionsBuilder<AppDbContext> optionsBuilder = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer(connectionString,
                    option => { option.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName); });

            await using AppDbContext dbContext = new AppDbContext(optionsBuilder.Options, null!, null!);


            await dbContext.Database.MigrateAsync();

            await dbContext.Database.EnsureCreatedAsync();

            Cinema newCinema = new Cinema("Galaxy Cinema Mall",
                new Address("Turkey", "Istanbul", "Beyoğlu", "İstiklal Caddesi No: 123", "34430",
                    "Located in the heart of Beyoğlu, next to Galatasaray High School"));


            CinemaHall newCinemaHall = new CinemaHall("Hall 1 - Standard", ScreeningTechnology.Standard);

            newCinemaHall.AddSeat(new Seat(new SeatPosition("A", 1), SeatType.Regular));
            newCinemaHall.AddSeat(new Seat(new SeatPosition("A", 2), SeatType.VIP));
            newCinemaHall.AddSeat(new Seat(new SeatPosition("A", 3), SeatType.Accessible));
            newCinemaHall.AddSeat(new Seat(new SeatPosition("B", 1), SeatType.Regular));
            newCinemaHall.AddSeat(new Seat(new SeatPosition("B", 2), SeatType.Regular));
            newCinemaHall.AddSeat(new Seat(new SeatPosition("B", 3), SeatType.VIP));


            newCinema.AddHall(newCinemaHall);


            await dbContext.Cinemas.AddAsync(newCinema);


            Movie newMovie = new Movie("Inception", new Duration((double)TimeSpan.FromMinutes(150).Minutes),
                "http://wwww.abc.com", ScreeningTechnology.Standard);


            newMovie.SetOriginalTitle("Inception");
            newMovie.SetDescription("A mind-bending thriller that delves into the world of dreams.");
            newMovie.SetEarliestShowingDate(DateTime.Parse("2026-07-16T00:00:00Z"));

            await dbContext.Movies.AddAsync(newMovie);


            Schedule newSchedule = new Schedule(newMovie.Id, newCinemaHall.Id,
                ShowTime.Create(new TimeOnly(19, 0), new TimeOnly(2, 0)), new Price(100, "TRY"));


            await dbContext.Schedules.AddAsync(newSchedule);


            await dbContext.SaveChangesAsync();
        }

        public new async Task DisposeAsync()
        {
            await _mssqlContainer.DisposeAsync();
        }
    }
}