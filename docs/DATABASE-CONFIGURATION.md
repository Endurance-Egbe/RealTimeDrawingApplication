# Database configuration

The application uses SQLite through Entity Framework Core by default. An
optional PostgreSQL connection can be configured for online deployments.

## Configure a clone

1. Open [`App.config`](../App.config).
2. Find the `connectionStrings` section.
3. Change the `connectionString` value on the `DrawingDatabase` entry.
4. Build and run the application.

The default configuration is:

```xml
<connectionStrings>
  <add name="DrawingDatabase"
       providerName="System.Data.SQLite"
       connectionString="Data Source={LocalAppData}\RealTimeDrawingApplication\DrawingShapeApplication.db;foreign keys=true;"/>
</connectionStrings>
```

To enable PostgreSQL, add a reachable `OnlineDatabase` entry:

```xml
<add name="OnlineDatabase" providerName="Npgsql"
     connectionString="Host=localhost;Port=5432;Database=drawing;Username=app;Password=change-me;Timeout=3"/>
```

For deployment, the preferred source is the
`REALTIME_DRAWING_POSTGRES_CONNECTION_STRING` environment variable. It takes
precedence over the config-file value:

```powershell
$env:REALTIME_DRAWING_POSTGRES_CONNECTION_STRING = "Host=server;Port=5432;Database=drawing;Username=app;Password=change-me;SSL Mode=Require;Timeout=3"
```

The application tests the environment value first, then `OnlineDatabase` when
a context is created. If the selected connection succeeds, PostgreSQL is
used; if it is empty or unavailable, the application uses `DrawingDatabase`
and continues offline with SQLite. Credentials should be supplied through
deployment configuration, never committed.

See [`.env.example`](../.env.example) for a safe template. It documents the
variable but is not loaded automatically by this .NET Framework application;
the deployment process must set the Windows user, machine, or process
environment variable.

The application tests `OnlineDatabase` when a context is created. If the
connection succeeds, PostgreSQL is selected; if it is empty or unavailable,
the application uses `DrawingDatabase` and continues offline with SQLite.
Credentials should be supplied in deployment configuration, never committed.

The connection string is read from:

- `App.config` when running from Visual Studio.
- `RealTimeDrawingApplication.exe.config` beside the executable after deployment.

The PostgreSQL tables can be provisioned with
[`database/postgresql-schema.sql`](../database/postgresql-schema.sql). It is
safe to run for a new database because it uses `IF NOT EXISTS`. The application
also calls `EnsureCreated()` during startup, so a newly created PostgreSQL
database is provisioned automatically when the connection is reachable.

`{LocalAppData}` is resolved by the application to the current user's local
application-data folder. This is the recommended client-installation default:
the application can write there even when installed under `Program Files`.
To store the database elsewhere, use an absolute Windows path:

```xml
<add name="DrawingDatabase"
     providerName="System.Data.SQLite"
     connectionString="Data Source=C:\Data\RealTimeDrawingApplication.db;foreign keys=true;"/>
```

Create the target directory before starting the application. The application creates the target directory, SQLite file, and schema
automatically before the login window opens.

## Configuring a deployed build

After building, .NET Framework copies `App.config` to a file beside the
executable named:

```text
RealTimeDrawingApplication.exe.config
```

If you distribute the `bin\Release` output, edit the `DrawingDatabase`
connection string in that `.exe.config` file. Do not edit the generated file
and expect the change to appear in source control; make the source change in
`App.config` as well.

## Troubleshooting

- **Missing connection string:** confirm the entry is named exactly
  `DrawingDatabase`.
- **Database is created in the wrong folder:** check the resolved
  `%LOCALAPPDATA%\RealTimeDrawingApplication` folder, or use an absolute path
  and confirm the target directory exists.
- **Database schema errors after an older build:** remove the local
  `DrawingShapeApplication.db` only if its data is disposable, then restart.
  The current application uses `EnsureCreated` for first-run provisioning.
  There is currently no EF migration history. For schema changes to an
  existing production database, update the SQL provisioning script or add a
  versioned EF migration before deploying the application update.
- **Provider errors:** keep `providerName="System.Data.SQLite"` and ensure the
  NuGet packages have been restored. For PostgreSQL, restore both
  `Npgsql.EntityFrameworkCore.PostgreSQL` and `Npgsql` packages.

Never commit production credentials or private database files. SQLite files are
ignored by `.gitignore`; a shared server connection string should be supplied
through deployment configuration rather than committed to the repository.
