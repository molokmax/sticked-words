artifact_version="$1"
server_deploy_dir="/home/sticked-words/deploy/"
server_ip="192.168.1.66"
cert_path="C:/Users/maxim/.ssh/sticked-words_rsa"
deploy_script_name="deploy.sh"
remote_user="sticked-words"

if [ -z $artifact_version ] ; then
    echo "Usage: $0 {version}"
    exit 1
fi

echo "Deploying $artifact_version ..."
scp -i $cert_path "./$deploy_script_name" $remote_user@$server_ip:$server_deploy_dir || exit 1
ssh -i $cert_path $remote_user@$server_ip "chmod +x $server_deploy_dir/$deploy_script_name" || exit 1
ssh -i $cert_path $remote_user@$server_ip "$server_deploy_dir/$deploy_script_name '$artifact_version'" || exit 1

echo "Done"
