#!/usr/bin/env bash

set -x

echo "Testing for $TEST_PLATFORM, Unit Type: $TESTING_TYPE"
trap 'kill $(jobs -pr)' SIGINT SIGTERM
CODE_COVERAGE_PACKAGE="com.unity.testtools.codecoverage"
PACKAGE_MANIFEST_PATH="Packages/manifest.json"

"${UNITY_EXECUTABLE}" \
  -projectPath $PROJECT_DIR \
  -runTests \
  -testPlatform $TEST_PLATFORM \
  -testResults "C:\\Builds\\results.xml" \
  -logFile "$LOGFILE" \
  -batchmode &
unitypid=$!  # Process ID of the most recently executed background pipeline (unity)

tail -f -n +0 "$LOGFILE" &
tailpid=$!
wait $unitypid # Wait for Unity to finish
UNITY_EXIT_CODE=$?

kill $tailpid

if [ $UNITY_EXIT_CODE -eq 0 ]; then
  echo "Run succeeded, no failures occurred";
elif [ $UNITY_EXIT_CODE -eq 2 ]; then
  echo "Run succeeded, some tests failed";
  if [ $TESTING_TYPE == 'JUNIT' ]; then
    echo "Converting results to JUNit for analysis";
    saxonb-xslt -s $UNITY_DIR/$TEST_PLATFORM-results.xml -xsl $CI_PROJECT_DIR/ci/nunit-transforms/nunit3-junit.xslt >$UNITY_DIR/$TEST_PLATFORM-junit-results.xml
  fi
elif [ $UNITY_EXIT_CODE -eq 3 ]; then
  echo "Run failure (other failure)";
  if [ $TESTING_TYPE == 'JUNIT' ]; then
    echo "Not converting results to JUNit";
  fi
else
  echo "Unexpected exit code $UNITY_EXIT_CODE";
  if [ $TESTING_TYPE == 'JUNIT' ]; then
    echo "Not converting results to JUNit";
  fi
fi

if grep $CODE_COVERAGE_PACKAGE $PACKAGE_MANIFEST_PATH; then
  cat $UNITY_DIR/$TEST_PLATFORM-coverage/Report/Summary.xml | grep Linecoverage
  mv $UNITY_DIR/$TEST_PLATFORM-coverage/$CI_PROJECT_NAME-opencov/*Mode/TestCoverageResults_*.xml $UNITY_DIR/$TEST_PLATFORM-coverage/coverage.xml
  rm -r $UNITY_DIR/$TEST_PLATFORM-coverage/$CI_PROJECT_NAME-opencov/
else
  {
    echo -e "\033[33mCode Coverage package not found in $PACKAGE_MANIFEST_PATH. Please install the package \"Code Coverage\" through Unity's Package Manager to enable coverage reports.\033[0m"
  } 2> /dev/null
fi

cat $UNITY_DIR/$TEST_PLATFORM-results.xml | grep test-run | grep Passed
exit $UNITY_EXIT_CODE
