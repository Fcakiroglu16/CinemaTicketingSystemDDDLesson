namespace CinemaTicketingSystem.Application.Test
{
    public class TicketIssuanceAppServiceTests(IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
    {
        [Fact]
        public void Test1()
        {
            var movieList = DbContext.Movies.ToList();

            Assert.Single(movieList);
        }
    }
}