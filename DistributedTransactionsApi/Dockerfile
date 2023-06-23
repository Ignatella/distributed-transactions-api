FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["distributed-transactions/distributed-transactions.csproj", "distributed-transactions/"]
RUN dotnet restore "distributed-transactions/distributed-transactions.csproj"
COPY . .
WORKDIR "/src/distributed-transactions"
RUN dotnet build "distributed-transactions.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "distributed-transactions.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "distributed-transactions.dll"]
