openpass="$1"
username="sticked-words"
groupname="sticked-words"

if [ -z $openpass ]
    then
        echo "Usage: $0 {password}"
        exit 1
fi

echo "Adding account $username ..."
groupadd $username
useradd -m -p $(openssl passwd -6 $openpass) -g $groupname $username

echo "Configuring account ..."
mkdir -p "/home/$username/.ssh/" || exit 1
cat "./sticked-words_rsa.pub" > "/home/$username/.ssh/authorized_keys" || exit 1
chown $username:$groupname "/home/$username/.ssh/" || exit 1
chown $username:$groupname "/home/$username/.ssh/authorized_keys" || exit 1
chmod 700 "/home/$username/.ssh/" || exit 1
chmod 600 "/home/$username/.ssh/authorized_keys" || exit 1
# I don't figured out why it doesn't work
# setcap CAP_NET_BIND_SERVICE=+eip /home/$username/app/StickedWords.API

echo "Creating dir structure ..."
mkdir -p "/home/$username/deploy/" || exit 1
mkdir -p "/home/$username/app/" || exit 1
mkdir -p "/home/$username/db/" || exit 1
mkdir -p "/home/$username/logs/" || exit 1
chown -R $username:$groupname "/home/$username/" || exit 1

echo "Configuring service ..."
cp ./10-sticked-words.rules /etc/polkit-1/rules.d/10-sticked-words.rules || exit 1
chmod 755 /etc/polkit-1/rules.d/ || exit 1
chmod 644 /etc/polkit-1/rules.d/10-sticked-words.rules || exit 1
cp ./sticked-words.service /etc/systemd/system/sticked-words.service || exit 1
systemctl daemon-reload || exit 1

echo "Done"
