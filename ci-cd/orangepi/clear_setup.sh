server_deploy_dir="/home/molokmax/setup/"
server_ip="192.168.1.66"
cert_path="C:/Users/maxim/.ssh/molokmax_rsa"
remote_user="molokmax"

echo "Cleaning ..."
ssh -i $cert_path $remote_user@$server_ip "rm -rf $server_deploy_dir" || exit 1

echo "Done"
