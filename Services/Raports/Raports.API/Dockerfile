FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/Raports/Raports.API/Raports.API.csproj", "Services/Raports/Raports.API/"]
COPY ["Services/Raports/Raports.Application/Raports.Application.csproj", "Services/Raports/Raports.Application/"]
COPY ["Common/CommonServiceLibrary.Messaging/CommonServiceLibrary.Messaging.csproj", "Common/CommonServiceLibrary.Messaging/"]
COPY ["Services/Raports/Raports.Infrastructure/Raports.Infrastructure.csproj", "Services/Raports/Raports.Infrastructure/"]
COPY ["Common/CommonServiceLibrary.BlobStorage/CommonServiceLibrary.BlobStorage.csproj", "Common/CommonServiceLibrary.BlobStorage/"]
COPY ["Common/CommonServiceLibrary.GRPC/CommonServiceLibrary.GRPC.csproj", "Common/CommonServiceLibrary.GRPC/"]
COPY ["Common/CommonServiceLibrary/CommonServiceLibrary.csproj", "Common/CommonServiceLibrary/"]
COPY ["Services/Raports/Raports.Domain/Raports.Domain.csproj", "Services/Raports/Raports.Domain/"]
RUN dotnet restore "./Services/Raports/Raports.API/Raports.API.csproj"
COPY . .
WORKDIR "/src/Services/Raports/Raports.API"
RUN dotnet build "./Raports.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Raports.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Raports.API.dll"]