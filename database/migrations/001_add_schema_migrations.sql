-- Migration 001: establish a lightweight SQL migration ledger.
-- Apply database/postgresql-schema.sql before this script.

CREATE TABLE IF NOT EXISTS "SchemaMigrations" (
    "Version" text PRIMARY KEY,
    "AppliedAtUtc" timestamp with time zone NOT NULL DEFAULT CURRENT_TIMESTAMP
);

INSERT INTO "SchemaMigrations" ("Version")
VALUES ('001_add_schema_migrations')
ON CONFLICT ("Version") DO NOTHING;
