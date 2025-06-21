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

Артифакт с новой версией сервиса собирается с помощью GitHub Actions - `.github\workflows\armlinux-build.yaml`.

Команда подключения по ssh с использованием сертификата - `ssh -i "%USERPROFILE%\.ssh\id_rsa" USERNAME@ORANGEPI_IP`

### Подготовка Orange Pi к хостингу сервиса
- Устанновить Ubuntu 24 на Orange Pi (так как бинарники собираются в GitHub Actions, нужна соответствующая версия ОС на orange pi, иначе возникает ошибка отсутствия нужной версии библиотек). Скачать можно здесь - https://www.armbian.com/orange-pi-one/.
- Записать образ OS на SD-карту. Например, при помощи rufus.
- Создать на Orange Pi пользователя от имени которого будем выполнять настройку. Это не тот же пользователь, от имени которого будет запущен сервис.
- Для удобства подключения можно сгенерировать сертификат и подключаться по сертификату без использования пароля. Генерация сертификата выполняется командой `ssh-keygen -m PEM -t rsa -b 2048`. Можно сразу сгенерировать 2 сертификата - для пользователя под которым выполняется администрирование Orange Pi и для пользователя, под которым деплоится и запускается сервис. Дополнительные ссылки - https://lepkov.ru/ssh-login-to-linux-using-certificate/, https://selectel.ru/blog/ssh-authentication/.
- Запустить скрипт `copy_setup.sh` из каталога `ci-cd\orangepi\`. Этот скрипт скопирует все необходимое для настройки на Orange Pi. Перед запуском проверить настройки в скрипте.
- Залогиниться на Orange Pi под пользователем для администрирования и запустить скрипт `sudo setup.sh {password}` из каталога `setup`.
- Выполнить очистку истории команд - `cat /dev/null > ~/.bash_history && history -c` чтобы нельзя было получить пароль из истории.
- После выполнения настройки можно удалить настроечные скрипты при помощи `clear_setup.sh`.

### Деплой новой версии сервиса
- Подготовить архив `sticked-words-linux-arm.{version}.zip` в каталоге с артефактами. Файлы должны лежать в корне архива.
- Для загрузки артефакта на Orange Pi запустить скрипт `upload_artifact.sh {version}`. Перед запуском проверить настройки в скрипте.
- Для обновления версии сервиса Orange Pi запустить скрипт `deploy_artifact.sh {version}`. Перед запуском проверить настройки в скрипте.

TODO: добавить периодическую задачу для удаления старых артифактов (удалять старше 2 недель, но оставлять минимум 2 версии)
