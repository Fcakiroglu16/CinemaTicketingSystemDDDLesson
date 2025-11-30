using CinemaTicketingSystem.Domain.BoundedContexts.Catalog;

namespace CinemaTicketingSystem.Application.Test
{
    public class TicketIssuanceAppServiceTests(IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
    {
        [Fact]
        public void Test1()
        {
            List<Movie> movieList = DbContext.Movies.ToList();

            Assert.Single(movieList);
        }
    }
}