FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CoffeeShop.Products.Api/CoffeeShop.Products.Api.csproj", "CoffeeShop.Products.Api/"]
RUN dotnet restore "./CoffeeShop.Products.Api/CoffeeShop.Products.Api.csproj"
COPY . .
WORKDIR "/src/CoffeeShop.Products.Api"
RUN dotnet build "./CoffeeShop.Products.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build


FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CoffeeShop.Products.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CoffeeShop.Products.Api.dll"]
