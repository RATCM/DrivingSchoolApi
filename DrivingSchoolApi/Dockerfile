FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["DrivingSchoolApi/DrivingSchoolApi.csproj", "DrivingSchoolApi/"]
COPY ["DrivingSchoolApi.Application/DrivingSchoolApi.Application.csproj", "DrivingSchoolApi.Application/"]
COPY ["DrivingSchoolApi.Domain/DrivingSchoolApi.Domain.csproj", "DrivingSchoolApi.Domain/"]
COPY ["DrivingSchoolApi.Infrastructure/DrivingSchoolApi.Infrastructure.csproj", "DrivingSchoolApi.Infrastructure/"]
RUN dotnet restore "DrivingSchoolApi/DrivingSchoolApi.csproj"
COPY . .
WORKDIR "/src/DrivingSchoolApi"
RUN dotnet build "./DrivingSchoolApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./DrivingSchoolApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DrivingSchoolApi.dll"]
