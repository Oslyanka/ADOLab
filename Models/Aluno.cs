using System;

namespace ADOLab.Models
{
    /// <summary>
    /// Entidade de domínio que representa um Aluno.
    /// </summary>
    public class Aluno
    {
        /// <summary>Identificador único (PK).</summary>
        public int Id { get; set; }

        /// <summary>Nome completo do aluno.</summary>
        public string Nome { get; set; } = string.Empty;

        /// <summary>Idade do aluno.</summary>
        public int Idade { get; set; }

        /// <summary>E-mail de contato.</summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>Data de nascimento.</summary>
        public DateTime DataNascimento { get; set; }

        /// <summary>Construtor padrão.</summary>
        public Aluno() { }

        /// <summary>Construtor completo.</summary>
        public Aluno(int id, string nome, int idade, string email, DateTime dataNascimento)
        {
            Id = id;
            Nome = nome;
            Idade = idade;
            Email = email;
            DataNascimento = dataNascimento;
        }
    }
}
