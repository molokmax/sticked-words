artifact_version=$1
service_name="sticked-words"
releases_dir="/home/sticked-words/deploy/"
bin_dir="/home/sticked-words/app"
artifact_name="sticked-words-linux-arm.$artifact_version.zip"
artifact_path="$releases_dir/$artifact_name"

if [ -z $1 ]
    then
        echo "Usage: $0 {version}"
        exit 1
fi

echo "Stopping service ..."
systemctl stop $service_name

echo "Deploying verion $artifact_version ..."
rm -rf $bin_dir/*
unzip $artifact_path -d $bin_dir
chmod +x $bin_dir/StickedWords.API

echo "Starting service $service_name ..."
systemctl start $service_name

echo "Service started"
