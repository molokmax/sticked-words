#!/bin/bash

CERT_DIR="C:/Users/maxim/.ssh"
if [[ ! -d "$CERT_DIR" ]]; then
    echo "ERROR: Direcory $CERT_DIR not found!"
    exit 1
fi

SERVERS_CONFIG_FILE="./servers.csv"
if [[ ! -f "$SERVERS_CONFIG_FILE" ]]; then
    echo "ERROR: File $SERVERS_CONFIG_FILE not found!"
    exit 1
fi

declare -a NAMES IPS USERS
line_num=0
while IFS=',' read -r name ip user arch || [[ -n "$name" ]]; do
    if [[ $line_num -eq 0 ]]; then
        line_num=1
        continue
    fi
    
    name=$(echo "$name" | tr -d '\r' | xargs)
    ip=$(echo "$ip" | tr -d '\r' | xargs)
    user=$(echo "$user" | tr -d '\r' | xargs)
    
    NAMES+=("$name")
    IPS+=("$ip")
    USERS+=("$user")
done < "$SERVERS_CONFIG_FILE"

if [[ ${#NAMES[@]} -eq 0 ]]; then
    echo "ERROR: Server config empty"
    exit 1
fi


echo "Available servers:"
for i in "${!NAMES[@]}"; do
    echo "$((i+1)) - ${NAMES[$i]}"
done

while true; do
    echo -n "Select server (1-${#NAMES[@]}): "
    read -r choice
    
    if [[ "$choice" =~ ^[0-9]+$ ]] && [[ $choice -ge 1 ]] && [[ $choice -le ${#NAMES[@]} ]]; then
        break
    else
        echo "ERROR: Wrong server number"
    fi
done

index=$((choice-1))
SERVER_NAME="${NAMES[$index]}"
SERVER_IP="${IPS[$index]}"
REMOTE_USER="${USERS[$index]}"


USER_CERT_PATH="$CERT_DIR/id_${SERVER_NAME}_${REMOTE_USER}"
if [[ ! -f "$USER_CERT_PATH" ]]; then
    echo "ERROR: File $USER_CERT_PATH not found!"
    exit 1
fi

SERVICE_ACCOUNT_CERT_PATH="$CERT_DIR/id_${SERVER_NAME}_sticked-words"
if [[ ! -f "$SERVICE_ACCOUNT_CERT_PATH" ]]; then
    echo "ERROR: File $SERVICE_ACCOUNT_CERT_PATH not found!"
    exit 1
fi

SERVICE_ACCOUNT_PUB_CERT_PATH="$CERT_DIR/id_${SERVER_NAME}_sticked-words.pub"
if [[ ! -f "$SERVICE_ACCOUNT_PUB_CERT_PATH" ]]; then
    echo "ERROR: File $SERVICE_ACCOUNT_PUB_CERT_PATH not found!"
    exit 1
fi

SERVER_DEPLOY_DIR="/home/${REMOTE_USER}/sticked-words-setup/"

echo "Uploading ..."
ssh -i $USER_CERT_PATH $REMOTE_USER@$SERVER_IP "mkdir -p $SERVER_DEPLOY_DIR" || exit 1
scp -i $USER_CERT_PATH $SERVICE_ACCOUNT_PUB_CERT_PATH $REMOTE_USER@$SERVER_IP:$SERVER_DEPLOY_DIR/cert_sticked-words.pub || exit 1
scp -i $USER_CERT_PATH "./remote/10-sticked-words.rules" $REMOTE_USER@$SERVER_IP:$SERVER_DEPLOY_DIR || exit 1
scp -i $USER_CERT_PATH "./remote/sticked-words.service" $REMOTE_USER@$SERVER_IP:$SERVER_DEPLOY_DIR || exit 1
scp -i $USER_CERT_PATH "./remote/deploy.sh" $REMOTE_USER@$SERVER_IP:$SERVER_DEPLOY_DIR || exit 1
scp -i $USER_CERT_PATH "./remote/setup.sh" $REMOTE_USER@$SERVER_IP:$SERVER_DEPLOY_DIR || exit 1
ssh -i $USER_CERT_PATH $REMOTE_USER@$SERVER_IP "chmod +x $SERVER_DEPLOY_DIR/setup.sh" || exit 1

echo "Done"
