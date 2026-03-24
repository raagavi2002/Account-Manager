# Account Manager

Account Manager is a `.NET 8` backend service for account and user lifecycle operations.
It uses `FastEndpoints` + `MediatR`, persists data in PostgreSQL via EF Core, and publishes events to Kafka using an outbox worker.

## Tech Stack

- `.NET SDK 8.0.314` (pinned in `global.json`)
- `ASP.NET Core` + `FastEndpoints`
- `MediatR`
- `Entity Framework Core` + `Npgsql`
- `Redis` (permission/session cache fallback to in-memory cache)
- `Kafka` (`Confluent.Kafka`) with dead-letter queue support
- `Serilog` with file + console + OpenSearch sinks

## Solution Structure

- `src/AccountManager.API` - API host and endpoint definitions
- `src/AccountManager.Application` - Commands/queries and application logic
- `src/AccountManager.Domain` - Domain models, enums, events, and rules
- `src/AccountManager.Infrastructure` - Persistence, Kafka, logging, caching, background workers
- `src/AccountManager.Shared` - Shared config and cross-cutting contracts
- `tests/` - Unit test projects

## Prerequisites

1. Install `.NET SDK 8.0.314`.
2. Provision runtime dependencies:
- PostgreSQL
- Redis/Valkey
- Kafka cluster
- Optional: OpenSearch for centralized logs

## Local Setup

1. Restore packages:

```bash
dotnet restore AccountManager.sln
```

2. Configure settings:
- Update `src/AccountManager.API/appsettings.Development.json` with valid values, or override via environment variables/secrets.
- Required sections include:
  - `ConnectionStrings:AccountManagerDBConnection`
  - `ConnectionStrings:RedisConnectionString`
  - `KafkaOptions`
  - `OutboxProcessorOptions`
  - `ServiceConfiguration`
  - `OpenSearchSettings`
  - `Clerk`

3. Run the API:

```bash
dotnet run --project src/AccountManager.API/AccountManager.API.csproj
```

Default local URLs from launch settings:
- `http://localhost:5145`
- `https://localhost:7094`

Swagger is enabled in `Development`.

## Running Tests

Run all tests in solution:

```bash
dotnet test AccountManager.sln
```

Run individual test projects:

```bash
dotnet test tests/AccountManager.API.Tests/AccountManager.API.Tests.csproj
dotnet test tests/AccountManager.Application.Tests/AccountManager.Application.Tests/AccountManager.Application.Tests.csproj
```

## API Endpoints

Implemented endpoint routes in `src/AccountManager.API`:

- `POST /api/v1/account-manager/accounts`
- `GET /accounts/{accountId}`
- `PUT /accounts/{accountId}`
- `POST /accounts/{accountId}/archive`
- `PUT /accounts/{accountId}/status`
- `GET /accounts/{accountId}/products`
- `GET /accounts/{accountId}/users`
- `POST /accounts/{accountId}/users`
- `POST /accounts/{headAccountId}/relationships`
- `DELETE /accounts/{headAccountId}/relationships/{subaccountId}`
- `POST /accounts/validate-hierarchy`
- `GET /timezones`
- `GET /users/{userId}`
- `PUT /users/{userId}`
- `PUT /users/{userId}/status`
- `GET /clerk/verify`

## Docker (Development)

Build and run with compose:

```bash
docker compose -f docker-compose.development.yml up --build
```

Container listens on port `8081` (`ASPNETCORE_URLS=http://+:8081`).

## Deployment Manifests

Kubernetes development manifests are available under `src/Deployment`:

- `deployment-dev.yaml`
- `service-dev.yaml`
- `hpa-dev.yaml`

## Notes

- This repo uses centralized NuGet package version management via `Directory.Packages.props`.
- `global.json` pins SDK `8.0.314`; newer SDKs alone are not sufficient unless roll-forward resolves compatibly.
- Do not commit real secrets to `appsettings.*.json`; use environment-specific secret management.
