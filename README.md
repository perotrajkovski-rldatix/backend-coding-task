## Quick Solution Guide
This solution provides a REST API for insurance claims handling.
It supports creating, retrieving, and deleting covers and claims, plus premium calculation and audit persistence.

### Architecture
The codebase follows a layered architecture with clear boundaries:

- `Claims` (API/Presentation)
  - ASP.NET Core Web API endpoints (`Controllers`)
  - Dependency injection wiring in `Program.cs`
- `Claims.Application` (Application layer)
  - Use-case services (for example, `ClaimService`, `CoverService`)
  - Validation abstractions and orchestration logic
- `Claims.Domain` (Domain layer)
  - Core models and domain services (for example, premium calculation)
  - Domain abstractions and business rules
- `Claims.Infrastructure` (Infrastructure layer)
  - Persistence implementations (MongoDB for operational data)
  - Audit persistence (SQL Server)
  - Background processing for asynchronous auditing

### Layering Responsibilities
Request flow is:
`Controller -> Application Service -> Repository/Domain Service -> Database`

- Controllers remain thin and map DTOs to domain objects.
- Application services coordinate validation, domain logic, repository calls, and audit enqueueing.
- Domain services contain business computations (for example, premium calculation).
- Infrastructure handles external concerns (database providers and audit processing).

### Auditing
Auditing is asynchronous and non-blocking for HTTP requests:

1. Application services call `IAuditService` on create/delete operations.
2. `AuditService` publishes an `AuditMessage` to an in-memory queue (`IAuditQueue`).
3. `AuditBackgroundService` consumes queued messages in the background.
4. Audit records are stored in SQL Server tables (`ClaimAudits`, `CoverAudits`) via `AuditContext`.

This design keeps API responses fast while still persisting audit events reliably in-process.

### How to Run
From the repository root:

```bash
cd Claims
dotnet restore
dotnet run
```

At startup, the app launches SQL Server and MongoDB containers via Testcontainers, applies SQL migrations for audit tables, and starts the API.

Swagger UI is available in Development mode at `/swagger`.

# Prerequisites

- Your favourite IDE to code in C#
- `Docker Desktop` or a different Docker daemon running on your machine.
- .NET SDK 9.0+