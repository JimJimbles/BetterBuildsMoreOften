#!/bin/bash

source ./BuildAutomation/local_find_unity.sh

export LOGFILE=$(mktemp)
./ci/build.sh
scriptsuccess=$?

wait
# Remove temp file
rm $LOGFILE

exit $scriptsuccess
