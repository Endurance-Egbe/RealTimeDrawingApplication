# PostgreSQL migrations

The application currently provisions a new database with
`database/postgresql-schema.sql` and `EnsureCreated()`. These numbered scripts
are for controlled changes to an existing PostgreSQL database.

Apply the baseline first, then apply migration scripts in filename order with a
PostgreSQL client such as `psql`:

```powershell
psql "$env:REALTIME_DRAWING_POSTGRES_CONNECTION_STRING" `
  -f database/postgresql-schema.sql
psql "$env:REALTIME_DRAWING_POSTGRES_CONNECTION_STRING" `
  -f database/migrations/001_add_schema_migrations.sql
```

Each script is idempotent where practical. Review and test scripts against a
database backup before production use. Record the applied migration number in
`"SchemaMigrations"`; do not delete or recreate a production database.
