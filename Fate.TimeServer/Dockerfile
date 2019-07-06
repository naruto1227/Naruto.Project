FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["Fate.TimeServer/Fate.TimeServer.csproj", "Fate.TimeServer/"]
RUN dotnet restore "Fate.TimeServer/Fate.TimeServer.csproj"
COPY . .
WORKDIR "/src/Fate.TimeServer"
RUN dotnet build "Fate.TimeServer.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Fate.TimeServer.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Fate.TimeServer.dll"]