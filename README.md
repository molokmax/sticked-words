# sticked-words
Сервис для запоминания иностранных слов

## Общая информация

- [Словарь терминов](./docs/glossary.md)

## Работа с миграциями

Есть 2 проекта с миграциями - один для Sqlite (StickedWords.DbMigrations.Sqlite), другой - для Postgres (StickedWords.DbMigrations.Postgres). Их приходится вести раздельно, так как для разных баз данных генерируются разные миграции.

### Добавить миграцию

Запускать из каталога проекта /src/StickedWords.DbMigrations.*
```
dotnet ef migrations add InitialCreate
```

### Применить миграции
Для локальной Sqlite базы:
```
dotnet ef database update -- --ConnectionStrings:StickedWordsDbContext "Data Source=|DataDirectory|stickedwords.db"
```

Запускать из каталога проекта /src/StickedWords.DbMigrations.Sqlite.

Для локальной Postgres базы можно запустить аналогично, скорректировав строку подключения.

## Сборка для облака
Подготовить образ:
```
docker build . -t molokmax/sticked-words:0.0.1 -f ./ci-cd/Dockerfile
docker push molokmax/sticked-words:0.0.1
docker build . -t molokmax/sticked-words-migrations:0.0.1 -f ./ci-cd/Dockerfile.migrations
docker push molokmax/sticked-words-migrations:0.0.1
```

Запустить из образа (скорректировать `.env` для sqlite/postgres):
```
docker run -d -p 8080:8080 --env-file ./ci-cd/.env molokmax/sticked-words:0.0.1
```

Запустить в Kubernates:
```
minikube start

# Проверить что плагин ingress включен
minikube addons enable ingress

# Установить postgresql
helm install pg oci://registry-1.docker.io/bitnamicharts/postgresql -f ./ci-cd/k8s/dv/values.yaml

# Пробросить порты postgres на хостовую машину
kubectl port-forward --namespace default svc/pg-postgresql 5432:5432

# Создать настройки и применить миграции БД
kubectl apply -f ./ci-cd/k8s/config.yaml
kubectl apply -f ./ci-cd/k8s/db/initdb.yaml

# Запустить само приложение
kubectl apply -f ./ci-cd/k8s/

# Создать туннель между кластером k8s и хостовой машиной
minikube tunnel
```
Проверить, что в hosts добавлен alias `127.0.0.1 sticked-words.local`

К сервису теперь можно обращаться через http://sticked-words.local

## Деплой на Orange Pi

Устанавливаем Ubuntu 24 на Orange Pi (так как бинарники собираются в github actions, нужна соответствующая версия ОС на orange pi, иначе возникает ошибка отсутствия нужной версии библиотек). Скачать можно здесь - https://www.armbian.com/orange-pi-one/.

Записываем на SD-карту. Например, при помощи rufus

Генерируем сертификат, чтобы можно было подключаться без пароля:
`ssh-keygen -m PEM -t rsa -b 2048`

В Orange Pi добавляем публичный ключ в файл ~/.ssh/authorized_keys. Проверяем права на каталог .ssh (700) и файл authorized_keys (600).

Команда подключения по ssh с использованием сертификата:
`ssh -i "%USERPROFILE%\.ssh\orangepi_rsa" USERNAME@ORANGEPI_IP`

