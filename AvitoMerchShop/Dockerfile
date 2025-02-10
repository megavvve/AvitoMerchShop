# Этот этап используется при запуске из VS в быстром режиме (для конфигурации отладки)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Этот этап используется для сборки проекта службы
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
# Копируем файл проекта в текущую директорию (в /src)
COPY ["AvitoMerchShop.csproj", "./"]
# Выполняем восстановление, указывая путь к файлу проекта, который лежит в /src
RUN dotnet restore "AvitoMerchShop.csproj"
# Копируем остальные файлы
COPY . .
# Строим проект
RUN dotnet build "AvitoMerchShop.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Этап публикации
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "AvitoMerchShop.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Финальный этап
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AvitoMerchShop.dll"]
