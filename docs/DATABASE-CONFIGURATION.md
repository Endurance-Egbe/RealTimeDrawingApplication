# Database configuration

The application uses SQLite through Entity Framework Core. No database server
is required for the default configuration.

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
  The current application uses `EnsureCreated` for first-run provisioning. A
  future version with schema upgrades should replace this with EF Core
  migrations.
- **Provider errors:** keep `providerName="System.Data.SQLite"` and ensure the
  NuGet packages have been restored.

Never commit production credentials or private database files. SQLite files are
ignored by `.gitignore`; a shared server connection string should be supplied
through deployment configuration rather than committed to the repository.
