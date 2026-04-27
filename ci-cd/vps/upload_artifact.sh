#!/bin/bash

SRC_RELEASE_DIR="D:/sticked-words-deploy"
if [[ ! -d "$SRC_RELEASE_DIR" ]]; then
    echo "ERROR: Direcory $SRC_RELEASE_DIR not found!"
    exit 1
fi

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

declare -a NAMES IPS USERS ARCHS
line_num=0
while IFS=',' read -r name ip user arch || [[ -n "$name" ]]; do
    if [[ $line_num -eq 0 ]]; then
        line_num=1
        continue
    fi
    
    name=$(echo "$name" | tr -d '\r' | xargs)
    ip=$(echo "$ip" | tr -d '\r' | xargs)
    user=$(echo "$user" | tr -d '\r' | xargs)
    arch=$(echo "$arch" | tr -d '\r' | xargs)
    
    NAMES+=("$name")
    IPS+=("$ip")
    USERS+=("$user")
    ARCHS+=("$arch")
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
SERVER_ARCH="${ARCHS[$index]}"


SERVICE_ACCOUNT_CERT_PATH="$CERT_DIR/id_${SERVER_NAME}_sticked-words"
if [[ ! -f "$SERVICE_ACCOUNT_CERT_PATH" ]]; then
    echo "ERROR: File $SERVICE_ACCOUNT_CERT_PATH not found!"
    exit 1
fi


declare -a RELEASE_FILES_WITH_DATES

for file in "$SRC_RELEASE_DIR"/sticked-words-"$SERVER_ARCH".*.zip; do
    [[ -f "$file" ]] || continue
    if CREATION_TIME=$(stat -c '%W' "$file" 2>/dev/null); then
        [[ "$CREATION_TIME" == "0" ]] && CREATION_TIME=$(stat -c '%Y' "$file")
    else
        CREATION_TIME=$(stat -f '%B' "$file")
    fi
    RELEASE_FILES_WITH_DATES+=("$CREATION_TIME|$file")
done

IFS=$'\n' SORTED_RELEASE_FILES=($(sort -rn <<<"${RELEASE_FILES_WITH_DATES[*]}"))
unset IFS

RELEASE_FILES=()
for item in "${SORTED_RELEASE_FILES[@]}"; do
    RELEASE_FILES+=("${item#*|}")
done

if [[ ${#RELEASE_FILES[@]} -eq 0 ]]; then
    echo "Releases not found in $SRC_RELEASE_DIR"
    exit 1
fi

echo "Available releases:"
for i in "${!RELEASE_FILES[@]}"; do
    filename=$(basename "${RELEASE_FILES[$i]}")
    echo "$((i+1)). $filename"
done

while true; do
    echo
    read -p "Select release (1-${#RELEASE_FILES[@]}): " choice
    
    if [[ "$choice" =~ ^[0-9]+$ ]] && [[ $choice -ge 1 ]] && [[ $choice -le ${#RELEASE_FILES[@]} ]]; then
        SELECTED_RELEASE=$(basename "${RELEASE_FILES[$choice-1]}")
        echo "Selected release: $SELECTED_RELEASE"
        break
    else
        echo "Wrong release number"
    fi
done


SRC_RELEASE_PATH="$SRC_RELEASE_DIR/$SELECTED_RELEASE"
REMOTE_DEPLOY_DIR="/home/sticked-words/deploy/"

echo "Uploading ..."
scp -i $SERVICE_ACCOUNT_CERT_PATH $SRC_RELEASE_PATH sticked-words@$SERVER_IP:$REMOTE_DEPLOY_DIR || exit 1

echo "================================"
read -p "Whould you like to deploy $SELECTED_RELEASE right now? y/n: " deploy_choice
if [[ "$deploy_choice" == "Y" ]] || [[ "$deploy_choice" == "y" ]]; then
    echo "Deploying ..."
    ssh -i $SERVICE_ACCOUNT_CERT_PATH sticked-words@$SERVER_IP "${REMOTE_DEPLOY_DIR}deploy.sh '$SELECTED_RELEASE'" || exit 1
fi

echo "Done"
