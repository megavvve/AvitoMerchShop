FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ./AvitoMerchShop/*.csproj ./AvitoMerchShop/
RUN dotnet restore "AvitoMerchShop/AvitoMerchShop.csproj"

COPY . .

RUN dotnet publish "AvitoMerchShop/AvitoMerchShop.csproj" \
    -c Release \
    -o /app/publish \
    --no-restore \
    /p:PreserveCompilationContext=true

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "AvitoMerchShop.dll"]