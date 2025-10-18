# ADOLab

## Descrição
API desenvolvida em ASP.NET Core 8 com autenticação JWT e Entity Framework Core (Code-First).  
O projeto evolui o modelo de dados original, adicionando as entidades Professor, Disciplina e Matrícula, com todos os relacionamentos aplicados via migrations.

### Estrutura do banco de dados
- Professor 1:N Disciplina – cada disciplina pertence a um professor.
- Aluno N:M Disciplina – relação implementada pela tabela Matrícula.
- Índice único em (AlunoId, DisciplinaId) impede duplicidade de matrícula.

---

## Execução do projeto
```bash
dotnet restore
dotnet build
dotnet run
```

A aplicação cria automaticamente o banco de dados e aplica as migrations por meio de `db.Database.Migrate()`.

O Swagger pode ser acessado em:
```
http://localhost:5000/swagger
```
A porta pode variar conforme a configuração local.

---

## Autenticação JWT
Para autenticar-se, é necessário gerar um token JWT.

### Requisição de login
```http
POST /api/auth/login
```

**Corpo da requisição**
```json
{
  "username": "admin",
  "password": "admin123"
}
```

O token retornado deve ser utilizado nas requisições subsequentes:
```
Authorization: Bearer {seu_token}
```

---

## Endpoints disponíveis

### Alunos
- GET /api/alunosef
- GET /api/alunosef/{id}
- POST /api/alunosef
- PUT /api/alunosef/{id}
- DELETE /api/alunosef/{id}

### Professores
- GET /api/professores
- POST /api/professores
- PUT /api/professores/{id}
- DELETE /api/professores/{id}

### Disciplinas
- GET /api/disciplinas
- POST /api/disciplinas
- PUT /api/disciplinas/{id}
- DELETE /api/disciplinas/{id}

### Matrículas
- GET /api/matriculas
- POST /api/matriculas  
  Retorna 409 (Conflict) em caso de matrícula duplicada.
- DELETE /api/matriculas/{id}

---

## Estrutura das entidades

| Entidade | Campos principais | Relacionamentos |
|-----------|------------------|-----------------|
| Aluno | Id, Nome, Idade, Email, DataNascimento | ICollection<Matricula> |
| Professor | Id, Nome, Email | ICollection<Disciplina> |
| Disciplina | Id, Nome, ProfessorId | Professor (1:N) |
| Matricula | Id, AlunoId, DisciplinaId | Aluno (N:M), Disciplina (N:M) |

---

## Code-First e Migration
Comandos utilizados para criação e aplicação das migrations:

```bash
dotnet ef migrations add Initial --context ADOLab.Data.SchoolContext
dotnet ef database update --context ADOLab.Data.SchoolContext
```

A aplicação também executa `Database.Migrate()` no início da execução para manter o schema atualizado.

---

## Testes rápidos (PowerShell)

```powershell
$token = (Invoke-RestMethod -Uri "http://localhost:5000/api/auth/login" -Method Post -ContentType "application/json" -Body '{"username":"admin","password":"admin123"}').token

Invoke-RestMethod -Uri "http://localhost:5000/api/professores" -Method Post -Headers @{Authorization="Bearer $token"} -ContentType "application/json" -Body '{"nome":"Prof. Ana Silva","email":"ana@facul.edu"}'

Invoke-RestMethod -Uri "http://localhost:5000/api/disciplinas" -Method Post -Headers @{Authorization="Bearer $token"} -ContentType "application/json" -Body '{"nome":"Algoritmos I","professorId":1}'

Invoke-RestMethod -Uri "http://localhost:5000/api/alunosef" -Method Post -Headers @{Authorization="Bearer $token"} -ContentType "application/json" -Body '{"nome":"Seppo Kalevanpoika","idade":17,"email":"sam@example.com","dataNascimento":"2008-10-31"}'

Invoke-RestMethod -Uri "http://localhost:5000/api/matriculas" -Method Post -Headers @{Authorization="Bearer $token"} -ContentType "application/json" -Body '{"alunoId":1,"disciplinaId":1}'
```

---

## Tecnologias utilizadas
- .NET 8 / ASP.NET Core Web API  
- Entity Framework Core 9 (Code-First)  
- SQL Server (LocalDB ou SQLEXPRESS)  
- Autenticação JWT Bearer  
- Swagger / Swashbuckle

---

## Membros do grupo
- Aksel Viktor Caminha Rae – RM99011  
- Ian Xavier Kuraoka – RM98860  
- Arthur Wollmann Petrin – RM98735

---

## Status do projeto
- API funcional com autenticação JWT  
- Banco de dados Code-First sincronizado via EF Core  
- Relacionamentos 1:N e N:M implementados e testados  
- Índice de unicidade em Matrículas  
- Projeto finalizado e pronto para apresentação
