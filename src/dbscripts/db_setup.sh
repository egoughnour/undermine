#!/bin/bash
set -ev
export PRJ_SOURCE_DIRECTORY=$(cd ..; pwd)
cqlsh -f ${PRJ_SOURCE_DIRECTORY}/dbscripts/CreateTestKeyspace.cql