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

        public SQLiteEFCore() : base()
        {

        }

        protected override void InitialiseDatabase(DbContextOptionsBuilder optionsBuilder)
        {
            var configuredConnectionString =
                ConfigurationManager.ConnectionStrings[ConnectionStringName]?.ConnectionString;
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

    }
}
