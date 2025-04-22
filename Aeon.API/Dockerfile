FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["aeon-bkd/aeon-bkd.csproj", "aeon-bkd/"]
RUN dotnet restore "aeon-bkd/aeon-bkd.csproj"
COPY . .
WORKDIR "/src/aeon-bkd"
RUN dotnet build "./aeon-bkd.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./aeon-bkd.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

COPY check-secrets.sh /app/
COPY docker-entrypoint.sh /app/
RUN chmod +x /app/check-secrets.sh /app/docker-entrypoint.sh
ENTRYPOINT ["/app/docker-entrypoint.sh"]