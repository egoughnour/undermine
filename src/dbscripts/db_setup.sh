#!/bin/bash
set -ev
export PRJ_PARENT_DIR=$(cd ..; pwd)
cqlsh -f ${PRJ_PARENT_DIR}/undermine/src/dbscripts/CreateTestKeyspace.cql