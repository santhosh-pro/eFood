#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/Services/Catalog/eFood.Catalog.WebApi/eFood.Catalog.WebApi.csproj", "src/Services/Catalog/eFood.Catalog.WebApi/"]
COPY ["src/Common/eFood.Common/eFood.Common.csproj", "src/Common/eFood.Common/"]
RUN dotnet restore "src/Services/Catalog/eFood.Catalog.WebApi/eFood.Catalog.WebApi.csproj"
COPY . .
WORKDIR "/src/src/Services/Catalog/eFood.Catalog.WebApi"
RUN dotnet build "eFood.Catalog.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "eFood.Catalog.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "eFood.Catalog.WebApi.dll"]