var builder = DistributedApplication.CreateBuilder(args);

var pgMaster = builder.AddContainer("postgres-master", "postgres:latest")
    .WithEnvironment("POSTGRES_USER", "masteruser")
    .WithEnvironment("POSTGRES_PASSWORD", "masterpass")
    .WithEnvironment("POSTGRES_DB", "appdb")
    // Runs on first-ever init of the data dir
    .WithBindMount("./init-master", "/docker-entrypoint-initdb.d")
    // Enable WAL streaming etc.  NOTE: no leading spaces in values.
    .WithArgs(
        "-c", "listen_addresses=*",
        "-c", "wal_level=replica",
        "-c", "max_wal_senders=10",
        "-c", "max_replication_slots=10",
        "-c", "hot_standby=on"
    )
    .WithEndpoint(5432, 5432);

var pgSlave1 = builder.AddContainer("postgres-slave1", "postgres:latest")
    .WithEnvironment("POSTGRES_USER", "replicauser")
    .WithEnvironment("POSTGRES_PASSWORD", "replicapass")
    .WithEntrypoint("bash")
    .WithArgs("-c",
        // wait until master is accepting connections
        "set -e; " +
        "until pg_isready -h postgres-master -U replicator; do echo waiting-for-master; sleep 2; done; " +
        // if already initialized, just start
        "if [ -s /var/lib/postgresql/data/PG_VERSION ]; then echo replica1-already-initialized; exec docker-entrypoint.sh postgres; fi; " +
        // fresh clone from master + auto standby (-R)
        "rm -rf /var/lib/postgresql/data/*; " +
        "PGPASSWORD=replica_pass pg_basebackup -h postgres-master -D /var/lib/postgresql/data -U replicator -Fp -Xs -P -R; " +
        "exec docker-entrypoint.sh postgres")
    .WithEndpoint(5433, 5432);

var pgSlave2 = builder.AddContainer("postgres-slave2", "postgres:latest")
    .WithEnvironment("POSTGRES_USER", "replicauser")
    .WithEnvironment("POSTGRES_PASSWORD", "replicapass")
    .WithEntrypoint("bash")
    .WithArgs("-c",
        "set -e; " +
        "until pg_isready -h postgres-master -U replicator; do echo waiting-for-master; sleep 2; done; " +
        "if [ -s /var/lib/postgresql/data/PG_VERSION ]; then echo replica2-already-initialized; exec docker-entrypoint.sh postgres; fi; " +
        "rm -rf /var/lib/postgresql/data/*; " +
        "PGPASSWORD=replica_pass pg_basebackup -h postgres-master -D /var/lib/postgresql/data -U replicator -Fp -Xs -P -R; " +
        "exec docker-entrypoint.sh postgres")
    .WithEndpoint(5434, 5432);

builder.AddProject<Projects.WebCCTVUi>("webcctvui");

builder.AddProject<Projects.Trains>("trains");

builder.Build().Run();
