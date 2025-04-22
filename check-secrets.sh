#!/bin/bash
set -e

if [ ! -f "/app/secrets.json" ]; then
    echo "Erro: The Secrets file is not at the root of the project!"
    exit 1
fi

REQUIRED_KEYS=("POSTGRES_USER" "POSTGRES_PASSWORD" "RABBITMQ_DEFAULT_USER" "RABBITMQ_DEFAULT_PASS")

for key in "${REQUIRED_KEYS[@]}"; do
    if ! jq -e "has(\"$key\")" "/app/secrets.json" > /dev/null; then
        echo "Erro: The key '$key' It was not found in the file!"
        exit 1
    fi
done

echo "✅ Let's Goo boy!!"
exit 0