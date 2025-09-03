#!/bin/bash
# failover.sh
# Arguments: failed_node_id new_master_id old_master_id

FAILED_NODE_ID=$1
NEW_MASTER_ID=$2
OLD_MASTER_ID=$3

echo "Failover triggered!"
echo "Failed node ID: $FAILED_NODE_ID"
echo "New master ID: $NEW_MASTER_ID"
echo "Old master ID: $OLD_MASTER_ID"

# Promote the new master if it’s a slave
if [ "$FAILED_NODE_ID" = "0" ]; then
  if [ "$NEW_MASTER_ID" = "1" ]; then
    echo "Promoting postgres-slave1..."
    docker exec postgres-slave1 psql -U admin -d appdb -c "SELECT pg_promote();"
  elif [ "$NEW_MASTER_ID" = "2" ]; then
    echo "Promoting postgres-slave2..."
    docker exec postgres-slave2 psql -U admin -d appdb -c "SELECT pg_promote();"
  fi
fi

exit 0
