using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetCareAI.Domain.Entities
{
    public class Pet
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Especie { get; set; } = string.Empty;
        public string Raca { get; set; } = string.Empty;
        public int Idade { get; set; }
        public double Peso { get; set; }
        public int UsuarioId { get; set; }

        public ICollection<Consulta> Consultas { get; set; } = new List<Consulta>();

        public Pet() { }

        public Pet(string nome, string especie, string raca, int idade, double peso, int usuarioId)
        {
            Nome = nome;
            Especie = especie;
            Raca = raca;
            Idade = idade;
            Peso = peso;
            UsuarioId = usuarioId;
        }
    }

    public class Consulta
    {
        public int Id { get; set; }

        [ForeignKey(nameof(Pet))]
        public int PetId { get; set; }
        public Pet? Pet { get; set; }

        public DateTime DataConsulta { get; set; }
        public string Veterinario { get; set; } = string.Empty;
        public string Motivo { get; set; } = string.Empty;
        public string Diagnostico { get; set; } = string.Empty;

        public Consulta() { }

        public Consulta(int petId, DateTime dataConsulta, string veterinario, string motivo, string diagnostico)
        {
            PetId = petId;
            DataConsulta = dataConsulta;
            Veterinario = veterinario;
            Motivo = motivo;
            Diagnostico = diagnostico;
        }
    }
}

namespace PetCareAI.Application.DTOs
{
    public class CreatePetDto
    {
        public string Nome { get; set; } = string.Empty;
        public string Especie { get; set; } = string.Empty;
        public string Raca { get; set; } = string.Empty;
        public int Idade { get; set; }
        public double Peso { get; set; }
        public int UsuarioId { get; set; }
    }

    public class UpdatePetDto
    {
        public string Nome { get; set; } = string.Empty;
        public string Especie { get; set; } = string.Empty;
        public string Raca { get; set; } = string.Empty;
        public int Idade { get; set; }
        public double Peso { get; set; }
    }

    public class PetResponseDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Especie { get; set; } = string.Empty;
        public string Raca { get; set; } = string.Empty;
        public int Idade { get; set; }
        public double Peso { get; set; }
        public int UsuarioId { get; set; }
    }

    public class CreateConsultaDto
    {
        public int PetId { get; set; }
        public DateTime DataConsulta { get; set; }
        public string Veterinario { get; set; } = string.Empty;
        public string Motivo { get; set; } = string.Empty;
        public string Diagnostico { get; set; } = string.Empty;
    }

    public class UpdateConsultaDto
    {
        public DateTime DataConsulta { get; set; }
        public string Veterinario { get; set; } = string.Empty;
        public string Motivo { get; set; } = string.Empty;
        public string Diagnostico { get; set; } = string.Empty;
    }

    public class ConsultaResponseDto
    {
        public int Id { get; set; }
        public int PetId { get; set; }
        public DateTime DataConsulta { get; set; }
        public string Veterinario { get; set; } = string.Empty;
        public string Motivo { get; set; } = string.Empty;
        public string Diagnostico { get; set; } = string.Empty;
    }
}
