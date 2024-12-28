FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/PetFamily.API/PetFamily.API.csproj", "src/PetFamily.API/"]
COPY ["src/PetFamily.Application/PetFamily.Application.csproj", "src/PetFamily.Application/"]
COPY ["src/PetFamily.Domain/PetFamily.Domain.csproj", "src/PetFamily.Domain/"]
COPY ["src/PetFamily.Infrastructure/PetFamily.Infrastructure.csproj", "src/PetFamily.Infrastructure/"]
RUN dotnet restore "src/PetFamily.API/PetFamily.API.csproj"
COPY . .
WORKDIR "/src/src/PetFamily.API"
RUN dotnet build "PetFamily.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "PetFamily.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PetFamily.API.dll"]
