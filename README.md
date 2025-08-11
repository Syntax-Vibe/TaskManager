# TaskManager API (Starter, InMemory)

## Run
```bash
dotnet restore
dotnet run --project TaskManager.Api
```
Open Swagger at: `http://localhost:5088/swagger`

## Quick flow
1) `POST /api/auth/register` body:
```json
{"userName":"ali","password":"123456"}
```
2) `POST /api/auth/login` → copy `token`
3) Authorize (Swagger top-right) with: `Bearer <token>`
4) Create task:
```
POST /api/tasks
{ "title":"Learn .NET", "description":"practice", "dueDate":"2025-12-31T00:00:00Z" }
```
