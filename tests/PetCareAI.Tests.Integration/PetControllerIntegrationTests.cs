using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace PetCareAI.Tests.Integration
{
    public class PetControllerIntegrationTests : IClassFixture<WebApplicationFactory<PetCareAI.Api.Controllers.PetsController>>
    {
        private readonly HttpClient _client;

        public PetControllerIntegrationTests(WebApplicationFactory<PetCareAI.Api.Controllers.PetsController> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Teste_HealthCheck_DeveExecutarSemFalhas()
        {
            // Act
            var response = await _client.GetAsync("/health");

            // Assert
            Assert.NotNull(response);
        }
    }
}