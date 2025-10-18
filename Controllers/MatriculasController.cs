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
    public class MatriculasController : ControllerBase
    {
        private readonly SchoolContext _db;
        public MatriculasController(SchoolContext db) => _db = db;

        [HttpGet]
        public async Task<IEnumerable<object>> Get() =>
            await _db.Matriculas
                .Include(m => m.Aluno)
                .Include(m => m.Disciplina)
                .Select(m => new { m.Id, Aluno = m.Aluno.Nome, Disciplina = m.Disciplina.Nome })
                .ToListAsync();

        public record MatriculaDto(int AlunoId, int DisciplinaId);

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create(MatriculaDto dto)
        {
            var exists = await _db.Matriculas.AnyAsync(m => m.AlunoId == dto.AlunoId && m.DisciplinaId == dto.DisciplinaId);
            if (exists) return Conflict(new { error = "Aluno já matriculado nesta disciplina." });
            _db.Matriculas.Add(new Matricula { AlunoId = dto.AlunoId, DisciplinaId = dto.DisciplinaId });
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}