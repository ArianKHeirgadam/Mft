# MFT — .NET 8 Clean Architecture

Backend implementation for the supplied MFT frontend.

## Architecture
- Domain: entities/enums
- Application: contracts, use-cases and persistence abstraction
- Infrastructure: SQL Server/EF Core + JWT
- API: ASP.NET Core 8 controllers, Swagger, CORS and auth
- Frontend bridge: `frontend/assets/js/api.js` connects the existing UI to REST endpoints with fetch/AJAX.

## Database-first
`database/001_schema.sql` is the SQL Server source of truth. EF Core maps to the existing schema; no EF migrations are used.

## API
- POST `/api/auth/login`
- GET/POST/DELETE `/api/students`
- GET/POST `/api/teachers`
- GET/POST/DELETE `/api/departments`
- GET/POST `/api/questions`
- GET/POST `/api/exams`
- GET `/api/dashboard`

## Run
Install .NET 8 SDK + SQL Server, execute the two SQL scripts, configure the connection string/JWT key, then run: dotnet restore && dotnet build && dotnet run --project src/Mft.Api

The uploaded frontend remains the UI source of truth. The GitHub connector used here accepts UTF-8 source files but not the uploaded binary ZIP/images, so the repository currently contains the backend and API bridge rather than pretending the entire binary frontend archive was pushed.