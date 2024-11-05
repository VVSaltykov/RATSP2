FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["RATSP.WebClient/RATSP.WebClient.csproj", "RATSP.WebClient/"]
COPY ["RATSP.Common/RATSP.Common.csproj", "RATSP.Common/"]
COPY ["RATSP.WebCommon/RATSP.WebCommon.csproj", "RATSP.WebCommon/"]
RUN dotnet restore "RATSP.WebClient/RATSP.WebClient.csproj"
COPY . .
WORKDIR "/src/RATSP.WebClient"
RUN dotnet build "RATSP.WebClient.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "RATSP.WebClient.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RATSP.WebClient.dll"]
