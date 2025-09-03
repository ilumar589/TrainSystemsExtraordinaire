var builder = DistributedApplication.CreateBuilder(args);

// Named volumes equivalent to Compose volumes:
const string PrimaryVolume = "primary_data";
const string Replica1Volume = "replica1_data";
const string Replica2Volume = "replica2_data";

// ---------- PostgreSQL PRIMARY (repmgr) ----------
var pgPrimary = builder.AddContainer("postgresql-primary", "bitnami/postgresql-repmgr", "16")
    .WithContainerName("postgresql-primary")
    // Host:Container port mapping 5432:5432
    .WithEndpoint(name: "postgres", port: 5432, targetPort: 5432, isProxied: false)
    .WithEnvironment("POSTGRESQL_USERNAME", "admin")
    .WithEnvironment("POSTGRESQL_PASSWORD", "adminpass")
    .WithEnvironment("POSTGRESQL_DATABASE", "appdb")
    .WithEnvironment("POSTGRESQL_POSTGRES_PASSWORD", "postgrespass")
    .WithEnvironment("REPMGR_PRIMARY_ROLE_WAIT_FOR_SYNC", "yes")
    .WithEnvironment("REPMGR_PASSWORD", "repmgrpass")
    .WithEnvironment("REPMGR_PORT_NUMBER", "5432")
    .WithEnvironment("REPMGR_PRIMARY_HOST", "postgresql-primary")
    .WithEnvironment("REPMGR_NODE_NAME", "primary-0")
    .WithEnvironment("REPMGR_NODE_NETWORK_NAME", "postgresql-primary")
    .WithEnvironment("REPMGR_PARTNER_NODES", "postgresql-primary,postgresql-replica1,postgresql-replica2")
    .WithEnvironment("REPMGR_LOG_LEVEL", "NOTICE")
    .WithVolume(PrimaryVolume, "/bitnami/postgresql");

// ---------- PostgreSQL REPLICA #1 ----------
var pgReplica1 = builder.AddContainer("postgresql-replica1", "bitnami/postgresql-repmgr", "16")
    .WithContainerName("postgresql-replica1")
    // Host:Container port mapping 5433:5432
    .WithEndpoint(name: "postgres", port: 5433, targetPort: 5432, isProxied: false)
    .WithEnvironment("POSTGRESQL_USERNAME", "admin")
    .WithEnvironment("POSTGRESQL_PASSWORD", "adminpass")
    .WithEnvironment("POSTGRESQL_DATABASE", "appdb")
    .WithEnvironment("POSTGRESQL_POSTGRES_PASSWORD", "postgrespass")
    .WithEnvironment("REPMGR_PASSWORD", "repmgrpass")
    .WithEnvironment("REPMGR_PORT_NUMBER", "5432")
    .WithEnvironment("REPMGR_PRIMARY_HOST", "postgresql-primary")
    .WithEnvironment("REPMGR_NODE_NAME", "replica-1")
    .WithEnvironment("REPMGR_NODE_NETWORK_NAME", "postgresql-replica1")
    .WithEnvironment("REPMGR_PARTNER_NODES", "postgresql-primary,postgresql-replica1,postgresql-replica2")
    .WithVolume(Replica1Volume, "/bitnami/postgresql")
    .WithReference(pgPrimary.GetEndpoint("postgres")
    );

// ---------- PostgreSQL REPLICA #2 ----------
var pgReplica2 = builder.AddContainer("postgresql-replica2", "bitnami/postgresql-repmgr", "16")
    .WithContainerName("postgresql-replica2")
    // Host:Container port mapping 5434:5432
    .WithEndpoint(name: "postgres", port: 5434, targetPort: 5432, isProxied: false)
    .WithEnvironment("POSTGRESQL_USERNAME", "admin")
    .WithEnvironment("POSTGRESQL_PASSWORD", "adminpass")
    .WithEnvironment("POSTGRESQL_DATABASE", "appdb")
    .WithEnvironment("POSTGRESQL_POSTGRES_PASSWORD", "postgrespass")
    .WithEnvironment("REPMGR_PASSWORD", "repmgrpass")
    .WithEnvironment("REPMGR_PORT_NUMBER", "5432")
    .WithEnvironment("REPMGR_PRIMARY_HOST", "postgresql-primary")
    .WithEnvironment("REPMGR_NODE_NAME", "replica-2")
    .WithEnvironment("REPMGR_NODE_NETWORK_NAME", "postgresql-replica2")
    .WithEnvironment("REPMGR_PARTNER_NODES", "postgresql-primary,postgresql-replica1,postgresql-replica2")
    .WithVolume(Replica2Volume, "/bitnami/postgresql")
    .WithReference(pgPrimary.GetEndpoint("postgres")
    );

