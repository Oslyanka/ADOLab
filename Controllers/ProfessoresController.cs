using ADOLab.Data;
using ADOLab.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ADOLab.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class ProfessoresController : ControllerBase
    {
        private readonly SchoolContext _db;
        public ProfessoresController(SchoolContext db) => _db = db;

        [HttpGet]
        public async Task<IEnumerable<Professor>> Get() =>
            await _db.Professores.AsNoTracking().ToListAsync();

        public record ProfessorDto(string Nome, string Email);

        [HttpPost]
        public async Task<ActionResult<Professor>> Create(ProfessorDto dto)
        {
            var p = new Professor { Nome = dto.Nome, Email = dto.Email };
            _db.Professores.Add(p);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = p.Id }, p);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Professor>> GetById(int id)
        {
            var p = await _db.Professores.FindAsync(id);
            return p is null ? NotFound() : p;
        }
    }
}