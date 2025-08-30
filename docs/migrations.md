# Работа с миграциями

Есть 2 проекта с миграциями - один для Sqlite (StickedWords.DbMigrations.Sqlite), другой - для Postgres (StickedWords.DbMigrations.Postgres). Их приходится вести раздельно, так как для разных баз данных генерируются разные миграции.

## Добавить миграцию

Запускать из каталога проекта /src/StickedWords.DbMigrations.*
```
dotnet ef migrations add InitialCreate
```

## Применить миграции
Для локальной Sqlite базы:
```
dotnet ef database update -- --ConnectionStrings:StickedWordsDbContext "Data Source=|DataDirectory|stickedwords.db"
```

Запускать из каталога проекта /src/StickedWords.DbMigrations.Sqlite.

Для локальной Postgres базы можно запустить аналогично, скорректировав строку подключения.