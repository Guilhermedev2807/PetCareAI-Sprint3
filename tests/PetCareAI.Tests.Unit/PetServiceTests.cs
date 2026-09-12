using Moq;
using Xunit;
using FluentAssertions;
using PetCareAI.Application.Services;
using PetCareAI.Domain.Entities;
using PetCareAI.Domain.Interfaces;
using PetCareAI.Application.DTOs;

namespace PetCareAI.Tests.Unit.Services
{
    public class PetServiceTests
    {
        private readonly Mock<IPetRepository> _petRepositoryMock;
        private readonly PetService _petService;

        public PetServiceTests()
        {
            _petRepositoryMock = new Mock<IPetRepository>();
            _petService = new PetService(_petRepositoryMock.Object);
        }

        [Fact]
        public async Task CadastrarPet_ComDadosValidos_DeveRetornarPetCriado()
        {
            // ARRANGE (Preparar)
            var petDto = new CreatePetDto
            {
                Nome = "Thor",
                Especie = "Cachorro",
                Raca = "Golden Retriever",
                Idade = 3,
                Peso = 30.5,
                UsuarioId = 1
            };

            var petEntidade = new Pet(petDto.Nome, petDto.Especie, petDto.Raca, petDto.Idade, petDto.Peso, petDto.UsuarioId) 
            { 
                Id = 10 
            };

            _petRepositoryMock
                .Setup(r => r.AdicionarAsync(It.IsAny<Pet>()))
                .ReturnsAsync(petEntidade);

            // ACT (Agir)
            var resultado = await _petService.CadastrarPetAsync(petDto);

            // ASSERT (Validar)
            resultado.Should().NotBeNull();
            resultado.Id.Should().Be(10);
            resultado.Nome.Should().Be("Thor");
            _petRepositoryMock.Verify(r => r.AdicionarAsync(It.IsAny<Pet>()), Times.Once);
        }

        [Fact]
        public async Task ObterPorId_QuandoPetNaoExiste_DeveLancarExcecao()
        {
            // ARRANGE
            int petId = 999;
            _petRepositoryMock
                .Setup(r => r.ObterPorIdAsync(petId))
                .ReturnsAsync((Pet?)null);

            // ACT
            Func<Task> act = async () => await _petService.ObterPorIdAsync(petId);

            // ASSERT
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage($"Pet com ID {petId} não foi encontrado.");
        }
    }
}