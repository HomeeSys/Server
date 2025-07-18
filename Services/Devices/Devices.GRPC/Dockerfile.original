FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/Devices/Devices.GRPC/Devices.GRPC.csproj", "Services/Devices/Devices.GRPC/"]
COPY ["Services/Devices/Devices.Infrastructure/Devices.Infrastructure.csproj", "Services/Devices/Devices.Infrastructure/"]
COPY ["Common/CommonServiceLibrary/CommonServiceLibrary.csproj", "Common/CommonServiceLibrary/"]
COPY ["Services/Devices/Devices.Domain/Devices.Domain.csproj", "Services/Devices/Devices.Domain/"]
RUN dotnet restore "./Services/Devices/Devices.GRPC/Devices.GRPC.csproj"
COPY . .
WORKDIR "/src/Services/Devices/Devices.GRPC"
RUN dotnet build "./Devices.GRPC.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Devices.GRPC.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Devices.GRPC.dll"]