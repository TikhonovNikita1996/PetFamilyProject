FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/PetFamily.WEB/PetFamily.WEB.csproj", "src/PetFamily.WEB/"]
COPY ["tests/VolunteerTests/VolunteerTests.csproj", "tests/VolunteerTests/"]
COPY ["src/Shared/Pet.Family.SharedKernel/Pet.Family.SharedKernel.csproj", "src/Shared/Pet.Family.SharedKernel/"]
COPY ["src/Volunteers/PetFamily.Volunteers.Domain/PetFamily.Volunteers.Domain.csproj", "src/Volunteers/PetFamily.Volunteers.Domain/"]
COPY ["src/Shared/PetFamily.Core/PetFamily.Core.csproj", "src/Shared/PetFamily.Core/"]
COPY ["src/Shared/PetFamily.Framework/PetFamily.Framework.csproj", "src/Shared/PetFamily.Framework/"]
COPY ["src/Accounts/PetFamily.Accounts.Domain/PetFamily.Accounts.Domain.csproj", "src/Accounts/PetFamily.Accounts.Domain/"]
COPY ["src/Accounts/PetFamily.Accounts.Contracts/PetFamily.Accounts.Contracts.csproj", "src/Accounts/PetFamily.Accounts.Contracts/"]
COPY ["src/Species/PetFamily.Species.Infrastructure/PetFamily.Species.Infrastructure.csproj", "src/Species/PetFamily.Species.Infrastructure/"]
COPY ["src/Species/PetFamily.Species.Domain/PetFamily.Species.Domain.csproj", "src/Species/PetFamily.Species.Domain/"]
COPY ["src/Species/PetFamily.Species.Application/PetFamily.Species.Application.csproj", "src/Species/PetFamily.Species.Application/"]
COPY ["src/Volunteers/PetFamily.Volunteers.Contracts/PetFamily.Volunteers.Contracts.csproj", "src/Volunteers/PetFamily.Volunteers.Contracts/"]
COPY ["src/Species/PetFamily.Species.Presentation/PetFamily.Species.Presentation.csproj", "src/Species/PetFamily.Species.Presentation/"]
COPY ["src/Species/PetFamily.Species.Contracts/PetFamily.Species.Contracts.csproj", "src/Species/PetFamily.Species.Contracts/"]
COPY ["src/Volunteers/PetFamily.Volunteers.Application/PetFamily.Volunteers.Application.csproj", "src/Volunteers/PetFamily.Volunteers.Application/"]
COPY ["src/Discussions/PetFamily.Discussions.Contracts/PetFamily.Discussions.Contracts.csproj", "src/Discussions/PetFamily.Discussions.Contracts/"]
COPY ["src/Discussions/PetFamily.Discussions.Application/PetFamily.Discussions.Application.csproj", "src/Discussions/PetFamily.Discussions.Application/"]
COPY ["src/Discussions/PetFamily.Discussions.Domain/PetFamily.Discussions.Domain.csproj", "src/Discussions/PetFamily.Discussions.Domain/"]
COPY ["src/Volunteers/PetFamily.Volunteers.Infrastructure/PetFamily.Volunteers.Infrastructure.csproj", "src/Volunteers/PetFamily.Volunteers.Infrastructure/"]
COPY ["src/Volunteers/PetFamily.Volunteers.Presentation/PetFamily.Volunteers.Presentation.csproj", "src/Volunteers/PetFamily.Volunteers.Presentation/"]
COPY ["src/Discussions/PetFamily.Discussions.Infrastructure/PetFamily.Discussions.Infrastructure.csproj", "src/Discussions/PetFamily.Discussions.Infrastructure/"]
COPY ["src/Accounts/PetFamily.Accounts.Infrastructure/PetFamily.Accounts.Infrastructure.csproj", "src/Accounts/PetFamily.Accounts.Infrastructure/"]
COPY ["src/Accounts/PetFamily.Accounts.Application/PetFamily.Accounts.Application.csproj", "src/Accounts/PetFamily.Accounts.Application/"]
COPY ["src/Accounts/PetFamily.Accounts.Presentation/PetFamily.Accounts.Presentation.csproj", "src/Accounts/PetFamily.Accounts.Presentation/"]
COPY ["src/VolunteersRequests/PetFamily.VolunteersRequests.Application/PetFamily.VolunteersRequests.Application.csproj", "src/VolunteersRequests/PetFamily.VolunteersRequests.Application/"]
COPY ["src/VolunteersRequests/PetFamily.VolunteersRequests.Domain/PetFamily.VolunteersRequests.Domain.csproj", "src/VolunteersRequests/PetFamily.VolunteersRequests.Domain/"]
COPY ["src/VolunteersRequests/PetFamily.VolunteersRequests.Infrastructure/PetFamily.VolunteersRequests.Infrastructure.csproj", "src/VolunteersRequests/PetFamily.VolunteersRequests.Infrastructure/"]
COPY ["src/VolunteersRequests/PetFamily.VolunteersRequests.Contracts/PetFamily.VolunteersRequests.Contracts.csproj", "src/VolunteersRequests/PetFamily.VolunteersRequests.Contracts/"]
COPY ["src/VolunteersRequests/PetFamily.VolunteersRequests.Presentation/PetFamily.VolunteersRequests.Presentation.csproj", "src/VolunteersRequests/PetFamily.VolunteersRequests.Presentation/"]
COPY ["src/Discussions/PetFamily.Discussions.Presentation/PetFamily.Discussions.Presentation.csproj", "src/Discussions/PetFamily.Discussions.Presentation/"]
RUN dotnet restore "src/PetFamily.WEB/PetFamily.WEB.csproj"
COPY . .
WORKDIR "/src/src/PetFamily.WEB"
RUN dotnet build "PetFamily.WEB.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "PetFamily.WEB.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PetFamily.WEB.dll"]
