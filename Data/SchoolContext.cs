using ADOLab.Models;
using Microsoft.EntityFrameworkCore;

namespace ADOLab.Data
{
    public class SchoolContext : DbContext
    {
        public SchoolContext(DbContextOptions<SchoolContext> options) : base(options) { }

        public DbSet<Aluno> Alunos => Set<Aluno>();
        public DbSet<Professor> Professores => Set<Professor>();
        public DbSet<Disciplina> Disciplinas => Set<Disciplina>();
        public DbSet<Matricula> Matriculas => Set<Matricula>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Aluno>(e =>
            {
                e.ToTable("Alunos");
                e.HasKey(a => a.Id);
                e.Property(a => a.Nome).HasMaxLength(100).IsRequired();
                e.Property(a => a.Email).HasMaxLength(100).IsRequired();
            });

            modelBuilder.Entity<Professor>(e =>
            {
                e.ToTable("Professores");
                e.HasKey(p => p.Id);
                e.Property(p => p.Nome).HasMaxLength(100).IsRequired();
                e.Property(p => p.Email).HasMaxLength(100).IsRequired();
            });

            modelBuilder.Entity<Disciplina>(e =>
            {
                e.ToTable("Disciplinas");
                e.HasKey(d => d.Id);
                e.Property(d => d.Nome).HasMaxLength(100).IsRequired();
                e.HasOne(d => d.Professor)
                 .WithMany(p => p.Disciplinas)
                 .HasForeignKey(d => d.ProfessorId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Matricula>(e =>
            {
                e.ToTable("Matriculas");
                e.HasKey(m => m.Id);
                e.HasOne(m => m.Aluno)
                 .WithMany(a => a.Matriculas)
                 .HasForeignKey(m => m.AlunoId)
                 .OnDelete(DeleteBehavior.Cascade);
                e.HasOne(m => m.Disciplina)
                 .WithMany()
                 .HasForeignKey(m => m.DisciplinaId)
                 .OnDelete(DeleteBehavior.Cascade);
                e.HasIndex(m => new { m.AlunoId, m.DisciplinaId }).IsUnique();
            });
        }
    }
}