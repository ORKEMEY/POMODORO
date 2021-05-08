#!/bin/sh
vault secrets enable -path=internal kv-v2
vault kv put internal/database/config password="Password_123"
vault auth enable kubernetes
vault write auth/kubernetes/config \
    token_reviewer_jwt="$(cat /var/run/secrets/kubernetes.io/serviceaccount/token)" \
    kubernetes_host="https://$KUBERNETES_PORT_443_TCP_ADDR:443" \
    kubernetes_ca_cert=@/var/run/secrets/kubernetes.io/serviceaccount/ca.crt
vault policy write authorization-service /tmp/policy.hcl
vault write auth/kubernetes/role/authorization-service \
    bound_service_account_names=authorization-service \
    bound_service_account_namespaces=default \
    policies=authorization-service \
    ttl=24h