artifact_version="$1"
src_release_path="D:/sticked-words-deploy/sticked-words-linux-arm.$artifact_version.zip"
server_deploy_dir="/home/sticked-words/deploy/"
server_ip="192.168.1.66"
cert_path="C:/Users/maxim/.ssh/sticked-words_rsa"
remote_user="sticked-words"

if [ -z $artifact_version ]
    then
        echo "Usage: $0 {version}"
        exit 1
fi

echo "Uploading version $artifact_version ..."
scp -i $cert_path $src_release_path $remote_user@$server_ip:$server_deploy_dir || exit 1

echo "Done"
