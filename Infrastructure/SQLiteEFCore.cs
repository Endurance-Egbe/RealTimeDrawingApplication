using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using System.Configuration;
using System;
using System.IO;

namespace View.Infrastructure
{
    public class SQLiteEFCore : UserDbContext
    {
        private const string ConnectionStringName = "DrawingDatabase";
        private const string OnlineConnectionStringName = "OnlineDatabase";
        public const string OnlineConnectionEnvironmentVariable =
            "REALTIME_DRAWING_POSTGRES_CONNECTION_STRING";

        public SQLiteEFCore() : base()
        {

        }

        protected override void InitialiseDatabase(DbContextOptionsBuilder optionsBuilder)
        {
            string onlineConnectionString;
            if (TryGetOnlineConnection(out onlineConnectionString))
            {
                optionsBuilder.UseNpgsql(onlineConnectionString);
                return;
            }

            var configuredConnectionString = GetLocalConnectionString();
            if (string.IsNullOrWhiteSpace(configuredConnectionString))
            {
                throw new ConfigurationErrorsException(
                    string.Format(
                        "The '{0}' connection string is missing from the application configuration. " +
                        "Update App.config (or the deployed .exe.config) before starting the application.",
                        ConnectionStringName));
            }

            var connectionString = new SqliteConnectionStringBuilder(configuredConnectionString);
            var dataSource = connectionString.DataSource;
            const string localAppDataToken = "{LocalAppData}";
            if (dataSource.StartsWith(localAppDataToken, StringComparison.OrdinalIgnoreCase))
            {
                dataSource = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    dataSource.Substring(localAppDataToken.Length).TrimStart('\\', '/'));
            }
            else if (!Path.IsPathRooted(dataSource))
            {
                dataSource = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    dataSource);
            }

            var databaseDirectory = Path.GetDirectoryName(dataSource);
            if (!string.IsNullOrWhiteSpace(databaseDirectory))
            {
                Directory.CreateDirectory(databaseDirectory);
            }

            connectionString.DataSource = dataSource;
            optionsBuilder.UseSqlite(connectionString.ToString());
        }

        private static bool TryGetOnlineConnection(out string connectionString)
        {
            connectionString = null;
            var environmentConnectionString =
                Environment.GetEnvironmentVariable(OnlineConnectionEnvironmentVariable);
            var configuredConnectionString = ConfigurationManager.ConnectionStrings[OnlineConnectionStringName];
            var onlineConnectionString = !string.IsNullOrWhiteSpace(environmentConnectionString)
                ? environmentConnectionString
                : configuredConnectionString?.ConnectionString;

            if (!string.IsNullOrWhiteSpace(onlineConnectionString))
            {
                try
                {
                    using (var connection = new Npgsql.NpgsqlConnection(onlineConnectionString))
                    {
                        connection.Open();
                        connection.Close();
                        connectionString = onlineConnectionString;
                        return true;
                    }
                }
                catch (Npgsql.NpgsqlException)
                {
                    // The local SQLite database is the supported offline fallback.
                }
                catch (TimeoutException)
                {
                    // The local SQLite database is the supported offline fallback.
                }
            }

            return false;
        }

        private static string GetLocalConnectionString()
        {
            var localConnection = ConfigurationManager.ConnectionStrings[ConnectionStringName]?.ConnectionString;
            if (string.IsNullOrWhiteSpace(localConnection))
            {
                throw new ConfigurationErrorsException(
                    string.Format(
                        "The '{0}' connection string is missing from the application configuration. " +
                        "Update App.config (or the deployed .exe.config) before starting the application.",
                        ConnectionStringName));
            }

            return localConnection;
        }

    }
}
