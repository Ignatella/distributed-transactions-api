FROM mcr.microsoft.com/dotnet/nightly/aspnet:8.0-preview AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/nightly/sdk:8.0-preview AS build
WORKDIR /src
COPY ["DistributedTransactionsApi.csproj", "DistributedTransactionsApi/DistributedTransactionsApi.csproj"]
RUN dotnet restore "DistributedTransactionsApi/DistributedTransactionsApi.csproj"
COPY . DistributedTransactionsApi/
WORKDIR "/src/DistributedTransactionsApi"
RUN dotnet build "DistributedTransactionsApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DistributedTransactionsApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DistributedTransactionsApi.dll"]
