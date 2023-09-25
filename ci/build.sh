#!/bin/bash

set -x

echo "Building for $BUILD_TARGET"
trap 'kill $(jobs -pr)' SIGINT SIGTERM

mkdir -p $BUILD_PATH
# Being logged in to the unity hub with a valid license will work, if you're running on a machine that you can't log in with,
# you can activate with a manual activation file like this https://docs.unity3d.com/Manual/ManualActivationCmdWin.html

"${UNITY_EXECUTABLE}" -projectPath $WORKSPACE -quit -batchmode -nographics -buildTarget $BUILD_TARGET \
-customBuildName $BUILD_NAME \
-customBuildPath $BUILD_PATH \
-executeMethod $BUILD_METHOD \
-logFile "$LOGFILE" &
unitypid=$!  # Process ID of the most recently executed background pipeline (unity)

tail -f -n +0 "$LOGFILE" &
tailpid=$!
wait $unitypid # Wait for Unity to finish
unityreturn=$?

if [ $unityreturn -eq 0 ]; then
  echo "Run succeeded, no failures occurred";
elif [ $unityreturn -eq 2 ]; then
  echo "Run succeeded, some tests failed";
elif [ $unityreturn -eq 3 ]; then
  echo "Run failure (other failure)";
else
  echo "Unexpected exit code $unityreturn";
fi

ls -la $BUILD_PATH
[ -n "$(ls -A $BUILD_PATH)" ] # fail job if build folder is empty

kill $tailpid # Kill tail now that Unity has finished writing log messages.
wait # wait until all processes from this script have finished themselves


exit $unityreturn