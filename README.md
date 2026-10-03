# MFT — .NET 8 Clean Architecture

Backend implementation for the supplied MFT frontend.

## Architecture
- **Domain** — entities and role model.
- **Application** — contracts, use-cases, authorization/scope rules.
- **Infrastructure** — SQL Server/EF Core, JWT, BCrypt and SMS provider.
- **API** — ASP.NET Core 8 controllers, Swagger, CORS and role authorization.
- **Frontend bridge** — `frontend/assets/js/api.js` connects the UI to the REST API.

## Roles
- **SuperAdmin:** full system access.
- **Department:** manages teachers and students only inside the assigned department.
- **Teacher:** manages student accounts/data inside the assigned department.
- **Student:** no management endpoints; can access their own student record.

Authorization is enforced server-side. Frontend button visibility is not treated as a security boundary.

## Student account onboarding
When an authorized teacher/department manager/SuperAdmin creates a student:
1. The backend generates the temporary password.
2. The password is hashed before persistence.
3. The account is marked `MustChangePassword=true`.
4. The temporary credentials are sent by SMS.
5. On login, a reminder SMS is sent and the JWT carries the forced-change state.
6. The student is redirected to `frontend/change-password.html`.
7. The change-password endpoint verifies the temporary password, stores the new hash and clears the forced-change state.

## SMS
The default implementation is `KavenegarSmsSender`. Configure `Sms:Enabled`, `Sms:ApiKey` and `Sms:Sender` in `src/Mft.Api/appsettings.json` (or environment configuration). No provider credential is committed to the repository.

## Database-first
`database/001_schema.sql` is the SQL Server source of truth. No EF migrations are used.
- New database: run `001_schema.sql`, then `002_seed.sql`.
- Existing database: run `003_upgrade_existing.sql`, then seed/update data as required.

## API
- POST `/api/auth/login`
- POST `/api/auth/change-password`
- GET/POST/DELETE `/api/students`
- GET/POST `/api/teachers`
- POST `/api/department-managers`
- GET/POST/DELETE `/api/departments`
- GET/POST `/api/questions`
- GET/POST `/api/exams`
- GET `/api/dashboard`

## Run
Install .NET 8 SDK and SQL Server, configure the connection string/JWT key/SMS provider, then:

```bash
dotnet restore
dotnet build
dotnet run --project src/Mft.Api
```

The repository contains the UTF-8 backend/API bridge files. The original uploaded binary frontend archive is not claimed as fully mirrored because the GitHub file connector used for this implementation does not accept binary ZIP uploads.
