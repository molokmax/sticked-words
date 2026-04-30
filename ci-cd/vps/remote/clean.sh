#!/bin/bash

USERNAME="sticked-words"
GROUPNAME="sticked-words"

systemctl stop sticked-words || true
systemctl disable sticked-words || true
systemctl daemon-reload

killall -u $USERNAME
deluser --remove-home $USERNAME
delgroup $GROUPNAME
rm "/etc/systemd/system/sticked-words.service"

echo "Done"
