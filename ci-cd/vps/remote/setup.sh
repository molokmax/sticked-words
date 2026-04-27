#!/bin/bash

USERNAME="sticked-words"
GROUPNAME="sticked-words"

while true; do
    read -sp "Enter service account password: " OPEN_PASS
    echo
    
    if [ -z $OPEN_PASS ]; then
        echo "Password can't be empty"
    else
        break
    fi
done

read -sp "Repeat password: " REPEAT_PASS
echo
if [ "$OPEN_PASS" != "$REPEAT_PASS" ]; then
    echo "Passwords are different"
    exit 1
fi

echo "Adding account $USERNAME ..."
groupadd $GROUPNAME
useradd -m -p $(openssl passwd -6 $OPEN_PASS) -s /bin/bash -g $GROUPNAME $USERNAME

echo "Configuring account ..."
mkdir -p "/home/$USERNAME/.ssh/" || exit 1
cat "./cert_sticked-words.pub" > "/home/$USERNAME/.ssh/authorized_keys" || exit 1
chown $USERNAME:$GROUPNAME "/home/$USERNAME/.ssh/" || exit 1
chown $USERNAME:$GROUPNAME "/home/$USERNAME/.ssh/authorized_keys" || exit 1
chmod 700 "/home/$USERNAME/.ssh/" || exit 1
chmod 600 "/home/$USERNAME/.ssh/authorized_keys" || exit 1
# I don't figured out why it doesn't work
# setcap CAP_NET_BIND_SERVICE=+eip /home/$username/app/StickedWords.API

echo "Creating dir structure ..."
mkdir -p "/home/$USERNAME/deploy/" || exit 1
cp ./deploy.sh /home/$USERNAME/deploy/ || exit 1
chmod +x /home/$USERNAME/deploy/deploy.sh || exit 1
mkdir -p "/home/$USERNAME/app/" || exit 1
mkdir -p "/home/$USERNAME/db/" || exit 1
mkdir -p "/home/$USERNAME/logs/" || exit 1
chown -R $USERNAME:$GROUPNAME "/home/$USERNAME/" || exit 1

echo "Configuring service ..."
cp ./10-sticked-words.rules /etc/polkit-1/rules.d/10-sticked-words.rules || exit 1
chmod 755 /etc/polkit-1/rules.d/ || exit 1
chmod 644 /etc/polkit-1/rules.d/10-sticked-words.rules || exit 1
cp ./sticked-words.service /etc/systemd/system/sticked-words.service || exit 1
systemctl enable sticked-words || exit 1
systemctl daemon-reload || exit 1

echo "Done"
