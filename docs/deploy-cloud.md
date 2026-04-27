# Деплой в облако

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
