FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080


FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["Tic Tac Toe/Tic Tac Toe.csproj", "Tic Tac Toe/"]
RUN dotnet restore "./Tic Tac Toe/Tic Tac Toe.csproj"

COPY . .
WORKDIR "/src/Tic Tac Toe"
RUN dotnet build "./Tic Tac Toe.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Tic Tac Toe.csproj" -c $BUILD_CONFIGURATION -o /app/publish 

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tic Tac Toe.dll"]