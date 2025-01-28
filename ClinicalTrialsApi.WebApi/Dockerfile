#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ClinicalTrialsApi.WebApi/ClinicalTrialsApi.WebApi.csproj", "ClinicalTrialsApi.WebApi/"]
RUN dotnet restore "./ClinicalTrialsApi.WebApi/./ClinicalTrialsApi.WebApi.csproj"
COPY . .
WORKDIR "/src/ClinicalTrialsApi.WebApi"
RUN dotnet build "./ClinicalTrialsApi.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ClinicalTrialsApi.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ClinicalTrialsApi.WebApi.dll"]