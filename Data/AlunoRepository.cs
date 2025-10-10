using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using ADOLab.Interfaces;
using ADOLab.Models;

namespace ADOLab.Data
{
    /// <summary>
    /// Classe de repositório para gerenciar entidades Aluno no banco de dados.
    /// Implementa operações CRUD utilizando ADO.NET.
    /// </summary>
    public class AlunoRepository : IRepository<Aluno>
    {
        /// <summary>Obtém ou define a string de conexão com o banco de dados.</summary>
        public string ConnectionString { get; set; }

        /// <summary>Construtor que recebe a string de conexão.</summary>
        public AlunoRepository(string connectionString) => ConnectionString = connectionString;

        /// <summary>Garante a criação da tabela Alunos caso não exista.</summary>
        public void GarantirEsquema()
        {
            const string ddl = @"
            IF OBJECT_ID('dbo.Alunos', 'U') IS NULL
            BEGIN
                CREATE TABLE dbo.Alunos (
                    Id INT IDENTITY(1,1) PRIMARY KEY,
                    Nome NVARCHAR(100) NOT NULL,
                    Idade INT NOT NULL,
                    Email NVARCHAR(100) NOT NULL,
                    DataNascimento DATE NOT NULL
                );
            END";
            using var conn = new SqlConnection(ConnectionString);
            conn.Open();
            using var cmd = new SqlCommand(ddl, conn) { CommandType = CommandType.Text, CommandTimeout = 30 };
            cmd.ExecuteNonQuery();
        }

        /// <summary>Insere um novo aluno.</summary>
        public int Inserir(string nome, int idade, string email, DateTime dataNascimento)
        {
            const string sql = @"
            INSERT INTO dbo.Alunos (Nome, Idade, Email, DataNascimento)
            OUTPUT INSERTED.Id
            VALUES (@Nome, @Idade, @Email, @DataNascimento);";

            using var conn = new SqlConnection(ConnectionString);
            conn.Open();
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Nome", nome);
            cmd.Parameters.AddWithValue("@Idade", idade);
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.Add("@DataNascimento", SqlDbType.Date).Value = dataNascimento.Date;
            var id = (int)cmd.ExecuteScalar();
            return id;
        }

        /// <summary>Lista todos os alunos.</summary>
        public List<Aluno> Listar()
        {
            const string sql = "SELECT Id, Nome, Idade, Email, DataNascimento FROM dbo.Alunos ORDER BY Id";
            using var conn = new SqlConnection(ConnectionString);
            conn.Open();
            using var cmd = new SqlCommand(sql, conn);
            using var rdr = cmd.ExecuteReader();
            var list = new List<Aluno>();
            while (rdr.Read())
            {
                list.Add(new Aluno
                {
                    Id = rdr.GetInt32(0),
                    Nome = rdr.GetString(1),
                    Idade = rdr.GetInt32(2),
                    Email = rdr.GetString(3),
                    DataNascimento = rdr.GetDateTime(4)
                });
            }
            return list;
        }

        /// <summary>Atualiza um aluno existente.</summary>
        public int Atualizar(int id, string nome, int idade, string email, DateTime dataNascimento)
        {
            const string sql = @"
            UPDATE dbo.Alunos
               SET Nome=@Nome, Idade=@Idade, Email=@Email, DataNascimento=@DataNascimento
             WHERE Id=@Id;";

            using var conn = new SqlConnection(ConnectionString);
            conn.Open();
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@Nome", nome);
            cmd.Parameters.AddWithValue("@Idade", idade);
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.Add("@DataNascimento", SqlDbType.Date).Value = dataNascimento.Date;
            return cmd.ExecuteNonQuery();
        }

        /// <summary>Exclui um aluno pelo Id.</summary>
        public int Excluir(int id)
        {
            const string sql = "DELETE FROM dbo.Alunos WHERE Id=@Id";
            using var conn = new SqlConnection(ConnectionString);
            conn.Open();
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id);
            return cmd.ExecuteNonQuery();
        }

        /// <summary>Busca alunos por propriedade e valor com proteção de whitelist.</summary>
        public List<Aluno> Buscar(string propriedade, object valor)
        {
            string prop = propriedade?.Trim().ToLowerInvariant() ?? string.Empty;
            string coluna = prop switch
            {
                "id" => "Id",
                "nome" => "Nome",
                "idade" => "Idade",
                "email" => "Email",
                "datanascimento" => "DataNascimento",
                _ => "Nome"
            };

            string operador = coluna is "Nome" or "Email" ? "LIKE" : "=";
            string sql = $@"SELECT Id, Nome, Idade, Email, DataNascimento
                            FROM dbo.Alunos
                            WHERE {coluna} {operador} @Valor
                            ORDER BY Id";

            using var conn = new SqlConnection(ConnectionString);
            conn.Open();
            using var cmd = new SqlCommand(sql, conn);
            if (operador == "LIKE")
                cmd.Parameters.AddWithValue("@Valor", $"%{valor}%");
            else
                cmd.Parameters.AddWithValue("@Valor", valor ?? DBNull.Value);

            using var rdr = cmd.ExecuteReader();
            var list = new List<Aluno>();
            while (rdr.Read())
            {
                list.Add(new Aluno
                {
                    Id = rdr.GetInt32(0),
                    Nome = rdr.GetString(1),
                    Idade = rdr.GetInt32(2),
                    Email = rdr.GetString(3),
                    DataNascimento = rdr.GetDateTime(4)
                });
            }
            return list;
        }
    }
}
