FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ApiGateways/ApiGateway.Main/ApiGateway.Main.csproj", "ApiGateways/ApiGateway.Main/"]
RUN dotnet restore "./ApiGateways/ApiGateway.Main/ApiGateway.Main.csproj"
COPY . .
WORKDIR "/src/ApiGateways/ApiGateway.Main"
RUN dotnet build "./ApiGateway.Main.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ApiGateway.Main.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ApiGateway.Main.dll"]