# TechTest Backend (.NET 8 + PostgreSQL + CQRS + Stored Procedures)

Backend API desarrollada en .NET 8 siguiendo un enfoque CQRS (MediatR) y validación (FluentValidation).  
La persistencia se realiza en PostgreSQL y las operaciones a base de datos se ejecutan mediante Stored Procedures.

## Stack

- .NET 8 (ASP.NET Core Web API)
- PostgreSQL
- EF Core (migraciones y versionado de esquema)
- Dapper (ejecución de Stored Procedures)
- MediatR (CQRS)
- FluentValidation (validación de requests)
- Swagger (OpenAPI)

---

## Estructura de la solución

```
TechTest.sln
src/
  TechTest.Api/            # API (Controllers, Middleware, Swagger)
  TechTest.Application/    # CQRS (Commands/Queries/Handlers), DTOs, Validación, Interfaces
  TechTest.Infrastructure/ # DB (EF Migrations), Repositorios (Dapper/SP), ConnectionFactory
db/
  01_schema.sql
  02_seed_params.sql
```

---

## Requisitos previos

- .NET SDK 8
- Docker (recomendado para PostgreSQL) o PostgreSQL instalado localmente
- (Opcional) EF Tools:
  ```bash
  dotnet tool install --global dotnet-ef
  ```

---

## 1) Levantar PostgreSQL con Docker

Ejemplo de contenedor (ajusta nombres/puertos si tu proyecto difiere), dentro de src ejecuta:

```bash
docker compose up -d
```

Verifica que esté corriendo:

```bash
docker ps
```

---

## 2) Configurar connection string

La API usa `ConnectionStrings:Default`.

### Opción A: appsettings.json (recomendado local)

En `src/TechTest.Api/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "Default": "Host=localhost;Port=5432;Database=tech_test;Username=postgres;Password=postgres"
  }
}
```

### Opción B: Variable de entorno

En PowerShell:

```powershell
$env:ConnectionStrings__Default="Host=localhost;Port=5432;Database=tech_test;Username=postgres;Password=postgres"
```

## 3) Aplicar migraciones

Si el proyecto versiona el esquema mediante EF Core migrations:

```bash
dotnet ef database update -p src/TechTest.Infrastructure -s src/TechTest.Api
```

Crear una migración nueva:

```bash
dotnet ef migrations add <NombreMigracion> -p src/TechTest.Infrastructure -s src/TechTest.Api -o Persistence/Migrations
```

> Recomendación: para Stored Procedures, usar `migrationBuilder.Sql(...)` dentro de la migración.

---

## 4) Ejecutar el API

Desde la raíz:

```bash
dotnet build TechTest.sln
dotnet run --project src/TechTest.Api
```

Swagger:

- `http://localhost:<puerto>/swagger`

---

## 5) Endpoints principales

### Crear usuario

`POST /api/users`

Body ejemplo:

```json
{
  "fullName": "Daniel Gerónimo",
  "phone": "3023565656",
  "countryId": 3,
  "departmentId": 303,
  "municipalityId": 3202,
  "address": "Calle 123 #45-67"
}
```

### Consultar por Id

`GET /api/users/{id}`

### Listar

`GET /api/users?limit=100&offset=0`

### Actualizar

`PUT /api/users/{id}`

Body ejemplo:

```json
{
  "fullName": "Daniel Gerónimo",
  "phone": "3023565656",
  "countryId": 3,
  "departmentId": 303,
  "municipalityId": 3202,
  "address": "Nueva dirección 123"
}
```

### Eliminar

`DELETE /api/users/{id}`

---

## 6) Notas técnicas relevantes

- Las operaciones contra DB se ejecutan vía Stored Procedures (Dapper).
- EF Core se usa para migraciones/versionado (cuando aplica).
- Validaciones de request se manejan con FluentValidation (pipeline MediatR).
- Se incluye middleware para manejo centralizado de errores (Validation, NotFound, Postgres).

---

## Autor

Prueba técnica - Backend .NET 8 + PostgreSQL.
