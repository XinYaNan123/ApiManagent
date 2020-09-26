
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-nanoserver-1903 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-nanoserver-1903 AS build
WORKDIR /src
COPY ["ApiManagent/ApiManagent.csproj", "ApiManagent/"]
COPY ["DataAccess/DataAccess.csproj", "DataAccess/"]
RUN dotnet restore "ApiManagent/ApiManagent.csproj"
COPY . .
WORKDIR "/src/ApiManagent"
RUN dotnet build "ApiManagent.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ApiManagent.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ApiManagent.dll"]