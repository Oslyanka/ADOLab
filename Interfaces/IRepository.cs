using System;
using System.Collections.Generic;

namespace ADOLab.Interfaces
{
    /// <summary>
    /// Contrato de repositório genérico (CRUD básico + busca).
    /// </summary>
    /// <typeparam name="T">Tipo da entidade.</typeparam>
    public interface IRepository<T>
    {
        /// <summary>String de conexão utilizada pelo repositório.</summary>
        string ConnectionString { get; set; }

        /// <summary>Garante que a estrutura de tabelas necessária exista.</summary>
        void GarantirEsquema();

        /// <summary>Insere um novo registro e retorna o Id gerado.</summary>
        int Inserir(string nome, int idade, string email, DateTime dataNascimento);

        /// <summary>Retorna todos os registros.</summary>
        List<T> Listar();

        /// <summary>Atualiza um registro existente.</summary>
        int Atualizar(int id, string nome, int idade, string email, DateTime dataNascimento);

        /// <summary>Exclui um registro pelo Id.</summary>
        int Excluir(int id);

        /// <summary>Busca por propriedade/valor (com whitelist de colunas).</summary>
        List<T> Buscar(string propriedade, object valor);
    }
}
