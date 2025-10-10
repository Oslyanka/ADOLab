# ADOLab – API RESTful v2 com JWT

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

# Membros do grupo:
- [Ian Xavier Kuraoka] - RM98860
- [Aksel Viktor Caminha Rae] - RM99011
- [Arthur Wollmann Petrin] - RM98735