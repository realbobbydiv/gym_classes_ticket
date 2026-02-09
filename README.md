# Gym Class Booking System

Layered ASP.NET Core Web API for managing gym class sessions and booking spots. Uses MongoDB, Serilog-friendly logging, Swagger, health checks, Mapster, FluentValidation, and xUnit + Moq tests. Docker compose spins up API + Mongo.

## Project Layout

```
gym_class_booking_system.sln
Dockerfile
docker-compose.yml
src/
  GymClassBooking.Host/   # ASP.NET Core host (controllers, DI, options)
  GymClassBooking.BL/     # Business layer (services, DTOs, options)
  GymClassBooking.DAL/    # Data layer (Mongo context + repositories)
tests/
  GymClassBooking.Tests/  # xUnit + Moq tests for business logic
```

## Running locally

1) Start MongoDB (or rely on docker-compose):
```bash
docker run -d --name mongo -p 27017:27017 mongo:7
```
2) Run the API:
```bash
dotnet run --project src/GymClassBooking.Host
```
Swagger: http://localhost:5127/swagger
Health: http://localhost:5127/health

## Docker Compose
```bash
docker-compose up --build
```
Exposes API on port 5127 and MongoDB on 27017.

## Configuration
App settings (env vars supported):
- `Mongo:ConnectionString`
- `Mongo:DatabaseName`
- `Booking:MaxSpotsPerUser`
- `Booking:BookingFeePercent` (decimal fraction)
- `Booking:AllowBookingAfterStart`

## API Surface
- `GET  /api/ping`
- `GET  /health`
- `GET  /api/classes`
- `GET  /api/classes/{id}`
- `POST /api/classes`
- `PUT  /api/classes/{id}`
- `DELETE /api/classes/{id}`
- `POST /api/bookings/book` with body `{ userId, classSessionId, quantity }`

## Tests
```bash
dotnet test
```
Covers booking happy-path and insufficient-spots cases.
