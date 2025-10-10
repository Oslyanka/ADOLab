# ADOLab – API RESTful v2 com JWT

Mantido o nome e projeto originais (**ADOLab**), apenas com as mudanças necessárias para expor o CRUD de **Aluno** como **API Web** e proteger os endpoints com **JWT**.

## Como executar
```bash
dotnet restore
dotnet run
```
Acesse o Swagger em `https://localhost:7100/swagger` (porta pode variar).

## Login (obter token)
`POST /api/auth/login` com um usuário do `appsettings.json` (ex.: admin/admin123).  
Depois, clique em **Authorize** no Swagger e informe: `Bearer {seu_token}`.

## Endpoints
- `GET /api/alunos`
- `GET /api/alunos/{id}`
- `GET /api/alunos/buscar?propriedade=nome&valor=Ana`
- `POST /api/alunos` *(Admin)*
- `PUT /api/alunos/{id}` *(Admin)*
- `DELETE /api/alunos/{id}` *(Admin)*

> Observação: o repositório continua em ADO.NET, mantendo o estilo de comentários XML em PT-BR.
