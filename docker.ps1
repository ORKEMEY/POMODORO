minikube -p minikube docker-env | Invoke-Expression
minikube addons enable ingress
docker build -t root:0.1 -f root-service/Dockerfile .
docker build -t authorization:0.1 -f AuthorizationService/AuthorizationService/Dockerfile .
istioctl install --set profile=demo -y
kubectl label namespace default istio-injection=enabled
kubectl apply -f k8s/root
kubectl apply -f k8s/authorization
kubectl apply -f k8s/ingress.yaml
kubectl apply -f k8s/virtual-service/
kubectl apply -f k8s/destination-rule/