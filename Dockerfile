# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy project files
COPY ["src/EventTicketing.Host/EventTicketing.Host.csproj", "src/EventTicketing.Host/"]
COPY ["src/EventTicketing.BL/EventTicketing.BL.csproj", "src/EventTicketing.BL/"]
COPY ["src/EventTicketing.DAL/EventTicketing.DAL.csproj", "src/EventTicketing.DAL/"]

# Restore dependencies
RUN dotnet restore "src/EventTicketing.Host/EventTicketing.Host.csproj"

# Copy source code
COPY . .

# Build
RUN dotnet build "src/EventTicketing.Host/EventTicketing.Host.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "src/EventTicketing.Host/EventTicketing.Host.csproj" -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=publish /app/publish .

EXPOSE 5127
ENTRYPOINT ["dotnet", "EventTicketing.Host.dll"]
