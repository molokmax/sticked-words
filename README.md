# sticked-words
Сервис для запоминания иностранных слов

## Общая информация

- [Словарь терминов](./docs/glossary.md)

## Работа с миграциями

### Добавить миграцию

Запускать из каталога проекта /src/StickedWords.DbMigrations

dotnet ef migrations add InitialCreatу -- --ConnectionStrings:StickedWordsDbContext "Data Source=|DataDirectory|stickedwords.db"

### Применить миграции

dotnet ef database update -- --ConnectionStrings:StickedWordsDbContext "Data Source=|DataDirectory|stickedwords.db"

Запускать из каталога проекта /src/StickedWords.DbMigrations
