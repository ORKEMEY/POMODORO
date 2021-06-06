FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["Messages/Messages/Messages.csproj", "Messages/"]
COPY ["AuthorizationService/AuthorizationService.DAL/AuthorizationService.DAL.csproj", "/AuthorizationService.DAL/"]
RUN dotnet restore "Messages/Messages.csproj"
COPY Messages/ .
COPY AuthorizationService/AuthorizationService.DAL/ /AuthorizationService.DAL/
WORKDIR "/src/Messages"
RUN dotnet build "Messages.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Messages.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Messages.dll"]