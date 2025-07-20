FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Movie/Movie.API/Movie.API.csproj", "Movie/Movie.API/"]
COPY ["Movie/Movie.BLL/Movie.BLL.csproj", "Movie/Movie.BLL/"]
COPY ["Movie/Movie.DAL/Movie.DAL.csproj", "Movie/Movie.DAL/"]
RUN dotnet restore "Movie/Movie.API/Movie.API.csproj"
COPY . .
WORKDIR "/src/Movie/Movie.API"
RUN dotnet build "./Movie.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Movie.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Movie.API.dll"]
