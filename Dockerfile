# Base runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

# Build environment
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Add OS dependencies for SBOM richness
RUN apt-get update \
 && apt-get install -y curl git bash libc6 tzdata \
 && rm -rf /var/lib/apt/lists/*

COPY ./backend/SbomDemo.Api.csproj .
RUN dotnet restore

COPY . .
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "SbomDemo.Api.dll"]
