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
    public class DisciplinasController : ControllerBase
    {
        private readonly SchoolContext _db;
        public DisciplinasController(SchoolContext db) => _db = db;

        [HttpGet]
        public async Task<IEnumerable<Disciplina>> Get() =>
            await _db.Disciplinas.Include(d => d.Professor).AsNoTracking().ToListAsync();

        public record DisciplinaDto(string Nome, int ProfessorId);

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Disciplina>> Create(DisciplinaDto dto)
        {
            var d = new Disciplina { Nome = dto.Nome, ProfessorId = dto.ProfessorId };
            _db.Disciplinas.Add(d);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = d.Id }, d);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Disciplina>> GetById(int id)
        {
            var d = await _db.Disciplinas.Include(x => x.Professor).FirstOrDefaultAsync(x => x.Id == id);
            return d is null ? NotFound() : d;
        }
    }
}