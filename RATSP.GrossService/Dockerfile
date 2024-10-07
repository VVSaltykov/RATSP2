FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["RATSP.GrossService/RATSP.GrossService.csproj", "RATSP.GrossService/"]
COPY ["RATSP.Common/RATSP.Common.csproj", "RATSP.Common/"]
COPY ["RATSP.WebCommon/RATSP.WebCommon.csproj", "RATSP.WebCommon/"]
RUN dotnet restore "RATSP.GrossService/RATSP.GrossService.csproj"
COPY . .
WORKDIR "/src/RATSP.GrossService"
RUN dotnet build "RATSP.GrossService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "RATSP.GrossService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RATSP.GrossService.dll"]
