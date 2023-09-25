#!/bin/bash

source ./BuildAutomation/local_find_unity.sh

export TEST_PLATFORM=StandaloneWindows64
export PROJECT_DIR=${WORKSPACE}
export LOGFILE=$(mktemp)

"C:\Program Files\Unity\Hub\Editor\2021.3.18f1\Editor\Unity.exe" -logFile "$LOGFILE" -runTests -testPlatform PlayMode -batchmode -nographics -projectPath $PROJECT_DIR -testResults "C:\\Tests\\results.xml" -buildPlayerPath "C:\\Tests" -playerHeartbeatTimeout 60  &
unitypid=$!

tail -f -n +0 "$LOGFILE" &
tailpid=$!
wait $unitypid # Wait for Unity to finish

#./ci/test.sh
scriptsuccess=$?
kill $tailpid

exit $scriptsuccess
