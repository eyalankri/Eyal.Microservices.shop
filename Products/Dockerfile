# Products

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Products/Products.csproj", "Products/"]
RUN dotnet restore "./Products/./Products.csproj"
COPY . .
WORKDIR "/src/Products"
RUN dotnet build "./Products.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Products.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Create Cert directory and copy cert.pem // this not working
WORKDIR /app
RUN mkdir -p ./cert
COPY ["cert.pem", "./cert/"]
WORKDIR /app/cert
RUN chmod 644 ./cert.pem && update-ca-certificates
WORKDIR /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Products.dll"]
