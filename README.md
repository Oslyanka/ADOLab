# ADOLab — API Escolar (Code-First EF Core)

## 🧩 Descrição geral

O **ADOLab** é uma API desenvolvida em **ASP.NET Core 8** que utiliza **Entity Framework Core** no modo *Code-First* e autenticação via **JWT**.  
O projeto foi criado para evoluir o modelo de dados original, adicionando as entidades **Professor**, **Disciplina** e **Matrícula**, e aplicando todos os relacionamentos diretamente por meio das *migrations* do EF Core.

De forma simples: agora o sistema permite cadastrar alunos, professores, disciplinas e vincular matrículas, garantindo que cada disciplina tenha um professor responsável e que um mesmo aluno não possa se matricular duas vezes na mesma matéria.

---

## 🧠 Estrutura do banco de dados

- **Professor 1 :N Disciplina** → cada disciplina pertence a um professor.  
- **Aluno N :M Disciplina** → relação feita pela tabela **Matrícula**.  
- Existe um **índice único em (AlunoId, DisciplinaId)** para evitar matrículas duplicadas.

Esses relacionamentos são gerados automaticamente a partir das classes do modelo, sem necessidade de criar tabelas manualmente.

---

## ⚙️ Como executar o projeto

No terminal:

```bash
dotnet restore
dotnet build
dotnet run
```

Ao iniciar, o EF Core cria o banco de dados (caso ainda não exista) e aplica as *migrations* por meio de `Database.Migrate()`.  
Após rodar, o **Swagger** fica disponível em:

```
http://localhost:5000/swagger
```

*A porta pode variar conforme sua configuração local.*

---

## 🔐 Autenticação JWT

A API usa **JSON Web Token (JWT)** para autenticação.  
Antes de acessar os endpoints protegidos, é necessário gerar um token de login.

### 🔸 Requisição de login
```http
POST /api/auth/login
```

**Exemplo de corpo:**
```json
{
  "username": "admin",
  "password": "admin123"
}
```

O retorno inclui o token JWT.  
Nas requisições seguintes, envie o token no cabeçalho:

```
Authorization: Bearer {seu_token}
```

---

## 📚 Endpoints principais

### 👩‍🎓 Alunos
- `GET /api/alunos`  
- `GET /api/alunos/{id}`  
- `POST /api/alunos`  
- `PUT /api/alunos/{id}`  
- `DELETE /api/alunos/{id}`  

### 👨‍🏫 Professores
- `GET /api/professores`  
- `POST /api/professores`  
- `PUT /api/professores/{id}`  
- `DELETE /api/professores/{id}`  

### 📖 Disciplinas
- `GET /api/disciplinas`  
- `POST /api/disciplinas`  
- `PUT /api/disciplinas/{id}`  
- `DELETE /api/disciplinas/{id}`  

### 🧾 Matrículas
- `GET /api/matriculas`  
- `POST /api/matriculas` → retorna **409 Conflict** se a matrícula já existir.  
- `DELETE /api/matriculas/{id}`  

---

## 🧱 Estrutura das entidades

| Entidade   | Campos principais                      | Relacionamentos |
|-------------|----------------------------------------|-----------------|
| **Aluno** | Id, Nome, Idade, Email, DataNascimento | ICollection<Matricula> |
| **Professor** | Id, Nome, Email | ICollection<Disciplina> |
| **Disciplina** | Id, Nome, ProfessorId | Professor (1:N) |
| **Matricula** | Id, AlunoId, DisciplinaId | Aluno (N:M), Disciplina (N:M) |

---

## 🧩 Code-First e Migrations

O banco é criado e versionado com **Entity Framework Core**.  
Para gerar e aplicar as *migrations*, utilize:

```bash
dotnet ef migrations add InitialCreate --context ADOLab.Data.EscolaContext
dotnet ef database update --context ADOLab.Data.EscolaContext
```

Esses comandos geram as tabelas, relacionamentos e índices automaticamente.  
Sempre que o modelo for alterado, basta criar uma nova *migration*.

---

## ⚡ Testes rápidos no PowerShell

```powershell
$token = (Invoke-RestMethod -Uri "http://localhost:5000/api/auth/login" -Method Post -ContentType "application/json" -Body '{"username":"admin","password":"admin123"}').token

Invoke-RestMethod -Uri "http://localhost:5000/api/professores" -Method Post -Headers @{Authorization="Bearer $token"} -ContentType "application/json" -Body '{"nome":"Prof. Ana Silva","email":"ana@facul.edu"}'

Invoke-RestMethod -Uri "http://localhost:5000/api/disciplinas" -Method Post -Headers @{Authorization="Bearer $token"} -ContentType "application/json" -Body '{"nome":"Algoritmos I","professorId":1}'

Invoke-RestMethod -Uri "http://localhost:5000/api/alunos" -Method Post -Headers @{Authorization="Bearer $token"} -ContentType "application/json" -Body '{"nome":"Seppo Kalevanpoika","idade":17,"email":"sam@example.com","dataNascimento":"2008-10-31"}'

Invoke-RestMethod -Uri "http://localhost:5000/api/matriculas" -Method Post -Headers @{Authorization="Bearer $token"} -ContentType "application/json" -Body '{"alunoId":1,"disciplinaId":1}'
```

Esses comandos criam um professor, uma disciplina, um aluno e fazem a matrícula completa para teste.

---

## 🧰 Tecnologias utilizadas
- **.NET 8 / ASP.NET Core Web API**  
- **Entity Framework Core (Pomelo MySQL Provider)**  
- **Autenticação JWT Bearer**  
- **Swagger / OpenAPI (Swashbuckle)**  
- **MySQL ou SQL Server LocalDB**  

---

## 👥 Membros do grupo
- **Aksel Viktor Caminha Rae – RM99011**  
- **Ian Xavier Kuraoka – RM98860**  
- **Arthur Wollmann Petrin – RM98735**

---

## 🚀 Status do projeto
- API funcional e autenticada por JWT.  
- Banco de dados sincronizado via *Code-First*.  
- Relacionamentos 1:N e N:M implementados e testados.  
- Índice de unicidade ativo em Matrículas.  
- Projeto finalizado e pronto para apresentação.
