using CinemaTicketingSystem.Application.Contracts.Contracts;
using CinemaTicketingSystem.Infrastructure.Persistence;
using CinemaTicketingSystem.SharedKernel;
using Microsoft.Extensions.DependencyInjection;

namespace CinemaTicketingSystem.Application.Test
{
    public class BaseIntegrationTest : IClassFixture<IntegrationTestWebAppFactory>, IDisposable
    {
        private readonly IServiceScope _scope;

        protected readonly AppDbContext DbContext;
        protected readonly IServiceProvider ServiceProvider;


        protected BaseIntegrationTest(IntegrationTestWebAppFactory factory)
        {
            _scope = factory.Services.CreateScope();


            DbContext = _scope.ServiceProvider
                .GetRequiredService<AppDbContext>();

            ServiceProvider = _scope.ServiceProvider;
        }

        protected T GetService<T>() where T : class
        {
            return ServiceProvider.GetRequiredService<T>();
        }

        protected Guid GetTestUserId() => Guid.NewGuid();

        public void Dispose()
        {
            _scope.Dispose();
            DbContext.Dispose();
        }
    }

    // Fake UserContext for testing
    public class FakeUserContext : IUserContext
    {
        private readonly Guid _userId;

        public FakeUserContext(Guid userId)
        {
            _userId = userId;
        }

        public Guid UserId => _userId;
        public string UserName => "TestUser";
        public string Email => "test@example.com";
    }
}