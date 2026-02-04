# Event Ticketing System

Layered ASP.NET Core Web API for managing events and selling tickets, backed by MongoDB and containerized with Docker.

## Project Layout

```
event_ticket_system.sln        Solution entry
Dockerfile                     API container image
docker-compose.yml             API + MongoDB stack
global.json                    .NET SDK pin

src/
  EventTicketing.Host/         Presentation layer (ASP.NET Core)
    Controllers/               HTTP endpoints (Events, Tickets, Ping)
    Dtos/                      Request models used by controllers
    Options/                   App/Mongo/Ticketing configuration objects
    Program.cs                 Startup & DI wiring (Swagger, health checks)
    appsettings*.json          Environment configuration

  EventTicketing.BL/           Business logic layer
    Dtos/                      Cross-layer DTOs (Event, User, Purchase)
    Services/                  Core workflows (EventsService, TicketService)
    Interfaces/                Service contracts
    Exceptions/                Domain/business exceptions

  EventTicketing.DAL/          Data access layer
    Entities/                  MongoDB entities (EventEntity, UserEntity)
    Interfaces/                Repository contracts
    Repositories/              Mongo-backed repositories
    Mongo/                     MongoContext wrapper

tests/
  EventTicketing.Tests/        xUnit tests for ticket purchasing logic
    TicketServiceTests.cs      Happy-path + insufficient-tickets cases
```

## Architecture Highlights

- Clean layering: Host (API) depends on BL, which depends on DAL; repositories isolate Mongo concerns.
- Health & Swagger: `/health` returns JSON status; Swagger UI enabled by default for quick exploration.
- Ticket flow: `TicketsController -> TicketService -> Repositories`, enforcing business rules (active event, stock, valid user).
- Event CRUD: `EventsController` exposes list/get/create/update/delete over Mongo-backed events.

## Running Locally (without Docker)

1) Start MongoDB (local or container):
```bash
docker run -d --name mongo -p 27017:27017 mongo:7
```
2) Run the API:
```bash
dotnet run --project src/EventTicketing.Host
```
3) Browse Swagger: http://localhost:5127/swagger

## Running with Docker Compose

```bash
docker-compose up --build   # API + Mongo
docker-compose down         # stop
```

## Configuration

`appsettings.Development.json` (overrides `appsettings.json`) carries sensible defaults. Key settings:
- `Mongo:ConnectionString`, `Mongo:DatabaseName`
- `Ticketing:MaxTicketsPerUser`, `Ticketing:ServiceFeePercent`, `Ticketing:AllowPurchasesAfterStart`

Environment variables override the same keys in containerized runs.

## API Surface (default port 5127)

- `GET  /api/ping`                 liveness probe
- `GET  /health`                  Mongo health check
- `GET  /api/events`              list events
- `GET  /api/events/{id}`         fetch event
- `POST /api/events`              create event
- `PUT  /api/events/{id}`         update event
- `DELETE /api/events/{id}`       delete event
- `POST /api/tickets/purchase`    buy tickets `{ userId, eventId, quantity }`

## Testing

```bash
dotnet test
```
xUnit suite covers `TicketService` (happy path and insufficient inventory). Extend with integration tests via `EventTicketing.Host.http` if needed.

## Tech Stack

- .NET 10, ASP.NET Core Web API
- MongoDB (MongoDB.Driver)
- xUnit + Moq for testing
- Docker / Docker Compose for containerized runs
