FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/AIAssistant/AIAssistant.GRPC/AIAssistant.GRPC.csproj", "Services/AIAssistant/AIAssistant.GRPC/"]
COPY ["Services/AIAssistant/AIAssistant.Implementation/AIAssistant.Implementation.csproj", "Services/AIAssistant/AIAssistant.Implementation/"]
RUN dotnet restore "./Services/AIAssistant/AIAssistant.GRPC/AIAssistant.GRPC.csproj"
COPY . .
WORKDIR "/src/Services/AIAssistant/AIAssistant.GRPC"
RUN dotnet build "./AIAssistant.GRPC.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./AIAssistant.GRPC.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AIAssistant.GRPC.dll"]