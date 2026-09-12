using Microsoft.AspNetCore.Mvc;
using PetCareAI.Application.DTOs;
using PetCareAI.Application.Services;

namespace PetCareAI.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConsultasController : ControllerBase
    {
        private readonly ConsultaService _consultaService;

        public ConsultasController(ConsultaService consultaService)
        {
            _consultaService = consultaService;
        }

        [HttpPost]
        public async Task<IActionResult> CriarConsulta([FromBody] CreateConsultaDto dto)
        {
            var resultado = await _consultaService.CadastrarConsultaAsync(dto);
            return CreatedAtAction(nameof(ObterPorId), new { id = resultado.Id }, resultado);
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodas()
        {
            var consultas = await _consultaService.ObterTodasAsync();
            return Ok(consultas);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            try
            {
                var consulta = await _consultaService.ObterPorIdAsync(id);
                return Ok(consulta);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("pet/{petId}")]
        public async Task<IActionResult> ObterPorPetId(int petId)
        {
            var consultas = await _consultaService.ObterPorPetIdAsync(petId);
            return Ok(consultas);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] UpdateConsultaDto dto)
        {
            try
            {
                var resultado = await _consultaService.AtualizarAsync(id, dto);
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
                await _consultaService.RemoverAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
