username="sticked-words"
groupname="sticked-words"
openpass=$1

if [ -z $1 ]
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

echo "Creating dir structure ..."
mkdir -p "/home/$username/deploy/" || exit 1
mkdir -p "/home/$username/app/" || exit 1
mkdir -p "/home/$username/db/" || exit 1
mkdir -p "/home/$username/logs/" || exit 1
chown -R $username:$groupname "/home/$username/" || exit 1

echo "Configuring service ..."
cp ./10-sticked-words.rules /etc/polkit-1/rules.d/10-sticked-words.rules
chmod 755 /etc/polkit-1/rules.d/
chmod 644 /etc/polkit-1/rules.d/10-sticked-words.rules
cp ./sticked-words.service /etc/systemd/system/sticked-words.service
systemctl daemon-reload

echo "Done"
