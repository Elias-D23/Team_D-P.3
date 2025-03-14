FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Sistema de citas médicas/Sistema de citas médicas..csproj", "Sistema de citas médicas/"]
RUN dotnet restore "Sistema de citas médicas/Sistema de citas médicas..csproj"
COPY . .
WORKDIR "/src/Sistema de citas médicas"
RUN dotnet build "Sistema de citas médicas..csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Sistema de citas médicas..csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Sistema de citas médicas..dll"] 