using Microsoft.EntityFrameworkCore;
using PetCareAI.Domain.Entities;
using PetCareAI.Application.DTOs;
using PetCareAI.Domain.Interfaces;

namespace PetCareAI.Infrastructure.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Pet> Pets { get; set; }
        public DbSet<Consulta> Consultas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Consulta>()
                .HasOne(c => c.Pet)
                .WithMany(p => p.Consultas)
                .HasForeignKey(c => c.PetId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    // ===================== PET =====================

    public class PetRepository : IPetRepository
    {
        private readonly AppDbContext _context;

        public PetRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Pet> AdicionarAsync(Pet pet)
        {
            await _context.Pets.AddAsync(pet);
            await _context.SaveChangesAsync();
            return pet;
        }

        public async Task<Pet?> ObterPorIdAsync(int id)
        {
            return await _context.Pets.FindAsync(id);
        }

        public async Task<List<Pet>> ObterTodosAsync()
        {
            return await _context.Pets.ToListAsync();
        }

        public async Task<Pet?> AtualizarAsync(int id, Pet petAtualizado)
        {
            var pet = await _context.Pets.FindAsync(id);
            if (pet == null) return null;

            pet.Nome = petAtualizado.Nome;
            pet.Especie = petAtualizado.Especie;
            pet.Raca = petAtualizado.Raca;
            pet.Idade = petAtualizado.Idade;
            pet.Peso = petAtualizado.Peso;

            await _context.SaveChangesAsync();
            return pet;
        }

        public async Task<bool> RemoverAsync(int id)
        {
            var pet = await _context.Pets.FindAsync(id);
            if (pet == null) return false;

            _context.Pets.Remove(pet);
            await _context.SaveChangesAsync();
            return true;
        }
    }

    // ===================== CONSULTA =====================

    public class ConsultaRepository : IConsultaRepository
    {
        private readonly AppDbContext _context;

        public ConsultaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Consulta> AdicionarAsync(Consulta consulta)
        {
            await _context.Consultas.AddAsync(consulta);
            await _context.SaveChangesAsync();
            return consulta;
        }

        public async Task<Consulta?> ObterPorIdAsync(int id)
        {
            return await _context.Consultas.FindAsync(id);
        }

        public async Task<List<Consulta>> ObterTodasAsync()
        {
            return await _context.Consultas.ToListAsync();
        }

        public async Task<List<Consulta>> ObterPorPetIdAsync(int petId)
        {
            return await _context.Consultas.Where(c => c.PetId == petId).ToListAsync();
        }

        public async Task<Consulta?> AtualizarAsync(int id, Consulta consultaAtualizada)
        {
            var consulta = await _context.Consultas.FindAsync(id);
            if (consulta == null) return null;

            consulta.DataConsulta = consultaAtualizada.DataConsulta;
            consulta.Veterinario = consultaAtualizada.Veterinario;
            consulta.Motivo = consultaAtualizada.Motivo;
            consulta.Diagnostico = consultaAtualizada.Diagnostico;

            await _context.SaveChangesAsync();
            return consulta;
        }

        public async Task<bool> RemoverAsync(int id)
        {
            var consulta = await _context.Consultas.FindAsync(id);
            if (consulta == null) return false;

            _context.Consultas.Remove(consulta);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

namespace PetCareAI.Domain.Interfaces
{
    using PetCareAI.Domain.Entities;

    public interface IPetRepository
    {
        Task<Pet> AdicionarAsync(Pet pet);
        Task<Pet?> ObterPorIdAsync(int id);
        Task<List<Pet>> ObterTodosAsync();
        Task<Pet?> AtualizarAsync(int id, Pet petAtualizado);
        Task<bool> RemoverAsync(int id);
    }

    public interface IConsultaRepository
    {
        Task<Consulta> AdicionarAsync(Consulta consulta);
        Task<Consulta?> ObterPorIdAsync(int id);
        Task<List<Consulta>> ObterTodasAsync();
        Task<List<Consulta>> ObterPorPetIdAsync(int petId);
        Task<Consulta?> AtualizarAsync(int id, Consulta consultaAtualizada);
        Task<bool> RemoverAsync(int id);
    }
}

namespace PetCareAI.Application.Services
{
    using PetCareAI.Domain.Entities;

    public class PetService
    {
        private readonly IPetRepository _repository;

        public PetService(IPetRepository repository)
        {
            _repository = repository;
        }

        private static PetResponseDto ToDto(Pet pet) => new()
        {
            Id = pet.Id,
            Nome = pet.Nome,
            Especie = pet.Especie,
            Raca = pet.Raca,
            Idade = pet.Idade,
            Peso = pet.Peso,
            UsuarioId = pet.UsuarioId
        };

        public async Task<PetResponseDto> CadastrarPetAsync(CreatePetDto dto)
        {
            var pet = new Pet(dto.Nome, dto.Especie, dto.Raca, dto.Idade, dto.Peso, dto.UsuarioId);
            var criado = await _repository.AdicionarAsync(pet);
            return ToDto(criado);
        }

        public async Task<PetResponseDto> ObterPorIdAsync(int id)
        {
            var pet = await _repository.ObterPorIdAsync(id);
            if (pet == null)
                throw new KeyNotFoundException($"Pet com ID {id} não foi encontrado.");

            return ToDto(pet);
        }

        public async Task<List<PetResponseDto>> ObterTodosAsync()
        {
            var pets = await _repository.ObterTodosAsync();
            return pets.Select(ToDto).ToList();
        }

        public async Task<PetResponseDto> AtualizarAsync(int id, UpdatePetDto dto)
        {
            var petAtualizado = new Pet(dto.Nome, dto.Especie, dto.Raca, dto.Idade, dto.Peso, 0);
            var resultado = await _repository.AtualizarAsync(id, petAtualizado);
            if (resultado == null)
                throw new KeyNotFoundException($"Pet com ID {id} não foi encontrado.");

            return ToDto(resultado);
        }

        public async Task RemoverAsync(int id)
        {
            var removido = await _repository.RemoverAsync(id);
            if (!removido)
                throw new KeyNotFoundException($"Pet com ID {id} não foi encontrado.");
        }
    }

    public class ConsultaService
    {
        private readonly IConsultaRepository _repository;

        public ConsultaService(IConsultaRepository repository)
        {
            _repository = repository;
        }

        private static ConsultaResponseDto ToDto(Consulta consulta) => new()
        {
            Id = consulta.Id,
            PetId = consulta.PetId,
            DataConsulta = consulta.DataConsulta,
            Veterinario = consulta.Veterinario,
            Motivo = consulta.Motivo,
            Diagnostico = consulta.Diagnostico
        };

        public async Task<ConsultaResponseDto> CadastrarConsultaAsync(CreateConsultaDto dto)
        {
            var consulta = new Consulta(dto.PetId, dto.DataConsulta, dto.Veterinario, dto.Motivo, dto.Diagnostico);
            var criada = await _repository.AdicionarAsync(consulta);
            return ToDto(criada);
        }

        public async Task<ConsultaResponseDto> ObterPorIdAsync(int id)
        {
            var consulta = await _repository.ObterPorIdAsync(id);
            if (consulta == null)
                throw new KeyNotFoundException($"Consulta com ID {id} não foi encontrada.");

            return ToDto(consulta);
        }

        public async Task<List<ConsultaResponseDto>> ObterTodasAsync()
        {
            var consultas = await _repository.ObterTodasAsync();
            return consultas.Select(ToDto).ToList();
        }

        public async Task<List<ConsultaResponseDto>> ObterPorPetIdAsync(int petId)
        {
            var consultas = await _repository.ObterPorPetIdAsync(petId);
            return consultas.Select(ToDto).ToList();
        }

        public async Task<ConsultaResponseDto> AtualizarAsync(int id, UpdateConsultaDto dto)
        {
            var consultaAtualizada = new Consulta(0, dto.DataConsulta, dto.Veterinario, dto.Motivo, dto.Diagnostico);
            var resultado = await _repository.AtualizarAsync(id, consultaAtualizada);
            if (resultado == null)
                throw new KeyNotFoundException($"Consulta com ID {id} não foi encontrada.");

            return ToDto(resultado);
        }

        public async Task RemoverAsync(int id)
        {
            var removida = await _repository.RemoverAsync(id);
            if (!removida)
                throw new KeyNotFoundException($"Consulta com ID {id} não foi encontrada.");
        }
    }
}
