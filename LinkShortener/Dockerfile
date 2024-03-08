FROM mcr.microsoft.com/dotnet/sdk:7.0.306 AS build-env
WORKDIR /src
EXPOSE 5129

COPY LinkShortener.Api/LinkShortener.Api.csproj ./LinkShortener.Api/
COPY LinkShortener.Domain/LinkShortener.Domain.csproj ./LinkShortener.Domain/
COPY LinkShortener.Application/LinkShortener.Application.csproj ./LinkShortener.Application/
COPY LinkShortener.Infrastructure/LinkShortener.Infrastructure.csproj ./LinkShortener.Infrastructure/
RUN dotnet restore LinkShortener.Api/LinkShortener.Api.csproj

COPY . ./

RUN dotnet publish LinkShortener.Api/LinkShortener.Api.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:7.0.9
WORKDIR /src
COPY --from=build-env /src/out .
ENTRYPOINT [ "dotnet", "LinkShortener.Api.dll" ]