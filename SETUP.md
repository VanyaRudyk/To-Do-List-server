# Инструкция по настройке проекта

## Шаги для запуска проекта:

1. **Восстановите NuGet пакеты:**
   ```bash
   dotnet restore
   ```

2. **Создайте миграцию базы данных:**
   ```bash
   dotnet ef migrations add InitialCreate --project kursovaya.Server
   ```
   
   Если у вас не установлен EF Core Tools, установите его:
   ```bash
   dotnet tool install --global dotnet-ef
   ```

3. **Примените миграцию к базе данных:**
   ```bash
   dotnet ef database update --project kursovaya.Server
   ```

4. **Запустите проект:**
   ```bash
   dotnet run --project kursovaya.Server
   ```

## Создание пользователя-администратора

После первого запуска приложения роли "Admin" и "User" будут созданы автоматически.

Для создания пользователя-администратора вы можете:
1. Зарегистрировать пользователя через API `/api/Auth/register`
2. Затем вручную назначить роль "Admin" через базу данных или создать отдельный endpoint

## API Endpoints

### Авторизация
- `POST /api/Auth/register` - Регистрация нового пользователя
- `POST /api/Auth/login` - Вход в систему (возвращает JWT токен)

### Списки задач (ToDoList)
- `GET /api/ToDoList` - Получить все списки (User видит только свои, Admin видит все)
- `GET /api/ToDoList/{id}` - Получить список по ID
- `POST /api/ToDoList` - Создать новый список
- `PUT /api/ToDoList/{id}` - Обновить список
- `DELETE /api/ToDoList/{id}` - Удалить список

### Задачи (ToDo)
- `GET /api/ToDo` - Получить все задачи (можно фильтровать по `toDoListId`)
- `GET /api/ToDo/{id}` - Получить задачу по ID
- `POST /api/ToDo` - Создать новую задачу
- `PUT /api/ToDo/{id}` - Обновить задачу
- `PATCH /api/ToDo/{id}/status` - Изменить статус задачи (выполнено/невыполнено)
- `DELETE /api/ToDo/{id}` - Удалить задачу

## Использование JWT токена

После входа в систему вы получите JWT токен. Используйте его в заголовке запросов:
```
Authorization: Bearer <ваш_токен>
```

## Настройка строки подключения

Строка подключения к базе данных находится в `appsettings.json`. По умолчанию используется LocalDB. При необходимости измените строку подключения для вашей базы данных.



