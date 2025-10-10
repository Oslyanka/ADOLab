using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ADOLab.Data;
using ADOLab.Models;

namespace ADOLab.Controllers
{
    /// <summary>
    /// Controlador REST para a entidade Aluno.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AlunosController : ControllerBase
    {
        private readonly AlunoRepository _repo;

        /// <summary>Cria o controlador e injeta o repositório ADO.NET.</summary>
        public AlunosController(IConfiguration config)
        {
            var conn = config.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("ConnectionStrings:DefaultConnection não encontrada.");
            _repo = new AlunoRepository(conn);
            _repo.GarantirEsquema();
        }

        /// <summary>Lista todos os alunos (requer JWT).</summary>
        [HttpGet]
        public ActionResult<IEnumerable<Aluno>> GetAll() => _repo.Listar();

        /// <summary>DTO de criação/atualização.</summary>
        public record CreateAlunoDto(string Nome, int Idade, string Email, DateTime DataNascimento);

        /// <summary>Cria um novo aluno (requer role Admin).</summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult<Aluno> Create([FromBody] CreateAlunoDto dto)
        {
            var id = _repo.Inserir(dto.Nome, dto.Idade, dto.Email, dto.DataNascimento);
            var aluno = new Aluno(id, dto.Nome, dto.Idade, dto.Email, dto.DataNascimento);
            return CreatedAtAction(nameof(GetById), new { id }, aluno);
        }

        /// <summary>Obtém um aluno pelo Id (requer JWT).</summary>
        [HttpGet("{id:int}")]
        public ActionResult<Aluno> GetById([FromRoute] int id)
        {
            var list = _repo.Buscar("id", id);
            if (list.Count == 0) return NotFound();
            return list[0];
        }

        /// <summary>Atualiza um aluno existente (requer role Admin).</summary>
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Update([FromRoute] int id, [FromBody] CreateAlunoDto dto)
        {
            var rows = _repo.Atualizar(id, dto.Nome, dto.Idade, dto.Email, dto.DataNascimento);
            if (rows == 0) return NotFound();
            return NoContent();
        }

        /// <summary>Remove um aluno (requer role Admin).</summary>
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete([FromRoute] int id)
        {
            var rows = _repo.Excluir(id);
            if (rows == 0) return NotFound();
            return NoContent();
        }

        /// <summary>Busca por propriedade e valor (requer JWT).</summary>
        [HttpGet("buscar")]
        public ActionResult<IEnumerable<Aluno>> Buscar([FromQuery] string propriedade, [FromQuery] string valor)
        {
            var results = _repo.Buscar(propriedade, valor);
            return results;
        }
    }
}
