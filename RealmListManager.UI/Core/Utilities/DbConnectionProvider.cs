using System;
using System.Data;
using System.Data.SQLite;
using System.IO;

namespace RealmListManager.UI.Core.Utilities
{
    public class DbConnectionProvider
    {
        /// <summary>
        /// Default Database Filename
        /// </summary>
        public const string FileName = "RealmListManager.db";

        /// <summary>
        /// Get a Database Connection
        /// </summary>
        /// <returns>Database Connection</returns>
        public IDbConnection GetConnection()
        {
            var file = Path.Combine(Environment.CurrentDirectory, FileName);
            var connection = new SQLiteConnection($"Data Source={file}");
            connection.Open();
            return connection;
        }
    }
}
