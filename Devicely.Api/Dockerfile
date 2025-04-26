FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

RUN apt-get update && apt-get install -y postgresql-client

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Devicely.Api/Devicely.Api.csproj", "Devicely.Api/"]
RUN dotnet restore "Devicely.Api/Devicely.Api.csproj"
COPY . .
WORKDIR "/src/Devicely.Api"
RUN dotnet build "./Devicely.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Devicely.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY Devicely.Api/wait-for-postgres.sh /wait-for-postgres.sh 
RUN chmod +x /wait-for-postgres.sh
ENTRYPOINT ["/wait-for-postgres.sh", "devices_db", "dotnet", "Devicely.Api.dll"]
