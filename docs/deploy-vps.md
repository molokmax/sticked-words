# Деплой на VPS

Артифакт с новой версией сервиса собирается с помощью GitHub Actions - `.github\workflows\self-contained-build.yaml`.

TODO: Команда подключения по ssh с использованием сертификата - `ssh -i "%USERPROFILE%\.ssh\id_rsa" USERNAME@ORANGEPI_IP`
TODO: сформировать настройки серверов

## Подготовка VPS к хостингу сервиса

### Устанновить Ubuntu 24 на VPS.
Если используется Orange Pi, тоже нужна Ubuntu - так как бинарники собираются в GitHub Actions, нужна соответствующая версия ОС на orange pi, иначе возникает ошибка отсутствия нужной версии библиотек. Скачать можно здесь - https://www.armbian.com/orange-pi-one/. Записать образ OS на SD-карту. Например, при помощи rufus.

### Сменить пароль root
Нужно сделать при первом подключении к серверу
```bash
passwd
```
### Обновить ПО
```bash
apt update && apt upgrade
```

### Создать собственного пользователя
Создать на VPS пользователя от имени которого будем выполнять настройку. Это не тот же пользователь, от имени которого будет запущен сервис. Это учетка основного пользователя.
```bash
adduser username
usermod -aG sudo username
```
На локале сгенерировать сертификат, чтобы можно было подключаться к серверу без пароля:
```bash
ssh-keygen -t ed25519 -f %USERPROFILE%/.ssh/id_[server_name]_[username]
# Для PowerShell
ssh-keygen -t ed25519 -f $env:USERPROFILE/.ssh/id_[server_name]_[username]
# Еще вариант: ssh-keygen -m PEM -t rsa -b 2048
```
Если локал - linux, то вместо `%USERPROFILE%` нужно указать `~`

Нужно добавить публичный ключ на VPS:
```bash
ssh-copy-id -i id_[server_name]_[username].pub [USERNAME]@[VPS_IP]
```
Команда выше отсутствует в Windows. Альтернативно используй это в PowerShell:
```powershell
type id_[server_name]_[username].pub | ssh [USERNAME]@[VPS_IP] "mkdir -p ~/.ssh/"
type id_[server_name]_[username].pub | ssh [USERNAME]@[VPS_IP] "cat >> ~/.ssh/authorized_keys"
```

TODO: какие права на .ssh и authorized_keys нужны?

Дальше можно подключиться к серверу под своим пользователем:
```bash
ssh -i "%USERPROFILE%\.ssh\id_[server_name]_[username]" [USERNAME]@[VPS_IP]
```

### Подготовить сертификат для сервисной учетки
На локале сгенерировать сертификат для учетной записи под которой будет запускаться сервис и выполняться деплой:
```bash
ssh-keygen -t ed25519 -f $env:USERPROFILE/.ssh/id_[server_name]_sticked-words
# еще вариант: ssh-keygen -m PEM -t rsa -b 2048
```

### Скопировать на VPS скрипты настройки
- Запустить скрипт `copy_setup.sh` из каталога `ci-cd\vps\`. Этот скрипт скопирует все необходимое для настройки. Скрипт предлагает выбрать параметры сервера из `servers.csv`.
- Залогиниться на VPS под основным пользователем и запустить скрипт `sudo setup.sh` из каталога `setup`.
- После выполнения настройки можно удалить настроечные скрипты при помощи `clear_setup.sh`.


## Деплой новой версии сервиса
- Подготовить архив `sticked-words-{arch}.{version}.zip` в каталоге с артефактами. Файлы должны лежать в корне архива.
- Для загрузки артефакта на VPS запустить скрипт `upload_artifact.sh`.
- Для обновления версии сервиса VPS запустить скрипт `deploy_artifact.sh`.


## Поезные команды
```bash
sudo systemctl status blog-app
sudo systemctl stop blog-app
sudo journalctl -u blog-app -f # смотрим логи
```

## Полезные ссылки
- https://lepkov.ru/ssh-login-to-linux-using-certificate/
- https://selectel.ru/blog/ssh-authentication/
- https://habr.com/ru/articles/964950/
