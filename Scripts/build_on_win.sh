#! /bin/sh

project="Cygnus"

echo "Attempting to build $project for Windows"
/c/Program\ Files/Unity/Editor/Editor.exe \
  -batchmode \
  -nographics \
  -silent-crashes \
  -logFile $(pwd)/unity.log \
  -buildTarget win32
  -projectPath $(pwd) \
  -buildWindowsPlayer "$(pwd)/Build/windows/$project.exe" \
  -quit

echo "Attempting to build $project for OS X"
/c/Program\ Files/Unity/Editor/Editor.exe \
  -batchmode \
  -nographics \
  -silent-crashes \
  -logFile $(pwd)/unity.log \
  -buildTarget osx
  -projectPath $(pwd) \
  -buildOSXUniversalPlayer "$(pwd)/Build/osx/$project.app" \
  -quit

echo "Attempting to build $project for Linux"
/c/Program\ Files/Unity/Editor/Editor.exe \
  -batchmode \
  -nographics \
  -silent-crashes \
  -logFile $(pwd)/unity.log \
  -projectPath $(pwd) \
  -buildLinuxUniversalPlayer "$(pwd)/Build/linux/$project" \
  -quit

echo '\n\nLogs from build\n'
cat $(pwd)/unity.log