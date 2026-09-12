using Microsoft.AspNetCore.Mvc;
using PetCareAI.Application.DTOs;
using PetCareAI.Application.Services;

namespace PetCareAI.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PetsController : ControllerBase
    {
        private readonly PetService _petService;

        public PetsController(PetService petService)
        {
            _petService = petService;
        }

        [HttpPost]
        public async Task<IActionResult> CriarPet([FromBody] CreatePetDto dto)
        {
            var resultado = await _petService.CadastrarPetAsync(dto);
            return CreatedAtAction(nameof(ObterPorId), new { id = resultado.Id }, resultado);
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodos()
        {
            var pets = await _petService.ObterTodosAsync();
            return Ok(pets);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            try
            {
                var pet = await _petService.ObterPorIdAsync(id);
                return Ok(pet);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] UpdatePetDto dto)
        {
            try
            {
                var resultado = await _petService.AtualizarAsync(id, dto);
                return Ok(resultado);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remover(int id)
        {
            try
            {
                await _petService.RemoverAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
