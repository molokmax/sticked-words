#!/bin/bash

ARTIFACT_NAME="$1"

if [ -z $ARTIFACT_NAME ] ; then
    echo "Usage: $0 {release_file}"
    exit 1
fi

SERVICE_NAME="sticked-words"
RELEASES_DIR="/home/sticked-words/deploy/"
BIN_DIR="/home/sticked-words/app"
ARTIFACT_PATH="$RELEASES_DIR/$ARTIFACT_NAME"

echo "Stopping service ..."
systemctl stop $SERVICE_NAME

echo "Deploying version $ARTIFACT_NAME ..."
rm -rf $BIN_DIR/*
unzip $ARTIFACT_PATH -d $BIN_DIR
chmod +x $BIN_DIR/StickedWords.API

echo "Starting service $SERVICE_NAME ..."
systemctl start $SERVICE_NAME

echo "Service started"
