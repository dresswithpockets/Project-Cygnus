#! /bin/sh

echo 'Downloading from http://netstorage.unity3d.com/unity/3757309da7e7/MacEditorInstaller/Unity-5.2.4f1.pkg: '
curl -o Unity.pkg http://download.unity3d.com/download_unity/fdbb5133b820/MacEditorInstaller/Unity-5.3.4f1.pkg

echo 'Installing Unity.pkg'
sudo installer -dumplog -package Unity.pkg -target /