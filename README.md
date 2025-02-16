# AvitoMerchShop
 
# Магазин мерча Авито

## Описание
Сервис для внутреннего использования сотрудниками Авито, позволяющий:
- Покупать мерч за виртуальные монеты (1000 монет при регистрации)
- Переводить монеты между сотрудниками
- Просматривать историю операций

**Технологический стек**:
- Язык: C#
- БД: PostgreSQL
- Аутентификация: JWT
- Контейнеризация: Docker
- Тестирование: xUnit, Integration tests

## Быстрый старт

### Требования
- Docker 20.10+
- Docker Compose 2.0+

### Запуск

git clone https://github.com/megavvve/avito-merch-shop.git

# Сборка и запуск
docker-compose build

docker-compose up

Сервис будет доступен по адресу: http://localhost:8080

### Настройка БД
Инициализация данных
После первого запуска выполните:
```
# Создание миграций
dotnet ef migrations add InitialCreate --project AvitoMerchShop

# Применение миграций
dotnet ef database update --project AvitoMerchShop

# Заполнение товаров
docker exec -it avitomerchshop-db-1 psql -U postgres -d avito_merch

INSERT INTO "Items" ("Name", "Price") VALUES
('t-shirt', 80),
('cup', 20),
('book', 50),
('pen', 10),
('powerbank', 200),
('hoody', 300),
('umbrella', 200),
('socks', 10),
('wallet', 50),
('pink-hoody', 500);
```
### Работа с API
Работа с Api происходит по ссылке:
http://localhost:8080/swagger/index.html

Сперва нужно получить JWT token и в Authorize ввести: "Bearer [JWT token]"

### Проблемы
Проблемы возникли с тестами PurchaseFlowTests TransferFlowTests

Тесты начали падать из за отсутствия файла зависимостей testhost.deps.json
