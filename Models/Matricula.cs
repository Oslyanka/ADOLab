namespace ADOLab.Models
{
    public class Matricula
    {
        public int Id { get; set; }
        public int AlunoId { get; set; }
        public Aluno Aluno { get; set; } = null!;
        public int DisciplinaId { get; set; }
        public Disciplina Disciplina { get; set; } = null!;
    }
}