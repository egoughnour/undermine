#!/bin/bash
set -ev
export PRJ_PARENT_DIR=$(cd ..; pwd)
${PRJ_PARENT_DIR}/Downloads/dsc-cassandra-2.0.14/bin/cqlsh -f ${PRJ_PARENT_DIR}/undermine/src/dbscripts/CreateTestKeyspace.cql