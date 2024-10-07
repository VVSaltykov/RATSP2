FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5027

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["RATSP.API/RATSP.API.csproj", "RATSP.API/"]
COPY ["RATSP.Common/RATSP.Common.csproj", "RATSP.Common/"]
RUN dotnet restore "RATSP.API/RATSP.API.csproj"
COPY . .
WORKDIR "/src/RATSP.API"
RUN dotnet build "RATSP.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "RATSP.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RATSP.API.dll"]
