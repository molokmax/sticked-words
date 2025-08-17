FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS backend-base
USER $APP_UID
WORKDIR /app
EXPOSE 80
EXPOSE 8080
EXPOSE 8082


FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS backend-build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/StickedWords.Domain/StickedWords.Domain.csproj", "./src/StickedWords.Domain/"]
COPY ["src/StickedWords.DbMigrations.Sqlite/StickedWords.DbMigrations.Sqlite.csproj", "./src/StickedWords.DbMigrations.Sqlite/"]
COPY ["src/StickedWords.DbMigrations.Postgres/StickedWords.DbMigrations.Postgres.csproj", "./src/StickedWords.DbMigrations.Postgres/"]
COPY ["src/StickedWords.Infrastructure/StickedWords.Infrastructure.csproj", "./src/StickedWords.Infrastructure/"]
COPY ["src/StickedWords.Infrastructure.Sqlite/StickedWords.Infrastructure.Sqlite.csproj", "./src/StickedWords.Infrastructure.Sqlite/"]
COPY ["src/StickedWords.Infrastructure.Postgres/StickedWords.Infrastructure.Postgres.csproj", "./src/StickedWords.Infrastructure.Postgres/"]
COPY ["src/StickedWords.Application/StickedWords.Application.csproj", "./src/StickedWords.Application/"]
COPY ["src/StickedWords.API/StickedWords.API.csproj", "./src/StickedWords.API/"]
COPY ["StickedWords.sln", "./"]
COPY ["Directory.Build.props", "./"]
RUN dotnet restore "./src/StickedWords.API/StickedWords.API.csproj"
COPY . .
RUN dotnet build "./src/StickedWords.API/StickedWords.API.csproj" -c $BUILD_CONFIGURATION -o /app/build
RUN dotnet publish "./src/StickedWords.API/StickedWords.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM node:24-slim AS frontend-build
WORKDIR /src/StickedWords.UI
COPY ["src/StickedWords.UI/package.json", "./"]
COPY ["src/StickedWords.UI/package-lock.json", "./"]
RUN npm install
COPY ["src/StickedWords.UI/", "./"]
RUN npm run build

FROM backend-base AS final
WORKDIR /app
COPY --from=backend-build /app/publish .
COPY --from=frontend-build /src/StickedWords.API/wwwroot ./wwwroot/
ENV ASPNETCORE_HTTP_PORTS=80;8080;8082

ENTRYPOINT ["dotnet", "StickedWords.API.dll"]