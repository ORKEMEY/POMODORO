minikube -p minikube docker-env | Invoke-Expression
minikube addons enable ingress
./vault.ps1
docker build -t root:0.1 -f root-service/Dockerfile .
docker build -t authorization:0.1 -f AuthorizationService/AuthorizationService/Dockerfile .
docker build -t email:0.1 -f EmailService/EmailService/Dockerfile .
kubectl apply -f k8s/vault
kubectl apply -f k8s/authorization
kubectl apply -f k8s/root
kubectl apply -f k8s/email
kubectl apply -f k8s/storage
kubectl apply -f k8s/mssql
kubectl apply -f k8s/kafka
kubectl apply -f k8s/ingress.yaml