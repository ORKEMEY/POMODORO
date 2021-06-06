minikube -p minikube docker-env | Invoke-Expression
minikube addons enable ingress  
docker build -t root:0.1 -f root-service/Dockerfile .
docker build -t authorization:0.1 -f AuthorizationService/AuthorizationService/Dockerfile .
docker build -t tasks:0.1 -f Tasks/Tasks/Dockerfile .
#docker build -t email:0.1 -f EmailService/EmailService/Dockerfile .
kubectl apply -f k8s/authorization
kubectl apply -f k8s/root
kubectl apply -f k8s/tasks
#kubectl apply -f k8s/email
kubectl apply -f k8s/storage
kubectl apply -f k8s/mssql
#kubectl apply -f k8s/kafka
kubectl apply -f k8s/ingress.yaml


#minikube addons enable metrics-server
#kubectl create namespace monitoring
#helm repo add prometheus-community https://prometheus-community.github.io/helm-charts
#helm install --namespace monitoring prometheus prometheus-community/kube-prometheus-stack --set server.service.type=LoadBalancer -f k8s/prometheus.values.yaml
#kubectl apply -f .\k8s\grafana-ingress.yaml --namespace=monitoring
#kubectl get ingress --namespace=monitoring
#$pass = kubectl get secret --namespace monitoring prometheus-grafana -o jsonpath="{.data.admin-password}"
#[System.Text.Encoding]::UTF8.GetString([System.Convert]::FromBase64String($pass))

#helm upgrade prometheus prometheus-community/kube-prometheus-stack --set server.service.type=LoadBalancer --set rbac.create=false -f prometheus.values.yaml