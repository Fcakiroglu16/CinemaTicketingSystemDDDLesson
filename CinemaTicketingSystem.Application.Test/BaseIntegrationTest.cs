using CinemaTicketingSystem.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace CinemaTicketingSystem.Application.Test
{
    public class BaseIntegrationTest : IClassFixture<IntegrationTestWebAppFactory>, IDisposable
    {
        private readonly IServiceScope _scope;

        protected readonly AppDbContext DbContext;


        protected BaseIntegrationTest(IntegrationTestWebAppFactory factory)
        {
            _scope = factory.Services.CreateScope();


            DbContext = _scope.ServiceProvider
                .GetRequiredService<AppDbContext>();
        }

        public void Dispose()
        {
            _scope.Dispose();
            DbContext.Dispose();
        }
    }
}