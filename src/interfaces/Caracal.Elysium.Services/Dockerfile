FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG PASSWORD_ENV_SEEDED

#generate the cert, define the path to store it and password to use
RUN dotnet dev-certs https -ep /https/aspnetapp.pfx -p password1

WORKDIR /src
COPY ["src/interfaces/Caracal.Elysium.Services/Caracal.Elysium.Services.csproj", "src/interfaces/Caracal.Elysium.Services/"]
RUN dotnet restore "src/interfaces/Caracal.Elysium.Services/Caracal.Elysium.Services.csproj"
COPY . .
WORKDIR "/src/src/interfaces/Caracal.Elysium.Services"
RUN dotnet build "Caracal.Elysium.Services.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Caracal.Elysium.Services.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
COPY --from=build /https/* /https/
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Caracal.Elysium.Services.dll"]
