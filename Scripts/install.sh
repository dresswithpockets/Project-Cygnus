#! /bin/sh

echo 'Downloading from http://netstorage.unity3d.com/unity/3757309da7e7/MacEditorInstaller/Unity-5.2.4f1.pkg: '
curl -o Unity.pkg http://netstorage.unity3d.com/unity/960ebf59018a/MacEditorInstaller/Unity-5.3.5f1.pkg

echo 'Installing Unity.pkg'
sudo installer -dumplog -package Unity.pkg -target /