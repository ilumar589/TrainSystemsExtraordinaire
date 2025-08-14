# Dockerfile for Postgres replication (same for master & replicas)
FROM postgres:latest

# Optional: Copy custom configuration
COPY ./postgresql.conf /etc/postgresql/postgresql.conf
COPY ./init.sql /docker-entrypoint-initdb.d

# Use custom config
CMD ["postgres", "-c", "config_file=/etc/postgresql/postgresql.conf"]
