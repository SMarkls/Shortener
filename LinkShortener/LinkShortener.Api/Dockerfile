FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["LinkShortener.Api/LinkShortener.Api.csproj", "LinkShortener.Api/"]
RUN dotnet restore "LinkShortener.Api/LinkShortener.Api.csproj"
COPY . .
WORKDIR "/src/LinkShortener.Api"
RUN dotnet build "LinkShortener.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LinkShortener.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LinkShortener.Api.dll"]
