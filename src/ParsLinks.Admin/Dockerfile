# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ParsLinks.Admin/ParsLinks.Admin.csproj", "ParsLinks.Admin/"]
COPY ["ParsLinks.Application/ParsLinks.Application.csproj", "ParsLinks.Application/"]
COPY ["ParsLinks.Shared/ParsLinks.Shared.csproj", "ParsLinks.Shared/"]
COPY ["ParsLinks.Localization/ParsLinks.Localization.csproj", "ParsLinks.Localization/"]
RUN dotnet restore "./ParsLinks.Admin/ParsLinks.Admin.csproj"
COPY . .
WORKDIR "/src/ParsLinks.Admin"
RUN dotnet build "./ParsLinks.Admin.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ParsLinks.Admin.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ParsLinks.Admin.dll"]