// ---------- PGPOOL-II (read/write split) ----------
var pgpool = builder.AddContainer("pgpool", "bitnami/pgpool", "4")
    .WithContainerName("pgpool")
    // Host:Container port mapping 9999:5432 (clients connect to localhost:9999)
    .WithEndpoint(name: "pg", port: 9999, targetPort: 5432, isProxied: false)
    .WithEnvironment("PGPOOL_BACKEND_NODES", "0:postgresql-primary:5432,1:postgresql-replica1:5432,2:postgresql-replica2:5432")
    .WithEnvironment("PGPOOL_POSTGRES_USERNAME", "admin")
    .WithEnvironment("PGPOOL_POSTGRES_PASSWORD", "adminpass")
    .WithEnvironment("PGPOOL_SR_CHECK_USER", "postgres")
    .WithEnvironment("PGPOOL_SR_CHECK_PASSWORD", "postgrespass")
    .WithEnvironment("PGPOOL_SR_CHECK_DATABASE", "postgres")
    .WithEnvironment("PGPOOL_ENABLE_LOAD_BALANCING", "yes")
    .WithEnvironment("PGPOOL_NUM_INIT_CHILDREN", "64")
    .WithEnvironment("PGPOOL_HEALTH_CHECK_PERIOD", "5")
    .WithEnvironment("PGPOOL_HEALTH_CHECK_TIMEOUT", "3")
    .WithEnvironment("PGPOOL_FAILOVER_ON_BACKEND_ERROR", "yes")
    .WithEnvironment("PGPOOL_ENABLE_AUTO_FAILOVER", "yes")
    .WithEnvironment("PGPOOL_ENABLE_WATCHDOG", "yes")
    .WithEnvironment("PGPOOL_WD_HEARTBEAT_MODE", "heartbeat")
    .WithEnvironment("PGPOOL_WD_HEARTBEAT_PORT0", "9694")
    .WithEnvironment("PGPOOL_DELEGATE_IP", "pgpool")
    .WithEnvironment("PGPOOL_ADMIN_USERNAME", "pgpooladmin")
    .WithEnvironment("PGPOOL_ADMIN_PASSWORD", "pgpoolpass")
    .WithReference(pgPrimary.GetEndpoint("postgres"))
    .WithReference(pgReplica1.GetEndpoint("postgres"))
    .WithReference(pgReplica2.GetEndpoint("postgres"));

// Optionally, wire your app to Pgpool via a connection string env var:
// - From host: Host=localhost;Port=9999;...
// - From inside the Aspire network: Host=pgpool;Port=5432;...
// Example for a project named "Trains":
// var app = builder.AddProject<Projects.Trains>("trains")
//     .WithEnvironment("ConnectionStrings__AppDb", "Host=pgpool;Port=5432;Database=appdb;Username=admin;Password=adminpass;Pooling=true;")
//     .WithReference(pgpool); // lets Aspire know app depends on pgpool


builder.AddProject<Projects.Trains>("trains")
     .WithEnvironment("DbConnections__TrainsDb", "Host=pgpool;Port=5432;Database=appdb;Username=admin;Password=adminpass;Pooling=true;")
     .WithReference(pgpool.GetEndpoint("postgres")); // lets Aspire know app depends on pgpool;


builder.AddProject<Projects.WebCCTVUi>("webcctvui");


builder.Build().Run();
