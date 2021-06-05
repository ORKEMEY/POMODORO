FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["Tasks/Tasks/Tasks.csproj", "Tasks/"]
COPY ["AuthorizationService/AuthorizationService.DAL/AuthorizationService.DAL.csproj", "/AuthorizationService.DAL/"]
RUN dotnet restore "Tasks/Tasks.csproj"
COPY Tasks/ .
COPY AuthorizationService/AuthorizationService.DAL/ /AuthorizationService.DAL/
WORKDIR "/src/Tasks"
RUN dotnet build "Tasks.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Tasks.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tasks.dll"]