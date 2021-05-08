helm repo add hashicorp https://helm.releases.hashicorp.com
helm install vault hashicorp/vault --set "server.dev.enabled=true"
Start-Sleep -s 45
kubectl cp k8s/vault/policy.hcl vault-0:/tmp/
kubectl cp k8s/vault/vault.sh vault-0:/tmp/
kubectl exec -it vault-0 -- /bin/sh /tmp/vault.sh
