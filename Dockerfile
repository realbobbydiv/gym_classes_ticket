# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy project files
COPY ["src/GymClassBooking.Host/GymClassBooking.Host.csproj", "src/GymClassBooking.Host/"]
COPY ["src/GymClassBooking.BL/GymClassBooking.BL.csproj", "src/GymClassBooking.BL/"]
COPY ["src/GymClassBooking.DAL/GymClassBooking.DAL.csproj", "src/GymClassBooking.DAL/"]

# Restore dependencies
RUN dotnet restore "src/GymClassBooking.Host/GymClassBooking.Host.csproj"

# Copy source code
COPY . .

# Build
RUN dotnet build "src/GymClassBooking.Host/GymClassBooking.Host.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "src/GymClassBooking.Host/GymClassBooking.Host.csproj" -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=publish /app/publish .

EXPOSE 5127
ENTRYPOINT ["dotnet", "GymClassBooking.Host.dll"]
