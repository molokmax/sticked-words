
server_deploy_dir="/home/molokmax/setup/"
server_ip="192.168.1.66"
cert_path="C:/Users/maxim/.ssh/molokmax_rsa"
remote_user="molokmax"

echo "Uploading ..."
ssh -i $cert_path $remote_user@$server_ip "mkdir -p $server_deploy_dir" || exit 1
scp -i $cert_path "./10-sticked-words.rules" $remote_user@$server_ip:$server_deploy_dir || exit 1
scp -i $cert_path "./sticked-words.service" $remote_user@$server_ip:$server_deploy_dir || exit 1
scp -i $cert_path "C:/Users/maxim/.ssh/sticked-words_rsa.pub" $remote_user@$server_ip:$server_deploy_dir || exit 1
scp -i $cert_path "./setup.sh" $remote_user@$server_ip:$server_deploy_dir || exit 1
ssh -i $cert_path $remote_user@$server_ip "chmod +x $server_deploy_dir/setup.sh" || exit 1

echo "Done"
