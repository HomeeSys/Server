FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/Measurements/Measurements.GRPC/Measurements.GRPC.csproj", "Services/Measurements/Measurements.GRPC/"]
COPY ["Services/Measurements/Measurements.Infrastructure/Measurements.Infrastructure.csproj", "Services/Measurements/Measurements.Infrastructure/"]
COPY ["Common/CommonServiceLibrary/CommonServiceLibrary.csproj", "Common/CommonServiceLibrary/"]
COPY ["Services/Measurements/Measurements.Domain/Measurements.Domain.csproj", "Services/Measurements/Measurements.Domain/"]
RUN dotnet restore "./Services/Measurements/Measurements.GRPC/Measurements.GRPC.csproj"
COPY . .
WORKDIR "/src/Services/Measurements/Measurements.GRPC"
RUN dotnet build "./Measurements.GRPC.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Measurements.GRPC.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Measurements.GRPC.dll"]