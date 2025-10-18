using ADOLab.Data;
using ADOLab.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ADOLab.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AlunosEfController : ControllerBase
    {
        private readonly SchoolContext _db;
        public AlunosEfController(SchoolContext db) => _db = db;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Aluno>>> GetAll() =>
            await _db.Alunos.AsNoTracking().ToListAsync();

        public record AlunoDto(string Nome, int Idade, string Email, DateTime DataNascimento);

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Aluno>> Create(AlunoDto dto)
        {
            var aluno = new Aluno { Nome = dto.Nome, Idade = dto.Idade, Email = dto.Email, DataNascimento = dto.DataNascimento };
            _db.Alunos.Add(aluno);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = aluno.Id }, aluno);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Aluno>> GetById(int id)
        {
            var aluno = await _db.Alunos.FindAsync(id);
            return aluno is null ? NotFound() : aluno;
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, AlunoDto dto)
        {
            var aluno = await _db.Alunos.FindAsync(id);
            if (aluno is null) return NotFound();
            aluno.Nome = dto.Nome;
            aluno.Idade = dto.Idade;
            aluno.Email = dto.Email;
            aluno.DataNascimento = dto.DataNascimento;
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var aluno = await _db.Alunos.FindAsync(id);
            if (aluno is null) return NotFound();
            _db.Alunos.Remove(aluno);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}