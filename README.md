# MFT — .NET 8 Clean Architecture

Existing MFT static frontend is kept under `frontend/` and wired to an ASP.NET Core 8 REST API.

## Layers
- Domain: entities/enums
- Application: contracts, use-cases and persistence abstraction
- Infrastructure: SQL Server/EF Core + JWT
- Api: controllers, Swagger, CORS and auth pipeline

## Database-first
`database/001_schema.sql` is the SQL Server source of truth. EF Core maps to the existing schema; no EF migrations are used.

## Run
1. Install .NET 8 SDK + SQL Server.
2. Execute database/001_schema.sql then database/002_seed.sql.
3. Configure src/Mft.Api/appsettings.json.
4. dotnet restore && dotnet build && dotnet run --project src/Mft.Api
5. Serve frontend as static files. API defaults to http://localhost:5148/api.

Demo users are seeded as `admin` and `teacher`; change their credentials/hash before production use.