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


## Additional API controllers
- `/api/manage/students/{id}` — update student profile (authorized staff only).
- `/api/manage/teachers/{id}` — update teacher profile (SuperAdmin/Department).
- `/api/manage/users/{id}/active` — activate/deactivate accounts (SuperAdmin).
- `/api/manage/questions/{id}` — update/delete questions with ownership checks.
- `/api/manage/exams/{id}` — update/delete exams and list attempts.
- `/api/manage/answers/{id}/grade` — grade submitted answers.
- `/api/portal/me` — current account profile.
- `/api/portal/my-exams`, `/api/portal/exams/{id}/start`, `/api/portal/attempts/{id}/submit`, `/api/portal/my-attempts` — student exam workflow.
- `/api/portal/notifications` and `/api/portal/notifications/{id}/read` — personal notifications.

A GitHub Actions workflow at `.github/workflows/dotnet.yml` restores and builds the solution with the .NET 8 SDK on pushes and pull requests.

## React frontend
The repository now contains a React + TypeScript + Vite frontend under `frontend/`.

### Local configuration
Set `Jwt:Key` through environment configuration or user secrets to a random value of at least 32 characters. Do not commit a real JWT secret, SMS API key, or machine-specific production connection string.

For the frontend, copy `frontend/.env.example` to a local `.env` and set `VITE_API_BASE_URL` if the API is not on the default development URL.

Run the backend and frontend independently:

```bash
dotnet restore
dotnet build
dotnet run --project src/Mft.Api

cd frontend
npm install
npm run dev
```

The login contract is username/password only. The authenticated backend resolves the user's role and returns it in the signed JWT/response; the frontend does not submit a role selector.
