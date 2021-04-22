FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["AuthorizationService/AuthorizationService/AuthorizationService.csproj", "AuthorizationService/"]
COPY ["AuthorizationService/AuthorizationService.DAL/AuthorizationService.DAL.csproj", "AuthorizationService.DAL/"]
RUN dotnet restore "AuthorizationService/AuthorizationService.csproj"
COPY AuthorizationService/ .
WORKDIR "/src/AuthorizationService"
RUN dotnet build "AuthorizationService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AuthorizationService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AuthorizationService.dll"]