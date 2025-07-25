FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["Tarefas.API/Tarefas.API.csproj", "Tarefas.API/"]
COPY ["Tarefas.Application/Tarefas.Application.csproj", "Tarefas.Application/"]
COPY ["Tarefas.Domain/Tarefas.Domain.csproj", "Tarefas.Domain/"]
COPY ["Tarefas.Infra/Tarefas.Infra.csproj", "Tarefas.Infra/"]
RUN dotnet restore "Tarefas.API/Tarefas.API.csproj"

COPY . .
WORKDIR "/src/Tarefas.API"
RUN dotnet build "Tarefas.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Tarefas.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tarefas.API.dll"]