# Observability

## Настройка Prometheus и Grafana в Kubernates

создать namespace:
```
kubectl create namespace monitoring
```

так можно переключить контекст на этот namespace:
```
kubectl config set-context --current --namespace=monitoring
```

установка prometheus:
```
helm repo add prometheus-community https://prometheus-community.github.io/helm-charts
helm repo update
helm install prometheus prometheus-community/kube-prometheus-stack -f .\prometheus.yaml --atomic
```

получить пароль от графаны:
```
kubectl --namespace monitoring get secrets prometheus-grafana -o jsonpath="{.data.admin-password}" | base64 -d ; echo
```

пробросить графану и прометеус:
```
kubectl port-forward svc/prometheus-kube-prometheus-prometheus -n monitoring 9090:9090
kubectl port-forward svc/prometheus-grafana -n monitoring 3000:80
```

создать service monitor:
```
kubectl apply -f .\ci-cd\k8s\servicemonitor.yaml
```

В grafana можно импортировать dashboard - [grafana-dashboard.json](./grafana-dashboard.json). В нем настроены графики по RPS, Latancy и ошибкам.
